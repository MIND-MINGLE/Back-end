using System;
using Application.Request.Question;
using Application.Response;


namespace Application.Interface
{
    public interface IQuestionService
    {
        public Task<ApiResponse> AddNewQuestion(QuestionRequest newQuestion);
        public Task<ApiResponse> GetAllQuestions();
        public Task<ApiResponse> GetQuestionById(string questionId);
        public Task<ApiResponse> DeleteQuestion(string questionId);
    }
}
