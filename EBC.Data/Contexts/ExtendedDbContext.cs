using EBC.Core.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Contexts;

public class ExtendedDbContext : BaseDbContext
{
	public ExtendedDbContext() : base() {}
	public ExtendedDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }
}
