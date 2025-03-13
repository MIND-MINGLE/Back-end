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
using Microsoft.EntityFrameworkCore;

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
                if (categories.Count() == 0)
                {
                    return response.SetNotFound("Category not found!");
                }

                var categoryIds = categories.Select(c => c.CategoryId).ToList();
                var questions = await _unitOfWorks.QuestionRepo.GetAllAsync(q => categoryIds.Contains(q.CategoryId));
                var questionIds = questions.Select(q => q.QuestionId).ToList();
                var answers = await _unitOfWorks.AnswerRepo.GetAllAsync(a => questionIds.Contains(a.QuestionId));

                var resCategory = categories.Select(c => new ResponseCategory
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.Name,
                    Description = c.Description,
                    Questions = questions.Where(q => q.CategoryId == c.CategoryId).Select(q => new ResponseQuestion
                    {
                        QuestionId = q.QuestionId,
                        QuestionContent = q.QuestionContent,
                        CategoryId = q.CategoryId,
                        CreatedAt = q.CreatedAt,
                        Answers = answers.Where(a => a.QuestionId == q.QuestionId).Select(a => new ResponseAnswer
                        {
                            AnswerId = a.AnswerId,
                            AnswerContent = a.AnswerContent
                        }).ToList()
                    }).ToList(),
                }).ToList();

                return response.SetOk(resCategory);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }

        public async Task<ApiResponse> GetCategoryById(string categoryId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var category = await _unitOfWorks.CategoryRepo.GetAllAsync(c => c.CategoryId == categoryId);
                if (category == null || !category.Any())
                {
                    return response.SetNotFound("Category not found!");
                }

                var questions = await _unitOfWorks.QuestionRepo.GetAllAsync(q => q.CategoryId == categoryId);
                var questionIds = questions.Select(q => q.QuestionId).ToList();
                var answers = await _unitOfWorks.AnswerRepo.GetAllAsync(a => questionIds.Contains(a.QuestionId));

                var resCategory = category.Select(c => new ResponseCategory
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.Name,
                    Description = c.Description,
                    Questions = questions.Where(q => q.CategoryId == c.CategoryId).Select(q => new ResponseQuestion
                    {
                        QuestionId = q.QuestionId,
                        QuestionContent = q.QuestionContent,
                        CategoryId = q.CategoryId,
                        CreatedAt = q.CreatedAt,
                        Answers = answers.Where(a => a.QuestionId == q.QuestionId).Select(a => new ResponseAnswer
                        {
                            AnswerId = a.AnswerId,
                            AnswerContent = a.AnswerContent
                        }).ToList()
                    }).ToList(),
                }).FirstOrDefault();

                return response.SetOk(resCategory);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }

        public Task<ApiResponse> UpdateCategory(CategoryRequest categoryRequest, string categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
