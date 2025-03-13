using Application.Interface;
using Application.Request.Category;
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
    public class CategoryService : ICategoryService
    {
        private IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;
        private IQuestionService _questionService;

        public CategoryService(IUnitOfWorks unitOfWorks, IMapper mapper, IQuestionService questionService)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _questionService = questionService;
        }

        public async Task<ApiResponse> AddNewCategory(CategoryRequest categoryRequest)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var category = _mapper.Map<Category>(categoryRequest);
                await _unitOfWorks.CategoryRepo.AddAsync(category);
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk(categoryRequest);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }

        public async Task<ApiResponse> GetAllCategory()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var categories = await _unitOfWorks.CategoryRepo.GetAllAsync(null);
                var resCategory = _mapper.Map<List<ResponseCategory>>(categories);
                var questions = await _unitOfWorks.QuestionRepo.GetAllAsync(null);
                var resQuestions = _mapper.Map<List<ResponseQuestion>>(questions); 

                if (resCategory.Count == 0)
                {
                    return response.SetNotFound("Category not found!");
                }
                return response.SetOk(resCategory);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }

        public Task<ApiResponse> GetCategoryById(string categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateCategory(CategoryRequest categoryRequest, string categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
