using Application.Interface;
using Application.IRepository;
using Application.Request.PatientResponse;
using Application.Response;
using Application.Response.PatientResponse;
using AutoMapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PatientResponseService : IPatientResponseService
    {
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper _mapper;

        public PatientResponseService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            this.unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddResponseAsync(PatientResRequest request)
        {
            if (request == null)
                return new ApiResponse().SetBadRequest(message: "Request data is null");
            try
            {
                // Ánh xạ từ PatientResRequest sang PatientResponse
                var response = _mapper.Map<PatientResponse>(request);
                await unitOfWorks.PatientResponseRepo.AddAsync(response);
                await unitOfWorks.SaveChangeAsync();
                // Ánh xạ từ PatientResponse sang PatientResResponse để trả về
                var responseDto = _mapper.Map<PatientResResponse>(response);
                return new ApiResponse().SetOk(responseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error adding response: {ex.Message}");
            }
        }
       

        public async Task<ApiResponse> GetResponsesBySurveyIdAsync(string surveyId, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var responses = await unitOfWorks.PatientResponseRepo.GetResponsesBySurveyIdAsync(surveyId, pageIndex, pageSize);

                // Ánh xạ danh sách PatientResponse sang danh sách PatientResResponse
                var responseDtos = _mapper.Map<List<PatientResResponse>>(responses);
                return new ApiResponse().SetOk(responseDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching responses: {ex.Message}");
            }
        }
        public async Task<ApiResponse> ComposeResponse(PatientResRequest[] request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                // Validate input
                if (request == null || !request.Any())
                {
                    return response.SetBadRequest("Request is empty or null");
                }

                string patientSurveyId = request.First().PatientSurveyId;
                if (string.IsNullOrEmpty(patientSurveyId))
                {
                    return response.SetBadRequest("PatientSurveyId is required");
                }

                var patientSurvey = await unitOfWorks.PatientSurveyRepo.GetAsync(ps => ps.PatientSurveyId == patientSurveyId);
                if (patientSurvey == null)
                {
                    return response.SetBadRequest($"PatientSurvey with ID {patientSurveyId} not found");
                }

                // Process responses and group by CategoryId
                var patientResponses = new List<PatientResponse>();
                var scoresByCategory = new Dictionary<string, int>(); // CategoryId -> Total Score

                foreach (var req in request)
                {
                    // Validate QuestionId and get CategoryId
                    var question = await unitOfWorks.QuestionRepo.GetAsync(q => q.QuestionId == req.QuestionId);
                    if (question == null)
                    {
                        return response.SetBadRequest($"Question ID {req.QuestionId} not found");
                    }

                    var answer = await unitOfWorks.AnswerRepo.GetAsync(a => a.AnswerId == req.AnswerId && a.QuestionId == req.QuestionId);
                    if (answer == null)
                    {
                        return response.SetBadRequest($"Answer ID {req.AnswerId} not valid for Question ID {req.QuestionId}");
                    }

                    // Create PatientResponse
                    var patientResponse = new PatientResponse
                    {
                        PatientResponseId = Guid.NewGuid().ToString(),
                        PatientSurveyId = patientSurveyId,
                        QuestionId = req.QuestionId,
                        AnswerId = req.AnswerId,
                        CustomAnswer = req.CustomAnswer,
                        Score = answer.Score
                    };
                    patientResponses.Add(patientResponse);

                    // Aggregate score by CategoryId
                    if (!scoresByCategory.ContainsKey(question.CategoryId))
                    {
                        scoresByCategory[question.CategoryId] = 0;
                    }
                    scoresByCategory[question.CategoryId] += answer.Score;
                }

                // Generate summary diagnosis for each category
                var summaries = new Dictionary<string, string>(); // CategoryId -> Diagnosis
                foreach (var categoryId in scoresByCategory.Keys)
                {
                    var category = await unitOfWorks.CategoryRepo.GetAsync(c => c.CategoryId == categoryId);
                    if (category != null)
                    {
                        summaries[categoryId] = GetDiagnosisSummary(category.Name, scoresByCategory[categoryId]);
                    }
                    else
                    {
                        summaries[categoryId] = "Unknown Category";
                    }
                }

                // Combine summaries into a single string
                string combinedSummary = string.Join("; ", summaries.Select(s =>
                    $"{unitOfWorks.CategoryRepo.GetAsync(c => c.CategoryId == s.Key).Result?.Name}: {s.Value}"));
                patientSurvey.Summary = combinedSummary;

                // Update PatientSurvey with summary
                await unitOfWorks.PatientSurveyRepo.UpdateFieldAsync(patientSurvey.PatientSurveyId, ps => ps.Summary, combinedSummary);

                // Save all PatientResponses
                await unitOfWorks.PatientResponseRepo.AddRangeAsync(patientResponses);

                // Prepare response data
                var responseData = new
                {
                    PatientSurveyId = patientSurveyId,
                    Scores = scoresByCategory.Select(kvp => new
                    {
                        CategoryId = kvp.Key,
                        CategoryName = unitOfWorks.CategoryRepo.GetAsync(c => c.CategoryId == kvp.Key).Result?.Name,
                        TotalScore = kvp.Value
                    }).ToList(),
                    Summary = combinedSummary
                };

                return response.SetOk(responseData);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error processing responses: {ex.Message}");
            }
        }

        private string GetDiagnosisSummary(QuestionType categoryType, int score)
        {
            // Cap at 10 as per your requirement
            score = Math.Min(score, 10);
            switch (categoryType)
            {
                case QuestionType.PHQ9: // PHQ-9: Depression (0-10)
                    if (score >= 8) return "Severe Depression";
                    if (score >= 6) return "Moderate Depression";
                    if (score >= 4) return "Mild Depression";
                    if (score >= 2) return "Minimal Depression";
                    return "No Depression";

                case QuestionType.GAD7: // GAD-7: Anxiety (0-10)
                    if (score >= 8) return "Severe Anxiety";
                    if (score >= 6) return "Moderate Anxiety";
                    if (score >= 4) return "Mild Anxiety";
                    if (score >= 2) return "Minimal Anxiety";
                    return "No Anxiety";

                case QuestionType.PCPTSD5: // PC-PTSD-5: PTSD (0-10, but typically cutoff-based)
                    if (score >= 3) return "PTSD Likely"; // Adjusted to fit 0-10, keeping cutoff logic
                    return "PTSD Unlikely";

                default:
                    return "Unknown Category";
            }
        }
    }
}
