using Application.Interface;
using Application.Request.Question;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class QuestionService : IQuestionService
    {
        private IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public QuestionService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddNewQuestion(QuestionRequest newQuestion)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var question = _mapper.Map<Question>(newQuestion);
                await _unitOfWorks.QuestionRepo.AddAsync(question);
                await _unitOfWorks.SaveChangeAsync();
                response.SetOk(newQuestion);
                return response;
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }

        public async Task<ApiResponse> DeleteQuestion(string questionId)
        {
            ApiResponse response = new ApiResponse();

            var question = await _unitOfWorks.QuestionRepo.GetAsync(x => x.QuestionId == questionId);
            if (question == null)
            {
                response.SetNotFound("Question not found!");
                return response;
            }
            await _unitOfWorks.QuestionRepo.RemoveByIdAsync(question);
            response.SetOk(question);
            return response;
        }   

        public async Task<ApiResponse> GetAllQuestions()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var questions = await _unitOfWorks.QuestionRepo.GetAllAsync(null);
                if (questions.Count() == 0)
                {
                    return response.SetNotFound("No question found!");
                }

                var questionIds = questions.Select(q => q.QuestionId).ToList();
                var answers = await _unitOfWorks.AnswerRepo.GetAllAsync(a => questionIds.Contains(a.QuestionId));

                var resQuestions = questions.Select(q => new ResponseQuestion
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.QuestionContent,
                    QuestionType = q.QuestionType,
                    CategoryId = q.CategoryId,
                    CreatedAt = q.CreatedAt,
                    Answers = answers.Where(a => a.QuestionId == q.QuestionId).Select(a => new ResponseAnswer
                    {
                        AnswerId = a.AnswerId,
                        AnswerContent = a.AnswerContent
                    }).ToList()
                }).ToList();

                return response.SetOk(resQuestions);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }

        public async Task<ApiResponse> GetQuestionById(string questionId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var question = await _unitOfWorks.QuestionRepo.GetAsync(x => x.QuestionId == questionId);
                if (question == null)
                {
                    return response.SetNotFound("Question not found!");
                }

                var answers = await _unitOfWorks.AnswerRepo.GetAllAsync(a => a.QuestionId == questionId);

                var resQuestion = new ResponseQuestion
                {
                    QuestionId = question.QuestionId,
                    QuestionContent = question.QuestionContent,
                    QuestionType = question.QuestionType,
                    CategoryId = question.CategoryId,
                    CreatedAt = question.CreatedAt,
                    Answers = answers.Select(a => new ResponseAnswer
                    {
                        AnswerId = a.AnswerId,
                        AnswerContent = a.AnswerContent
                    }).ToList()
                };

                return response.SetOk(resQuestion);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }
    }
}
