using AutoMapper;
using EBC.Core.Constants;
using EBC.Core.CustomFilter.WorkFilter.Core;
using EBC.Core.Entities.Common;
using EBC.Core.Models.FilterModels.Common;
using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.EF;

namespace EBC.Core.BaseContents.Controllers;

public class BaseController<TEntity, TDTO, TCreateDTO, TEditDTO, TFilterModel> : Controller
    where TEntity : BaseEntity<Guid>, new()
        where TDTO : class, new()
        where TCreateDTO : class
        where TEditDTO : class
        where TFilterModel : BaseFilterModel
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    public BaseController(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //Elaqeli olan Table-lar (Join) ucun SelectBox Numnunesi
    protected virtual Task GetFields()
    {
        // ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
        return Task.CompletedTask;
    }

    #region Actions
    [HttpGet]
    public virtual async Task<IActionResult> Index(TFilterModel model, int page = 1)
    {
        ViewBag.Model = model;
        Expression<Func<TEntity, bool>> predicate = BaseFilterAlgorithm<TEntity>.GenerateFilterExpression(model);
        var result = predicate == null
            ? await GetAll(page)
            : await GetAll(predicate, page);
        return View(result);
    }

    [HttpGet]
    public virtual IActionResult Create()
    {
        GetFields().Wait();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Create(TCreateDTO t)
    {
        if (!ModelState.IsValid)
            return View(t);

        var result = await Add(t);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, result.ExceptionMessage);
            return View(t);
        }
    }

    [HttpGet]
    public virtual async Task<IActionResult> Edit(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound();

        var data = await GetById(x => x.Id == id);

        if (data == null)
            return NotFound();

        GetFields().Wait();

        var editDto = _mapper.Map<TEditDTO>(data);

        return View("Edit", editDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Edit(TEditDTO t)
    {
        if (!ModelState.IsValid)
            return View(t);

        var result = await Update(t);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, result.ExceptionMessage);
            GetFields().Wait();
            return View(t);
        }
    }

    [HttpGet]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound();

        var result = await SoftDelete(id);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));
        else
            return BadRequest();
    }

    [HttpGet]
    public virtual IActionResult Remove(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound();

        var result = HardDelete(id);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));
        else
            return BadRequest();
    }
    #endregion

    #region Methods
    protected virtual async Task<IPagedList<TDTO>> GetAll(int pageNumber = 1, int pageSize = 10, bool noTracking = true)
        => await _repository
            .GetAllQueryable(x => true, noTracking, x => x.OrderByDescending(i => i.CreatedDate))
            .Select(entity => _mapper.Map<TDTO>(entity))
            .ToPagedListAsync(pageNumber, pageSize);

    protected virtual async Task<IPagedList<TDTO>> GetAll(Expression<Func<TEntity, bool>> predicate, int pageNumber = 1, int pageSize = 10, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        => await _repository
            .GetAllQueryable(predicate, noTracking, x => x.OrderByDescending(i => i.CreatedDate), includes)
            .Select(entity => _mapper.Map<TDTO>(entity))
            .ToPagedListAsync(pageNumber, pageSize);

    protected virtual async Task<TDTO> GetById(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        var entity = await _repository.GetFirstOrDefaultAsync(predicate, noTracking, includes);
        return _mapper.Map<TDTO>(entity);
    }

    protected virtual async Task<Result<TCreateDTO>> Add(TCreateDTO entity)
        => await Result<TCreateDTO>.ExecuteWithHandlingAsync(async () =>
        {
            if (entity == null)
                return Result<TCreateDTO>.Failure(ExceptionMessage.IsNull);

            var _entity = _mapper.Map<TEntity>(entity);

            return await _repository.AddAsync(_entity) > 0
                ? Result<TCreateDTO>.Success(entity)
                : Result<TCreateDTO>.Failure(ExceptionMessage.ErrorOccured);
        });


    protected virtual async Task<Result<TEditDTO>> Update(TEditDTO entity)
        => await Result<TEditDTO>.ExecuteWithHandlingAsync(async () =>
        {
            if (entity == null)
                return Result<TEditDTO>.Failure(ExceptionMessage.IsNull);

            var _entity = _mapper.Map<TEntity>(entity);

            return await _repository.UpdateAsync(_entity) > 0
                ? Result<TEditDTO>.Success(entity)
                : Result<TEditDTO>.Failure(ExceptionMessage.ErrorOccured);
        });


    protected virtual async Task<Result> SoftDelete(Guid id)
        => await Result.ExecuteWithHandlingAsync(async () =>
            await _repository.SoftDeleteAsync(id) > 0
                ? Result.Success()
                : Result.Failure(ExceptionMessage.ErrorOccured)
        );

    protected virtual Result HardDelete(Guid id)
        => Result.ExecuteWithHandling(() =>
            _repository.Delete(id) > 0
                ? Result.Success()
                : Result.Failure(ExceptionMessage.ErrorOccured)
        );

    #endregion

}

