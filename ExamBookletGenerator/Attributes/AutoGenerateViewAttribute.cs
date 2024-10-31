using EBC.Core.Models.Enums;

namespace ExamBookletGenerator.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class AutoGenerateActionViewAttribute : Attribute
{
    public MethodType ViewType { get; private set; }
    public Type DTOType { get; set; }

    public AutoGenerateActionViewAttribute()
    {
        ViewType = MethodType.Empty;
    }

    public AutoGenerateActionViewAttribute(MethodType viewType, Type _DTOType)
    {
        ViewType = viewType;
        DTOType = _DTOType;
    }
}
