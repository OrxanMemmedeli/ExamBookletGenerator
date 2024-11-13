namespace EBC.Core.Attributes;

/// <summary>
/// Fluent Validation yoxlamasından istisna etmək üçün istifadə olunan xüsusi atribut.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IgnoreValidationAttribute : Attribute
{
}
