using AutoMapper;
using EBC.Core.Attributes;
using EBC.Core.BaseContents.Controllers;
using EBC.Core.Caching.Abstract;
using EBC.Core.Models.Enums;
using EBC.Data.DTOs.AcademicYear;
using EBC.Data.Entities;
using EBC.Data.FilterModels;
using EBC.Data.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ExamBookletGenerator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoGenerateActionView]
    public class AcademicYearController : BaseController<AcademicYear, AcademicYearViewDTO, AcademicYearCreateDTO, AcademicYearEditDTO, AcademicYearFilterModel>
    {
        private readonly IMapper _mapper;
        private readonly ICachingService<IMemoryCache> _cachingService;
        private readonly IAcademicYearRepository _repository;

        public AcademicYearController(
            IAcademicYearRepository repository,
            ICachingService<IMemoryCache> cachingService,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _cachingService = cachingService;
        }


        [AutoGenerateActionView(MethodType.List, typeof(AcademicYearViewDTO))]
        public override async Task<IActionResult> Index(AcademicYearFilterModel model, int page = 1)
            => await base.Index(model, page);


        [AutoGenerateActionView(MethodType.Create, typeof(AcademicYearCreateDTO))]
        public override IActionResult Create()
            => base.Create();


        [AutoGenerateActionView(MethodType.Edit, typeof(AcademicYearEditDTO))]
        public override Task<IActionResult> Edit(Guid id)
            => base.Edit(id);

    }
}
