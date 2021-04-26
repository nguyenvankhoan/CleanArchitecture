using CleanArchitecture.Application.DatabaseServices;
using CleanArchitecture.Application.Models;
using System;
using System.Collections.Generic;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using SqlKata.Execution;
using SqlKata.Compilers;

namespace CleanArchitecture.Infrastructure.DatabaseServices
{
    public class ProductService : IProductService
    {
        private readonly IDatabaseConnectionFactory _database;

        public ProductService(IDatabaseConnectionFactory database)
        {
            _database = database;
        }

        public async Task<bool> CreateProduct(Product request)
        {
            using var conn = await _database.CreateConnectionAsync();
            var db = new QueryFactory(conn, new SqlServerCompiler());

            var affectedRecords = await db.Query("ProductType").InsertAsync(new
            {
                ProductID = Guid.NewGuid(),
                ProductTypeID = request.ProductTypeID,
                ProductKey = request.ProductKey,
                ProductName = request.ProductName,
                ProductImageUri = request.ProductImageUri,
                RecordStatus = request.RecordStatus,
                CreatedDate = DateTime.UtcNow,
                UpdatedUser = Guid.NewGuid()
            });

            return affectedRecords > 0;
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            using var conn = await _database.CreateConnectionAsync();

            var parameters = new
            {
                ProductID = productId
            };
            var affectedRecords = await conn.ExecuteAsync("UPDATE ProductType SET RecordStatus = 2 where ProductID = @ProductID",
                parameters);
            return affectedRecords > 0;
        }

        public async Task<IEnumerable<Product>> FetchProduct()
        {
            using var conn = await _database.CreateConnectionAsync();
            //var db = new QueryFactory(conn, new SqlServerCompiler());
            //var result = db.Query("ProductType");
            //return await result.GetAsync<ProductTypeResponseModel>();

            var result = conn.Query<Product>("Select * from Product").ToList();
            return result;
        }

        public async Task<Product> FindProduct(Guid productId)
        {
            using var conn = await _database.CreateConnectionAsync();

            var result = conn.QueryFirstOrDefault<Product>("Select * from Product where ProductId = " + productId);
            return result;
        }

        public async Task<bool> UpdateProduct(Product request)
        {
            using var conn = await _database.CreateConnectionAsync();
            var db = new QueryFactory(conn, new SqlServerCompiler());

            var result = await db.Query("ProductType").Where("ProductID", "=", request.ProductID)
                .FirstOrDefaultAsync<ProductType>();

            if (result == null)
                return false;

            var affectedRecords = await db.Query("ProductType").UpdateAsync(new
            {
                ProductTypeID = request.ProductTypeID,
                ProductKey = request.ProductKey,
                ProductName = request.ProductName,
                ProductImageUri = request.ProductImageUri,
                RecordStatus = request.RecordStatus,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = request.UpdatedUser
            });

            return affectedRecords > 0;
        }

        private async Task<bool> IsProductKeyUnique(QueryFactory db, string productKey, Guid productID)
        {
            var result = await db.Query("ProductType").Where("ProductKey", "=", productKey)
                .FirstOrDefaultAsync<ProductType>();

            if (result == null)
                return true;

            return result.ProductTypeID == productID;
        }
    }
}
