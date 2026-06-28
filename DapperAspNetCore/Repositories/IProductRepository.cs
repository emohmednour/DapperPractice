using DapperAspNetCore.Entities;

namespace DapperAspNetCore.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<int> CreateAsync(Product product);

        Task<int> UpdateAsync(int id,Product product);
        Task<int> DeleteAsync(int id);


        Task<bool> CreateBulkAsync(IEnumerable<Product> products);

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    }
}
