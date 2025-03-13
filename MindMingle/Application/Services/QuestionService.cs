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
                var resQuestions = _mapper.Map<List<ResponseQuestion>>(questions);
                if (resQuestions.Count == 0)
                {
                    return response.SetNotFound("No question found!");
                }
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
                return response.SetOk(question);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }
    }
}
