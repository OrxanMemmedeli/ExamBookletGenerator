using AutoMapper;
using EBC.Core.BaseContents.Controllers;
using EBC.Core.Caching.Abstract;
using EBC.Core.Models.Enums;
using EBC.Data.DTOs.UserType;
using EBC.Data.Entities;
using EBC.Data.FilterModels;
using EBC.Data.Repositories.Abstract;
using EBC.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ExamBookletGenerator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoGenerateActionView]
    public class UserTypeController : BaseController<UserType, UserTypeViewDTO, UserTypeCreateDTO, UserTypeEditDTO, UserTypeFilterModel>
    {
        private readonly IMapper _mapper;
        private readonly ICachingService<IMemoryCache> _cachingService;
        private readonly IUserTypeRepository _repository;

        public UserTypeController(
            IUserTypeRepository repository,
            ICachingService<IMemoryCache> cachingService,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _cachingService = cachingService;
        }


        [AutoGenerateActionView(MethodType.List, typeof(UserTypeViewDTO))]
        public override async Task<IActionResult> Index(UserTypeFilterModel model, int page = 1)
            => await base.Index(model, page);


        [AutoGenerateActionView(MethodType.Create, typeof(UserTypeCreateDTO))]
        public override IActionResult Create()
            => base.Create();


        [AutoGenerateActionView(MethodType.Edit, typeof(UserTypeEditDTO))]
        public override Task<IActionResult> Edit(Guid id)
            => base.Edit(id);

    }
}
