using AutoMapper;
using EBC.Core.BaseContents.Controllers;
using EBC.Core.Caching.Abstract;
using EBC.Core.Models.Enums;
using EBC.Data.DTOs.QuestionLevel;
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
    public class QuestionLevelController : BaseController<QuestionLevel, QuestionLevelViewDTO, QuestionLevelCreateDTO, QuestionLevelEditDTO, QuestionLevelFilterModel>
    {
        private readonly IMapper _mapper;
        private readonly ICachingService<IMemoryCache> _cachingService;
        private readonly IQuestionLevelRepository _repository;

        public QuestionLevelController(
            IQuestionLevelRepository repository,
            ICachingService<IMemoryCache> cachingService,
            IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _cachingService = cachingService;
        }


        [AutoGenerateActionView(MethodType.List, typeof(QuestionLevelViewDTO))]
        public override async Task<IActionResult> Index(QuestionLevelFilterModel model, int page = 1)
            => await base.Index(model, page);


        [AutoGenerateActionView(MethodType.Create, typeof(QuestionLevelCreateDTO))]
        public override IActionResult Create()
            => base.Create();


        [AutoGenerateActionView(MethodType.Edit, typeof(QuestionLevelEditDTO))]
        public override Task<IActionResult> Edit(Guid id)
            => base.Edit(id);

    }
}
