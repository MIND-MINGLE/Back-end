using Application.IRepository;
using Application.Request.Category;
using Application.Response;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICategoryService 
    {
        Task<ApiResponse> AddNewCategory(CategoryRequest categoryRequest);
        Task<ApiResponse> GetAllCategory();
        Task<ApiResponse> GetCategoryById(string categoryId);
        Task<ApiResponse> UpdateCategory(CategoryRequest categoryRequest, string categoryId);
    }
}
