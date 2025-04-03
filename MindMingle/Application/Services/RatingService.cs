using Application.Interface;
using Application.Request.Rating;
using Application.Response;
using Application.Response.Rating;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddRatingAsync(RatingRequest request)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var rating = _mapper.Map<Rating>(request);
                await _unitOfWorks.RatingRepo.AddAsync(rating);
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk(request);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllRatingAsync()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var ratings = await _unitOfWorks.RatingRepo.GetAllAsync(null);
                if (ratings.Count == 0)
                {
                    return response.SetNotFound("No ratings here");
                }
                var resRating = _mapper.Map<List<ResponseRating>>(ratings);
                return response.SetOk(resRating);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetRatingByAppointmentIdAsync(string appointmentId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var ratings = await _unitOfWorks.RatingRepo.GetAsync(x => x.AppointmentId == appointmentId);
                var resRating = _mapper.Map<ResponseRating>(ratings);
                return response.SetOk(resRating);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetRatingByPatientIdAsync(string patientId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var ratings = await _unitOfWorks.RatingRepo.GetAsync(x => x.PatientId == patientId);
                var resRating = _mapper.Map<ResponseRating>(ratings);
                return response.SetOk(resRating);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
    }
}
