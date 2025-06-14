using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper) {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(CreateProductDTO product)
        {
            await _productRepository.AddAsync(_mapper.Map<Product>(product));
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            var prroducts = await _productRepository.GetAllAsync();
            return _mapper.Map<List<ProductDTO>>(prroducts);
        }

        public async Task<ProductDTO> GetAsync(int id)
        {
            return _mapper.Map<ProductDTO>(await _productRepository.GetAsync(id));
        }

        public async Task UpdateAsync(UpdateRecordDTO product)
        {
            await _productRepository.UpdateAsync(_mapper.Map<Product>(product));
        }
    }
}
