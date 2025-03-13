using Application.Interface;
using Application.Request.Answer;
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
    public class AnswerService : IAnswerService
    {
        private IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public AnswerService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddNewAnswer(NewAnswerRequest newAnswer)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var answer = _mapper.Map<Answer>(newAnswer);
                await _unitOfWorks.AnswerRepo.AddAsync(answer);
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk(newAnswer);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }
    }
}
