using Dapper;
using DapperAspNetCore.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperAspNetCore.Repositories
{
    public class ProductRepository(IConfiguration configuration) : IProductRepository
    {
        private readonly string _connectionstring = configuration.GetConnectionString("cs") ?? string.Empty;

        public async Task<int> CreateAsync(Product product)
        {
            var query = @"INSERT INTO Products (Name, Price, CategoryId) 
                         VALUES (@Name, @Price, @CategoryId);
                          Select Cast(Scope_Identity() as int)";

                                                    
            using var connection = new SqlConnection(_connectionstring);
            return await connection.ExecuteAsync(query, product);
        }

        public async Task<bool> CreateBulkAsync(IEnumerable<Product> products)
        {
            using var connection = new SqlConnection(_connectionstring);

            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try {

                var query = "INSERT INTO Products (Name, Price, CategoryId) VALUES (@Name, @Price, @CategoryId)";

               await connection.ExecuteAsync(query, products,transaction);

                await transaction.CommitAsync();
                    return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return false;
            
            
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "Delete FROM Products WHERE Id = @id";

            using var connection = new SqlConnection(_connectionstring);

            return await connection.ExecuteAsync(query, new {id});
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = "SELECT * FROM Products";
            using var connection = new SqlConnection(_connectionstring);
            return await connection.QueryAsync<Product>(query);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
           
            using var connection = new SqlConnection(_connectionstring);
            return await 
                connection.QueryAsync<Product>(
                   sql: "sp_GetProductsByCategoryId",
                    param: new {CategoryId =  categoryId },
                    commandType: CommandType.StoredProcedure
                );

        }


      









        public async Task<Product?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @id";
            using var connection = new SqlConnection(_connectionstring);
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { id });
        }

        public async Task<int> UpdateAsync(int id, Product product)
        {
            var query = "Update  Products set Name = @Name , Price= @Price, CategoryId = @CategoryId WHERE Id = @id";

            using var connection = new SqlConnection(_connectionstring);

         

            return await connection.ExecuteAsync(query,product); 
        }
    }
}