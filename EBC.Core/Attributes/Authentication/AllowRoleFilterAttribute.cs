namespace EBC.Core.Attributes.Authentication;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class AllowRoleFilterAttribute : Attribute
{
    // Bu atribut, rol yoxlamasını aradan qaldırmaq üçün istifadə edilir.
}
