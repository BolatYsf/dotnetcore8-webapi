using AutoMapper;
using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Dto;
using App.Application.Features.Categories.Update;
using App.Application.Contracts.Persistence;
using App.Domain.Entities;

namespace App.Application.Features.Categories
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
            var category = await categoryRepository.GetCategoryWithProductsAsync();

            var categoryDto = mapper.Map<List<CategoryWithProductsDto>>(category);

            return ServiceResult<List<CategoryWithProductsDto>>.Success(categoryDto);
        }


        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAllAsync();

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
            var anyCategory = await categoryRepository.AnyAsync(x => x.Name == request.Name);

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
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return ServiceResult.Fail("Category not found", System.Net.HttpStatusCode.NotFound);
            }

            var iscategoryNameExist = await categoryRepository.AnyAsync(x => x.Name == request.Name && x.Id != category.Id);

            if (iscategoryNameExist)
            {
                return ServiceResult.Fail("Category is already exists", System.Net.HttpStatusCode.BadRequest);
            }

            category = mapper.Map(request, category);

            categoryRepository.Update(category);
            await unitOfWork.SavechangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);

        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return ServiceResult.Fail("Category not found", System.Net.HttpStatusCode.NotFound);
            }

            categoryRepository.Delete(category);
            await unitOfWork.SavechangesAsync();

            return ServiceResult.Success(System.Net.HttpStatusCode.NoContent);
        }



    }
}
