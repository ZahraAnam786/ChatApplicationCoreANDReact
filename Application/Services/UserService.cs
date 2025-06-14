using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    ///     Add any custom business logic (methods) here
    /// </summary>
    public interface IUserService : IService<User>
    {
        List<UserDTO> GetALLUser();
        UserDTO GetCurrentUser(int UserID);
    }

    /// <summary>
    ///     All methods that are exposed from Repository in Service are overridable to add business logic,
    ///     business logic should be in the Service layer and not in repository for separation of concerns.
    /// </summary>
    public class UserService : Service<User>, IUserService
    {
        private readonly IRepositoryAsync<User> _repository;
        private readonly IMapper _mapper;

        public UserService(IRepositoryAsync<User> repository, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;   
        }

        public List<UserDTO> GetALLUser()  //No need try catch using Middleware Custom Exception Handling. 
        {
            var users = _repository.Queryable().ToList();
           return _mapper.Map<List<UserDTO>>(users);

        }

        public UserDTO GetCurrentUser(int UserID)  //No need try catch using Middleware Custom Exception Handling. 
        {
            var users = _repository.Queryable().FirstOrDefault(e => e.UserID == UserID);
            return _mapper.Map<UserDTO>(users);
        }
    }
    
}
