using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetAsync(int id);
        Task DeleteAsync(int id);
        Task AddAsync(CreateProductDTO product);
        Task UpdateAsync(UpdateRecordDTO product);

    }
}
