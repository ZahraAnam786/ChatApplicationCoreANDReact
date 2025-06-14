using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
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
    public interface IUserMessagesService : IService<UserMessages>
    {
    }

    /// <summary>
    ///     All methods that are exposed from Repository in Service are overridable to add business logic,
    ///     business logic should be in the Service layer and not in repository for separation of concerns.
    /// </summary>
    public class UserMessagesService : Service<UserMessages>, IUserMessagesService
    {
        private readonly IRepositoryAsync<UserMessages> _repository;
        private readonly IMapper _mapper;

        public UserMessagesService(IRepositoryAsync<UserMessages> repository, IMapper mapper) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

      
    }

}


