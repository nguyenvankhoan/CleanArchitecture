using CleanArchitecture.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DatabaseServices
{
   public interface IProductService
    {
        Task<bool> CreateProduct(Product request);
        Task<bool> DeleteProduct(Guid productId);
        Task<bool> UpdateProduct(Product request);
        Task<IEnumerable<Product>> FetchProduct();
        Task<Product> FindProduct(Guid productId);
    }
}
