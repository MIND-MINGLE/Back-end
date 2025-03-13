using Application.Request.Answer;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAnswerService
    {
        Task<ApiResponse> AddNewAnswer(NewAnswerRequest newAnswer);
    }
}
