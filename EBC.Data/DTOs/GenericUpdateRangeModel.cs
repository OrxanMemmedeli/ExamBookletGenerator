using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EBC.Data.DTOs;

public class GenericUpdateRangeModel<T> where T : class
{
    public T t { get; set; }
    public Action<EntityEntry<T>> rules { get; set; }
}
