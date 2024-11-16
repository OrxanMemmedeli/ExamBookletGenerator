using AutoMapper;

public static class MapperExtensions
{
    /// <summary>
    /// Dinamik olaraq CreatedUser və ModifiedUser üçün xəritələmə yaradır.
    /// </summary>
    /// <typeparam name="TSource">Entity növü</typeparam>
    /// <typeparam name="TDestination">DTO növü</typeparam>
    /// <param name="mappingExpression">Xəritələmə ifadəsi</param>
    /// <param name="createdUserNameField">CreatedUserName sahəsi üçün xəritələmə</param>
    /// <param name="modifiedUserNameField">ModifiedUserName sahəsi üçün xəritələmə</param>
    /// <returns>Xəritələmə ifadəsi</returns>
    public static IMappingExpression<TSource, TDestination> MapAuditableFields<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> mappingExpression,
        string createdUserNameField = "CreatedUserName",
        string modifiedUserNameField = "ModifiedUserName")
        where TSource : class
        where TDestination : class
    {
        return mappingExpression
            .ForMember(createdUserNameField, 
                opt => opt.MapFrom(src => 
                    src.GetType()
                        .GetProperty("CreatedUser")?
                        .GetValue(src, null)?
                        .GetType()
                        .GetProperty("UserName")?
                        .GetValue(src.GetType().GetProperty("CreatedUser")?.GetValue(src, null), null)))
            
            
            
            .ForMember(modifiedUserNameField, opt => opt.MapFrom(src => src.GetType().GetProperty("ModifiedUser")?.GetValue(src, null)?.GetType().GetProperty("UserName")?.GetValue(src.GetType().GetProperty("ModifiedUser")?.GetValue(src, null), null)));
    }
}
