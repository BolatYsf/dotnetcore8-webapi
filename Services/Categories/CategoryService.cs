using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {

        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProducts(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);

            if (category is null)
            {
                return ServiceResult<CategoryWithProductsDto>.Fail("Category not found", System.Net.HttpStatusCode.NotFound);
            }

            var categoryDto = mapper.Map<CategoryWithProductsDto>(category);

            return ServiceResult<CategoryWithProductsDto>.Success(categoryDto);
        }

        public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProducts()
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync().ToListAsync();

            var categoryDto = mapper.Map<List<CategoryWithProductsDto>>(category);

            return ServiceResult<List<CategoryWithProductsDto>>.Success(categoryDto);
        }


        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();

            var caegoriesDto = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.Success(caegoriesDto);
        }

        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return ServiceResult<CategoryDto>.Fail("Category not found", System.Net.HttpStatusCode.NotFound);
            }

            var categoryDto = mapper.Map<CategoryDto>(category);

            return ServiceResult<CategoryDto>.Success(categoryDto);
        }


        // crud operation

        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var anyCategory = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();

            if (anyCategory)
            {
                return ServiceResult<int>.Fail("Category name already is exists", System.Net.HttpStatusCode.NotFound);
            }

            var newCategory = mapper.Map<Category>(request);

            await categoryRepository.AddAsync(newCategory);

            await unitOfWork.SavechangesAsync();

            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            //var category = await categoryRepository.GetByIdAsync(id);

            //if (category == null)
            //{
            //    return ServiceResult.Fail("Category not found", System.Net.HttpStatusCode.NotFound);
            //}

            var iscategoryNameExist = await categoryRepository.Where(x => x.Name == request.Name && x.Id != id).AnyAsync();

            if (iscategoryNameExist)
            {
                return ServiceResult.Fail("Category is already exists", System.Net.HttpStatusCode.BadRequest);
            }

            var category = mapper.Map<Category>(request);
            category.Id = id;

            categoryRepository.Update(category);
            await unitOfWork.SavechangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);

        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            //if (category == null)
            //{
            //    return ServiceResult.Fail("Category not found", System.Net.HttpStatusCode.NotFound);
            //}

            categoryRepository.Delete(category);
            await unitOfWork.SavechangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
        }



    }
}
