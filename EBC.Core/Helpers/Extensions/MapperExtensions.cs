using AutoMapper;

public static class MapperExtensions
{
    /// <summary>
    /// Dinamik olaraq CreatedUser və ModifiedUser üçün xəritələmə yaradır.
    /// </summary>
    /// <typeparam name="TSource">Entity növü</typeparam>
    /// <typeparam name="TDestination">DTO növü</typeparam>
    /// <param name="mappingExpression">Xəritələmə ifadəsi</param>
    /// <param name="mapCreatedUser">CreatedUserName sahəsi üçün xəritələmə (true/false)</param>
    /// <param name="mapModifiedUser">ModifiedUserName sahəsi üçün xəritələmə (true/false)</param>
    /// <param name="createdUserNameField">CreatedUserName sahəsinin adı</param>
    /// <param name="modifiedUserNameField">ModifiedUserName sahəsinin adı</param>
    /// <returns>Xəritələmə ifadəsi</returns>
    public static IMappingExpression<TSource, TDestination> MapAuditableFields<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> mappingExpression,
        bool mapCreatedUser = true,
        bool mapModifiedUser = true,
        string createdUserNameField = "CreatedUserName",
        string modifiedUserNameField = "ModifiedUserName")
        where TSource : class
        where TDestination : class
    {
        if (mapCreatedUser)
        {
            mappingExpression.ForMember(
                createdUserNameField,
                opt => opt.MapFrom((src, dest, destMember, context) => GetUserName(src, "CreatedUser")));
        }

        if (mapModifiedUser)
        {
            mappingExpression.ForMember(
                modifiedUserNameField,
                opt => opt.MapFrom((src, dest, destMember, context) => GetUserName(src, "ModifiedUser")));
        }

        return mappingExpression;
    }

    private static string GetUserName<TSource>(TSource source, string userPropertyName)
    {
        var userValue = source?
            .GetType()
            .GetProperty(userPropertyName)?
            .GetValue(source);

        if (userValue == null) 
            return string.Empty;

        var userNameProperty = userValue
            .GetType()
            .GetProperty("UserName");

        return userNameProperty?
            .GetValue(userValue)?
            .ToString() ?? string.Empty;
    }
}
