using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;


namespace EBC.Core.Helpers.HtmlViewGenerators;

/// <summary>
/// HTML View fayllarını avtomatik yaratmaq üçün `ISourceGenerator` implementasiyası.
/// Bu generator `AutoGenerateActionViewAttribute` atributu ilə işarələnmiş metodlara uyğun `.cshtml` fayllarını generasiya edir.
/// </summary>
/// <remarks>
/// İstifadə qaydaları:
/// 1. **Generatoru Layihəyə Daxil Edin**: 
///    `CsHtmlViewGenerator` sinifinin olduğu kitabxana layihəyə referens olaraq əlavə edilməlidir.
///    Əgər generator ayrıca bir kitabxana layihəsindədirsə, əsas layihəyə referens olaraq daxil edin.
///
/// 2. **Generatoru Aktivləşdirin**:
///    Generator əsas layihəyə referens olaraq əlavə edildikdən sonra Visual Studio və ya `dotnet build` əməliyyatı zamanı avtomatik işə düşəcək.
///    `Build` və ya `Rebuild` əməliyyatlarını həyata keçirdikdə `.cshtml` faylları generasiya ediləcək.
///
/// 3. **Generasiya Olunmuş Faylları Yoxlayın**:
///    `ISourceGenerator` tərəfindən generasiya olunmuş fayllar Visual Studio-da `Analyzers` qovluğunun altında görünəcək.
///    Buradan generasiya olunmuş `.cshtml` fayllarını və onların məzmununu yoxlaya bilərsiniz.
///
/// 4. **`AutoGenerateActionViewAttribute` ilə İstifadəçi Metodlarını Tənzimləyin**:
///    Generatorunuz `AutoGenerateActionViewAttribute` atributuna əsaslanaraq yalnız bu atribut ilə işarələnmiş metodlara uyğun `.cshtml` fayllarını yaradır.
///    İstifadəçi metodlarına atributu əlavə edin, məsələn:
///    <code>
///    [AutoGenerateActionView(MethodType.List, typeof(YourDTOType))]
///    public IActionResult Index()
///    {
///        // Metod məzmunu
///    }
///    </code>
///    Atributun `MethodType` və `DTOType` dəyərlərinə uyğun olaraq `.cshtml` faylları avtomatik generasiya olunacaq.
///
/// 5. **Dəyişikliklərə Real Zamanlı Nəzarət**:
///    Hər dəfə `.csproj` faylı və ya kod faylı dəyişdirildikdə, generator yenidən işləyəcək və faylları yeniləyəcək.
/// </remarks>
//[Generator]
public class CsHtmlViewGenerator : ISourceGenerator
{
    /// <summary>
    /// Generatorun ilkin konfigurasiya metodudur.
    /// </summary>
    /// <param name="context">Generatorun ilkin vəziyyətini təyin edir.</param>
    public void Initialize(GeneratorInitializationContext context)
    {
        return;
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    /// <summary>
    /// Generatorun əsas işə düşmə metodudur, lazımi atributları yoxlayır və uyğun `.cshtml` fayllarını generasiya edir.
    /// </summary>
    /// <param name="context">Generatorun icra vəziyyətini təyin edir.</param>
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is SyntaxReceiver receiver)
        {
            var compilation = context.Compilation;

            foreach (var methodDeclaration in receiver.CandidateMethods)
            {
                var semanticModel = compilation.GetSemanticModel(methodDeclaration.SyntaxTree);
                var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration) as IMethodSymbol;

                if (methodSymbol != null)
                {
                    // Atributu namespace ilə tam şəkildə yoxlayırıq
                    var attribute = methodSymbol.GetAttributes()
                        .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == "EBC.Core.Attributes.AutoGenerateActionViewAttribute");

                    // DTO tipini və MethodType dəyərini tapırıq
                    var dtoType = attribute?.ConstructorArguments.FirstOrDefault(arg => arg.Kind == TypedConstantKind.Type).Value as INamedTypeSymbol;
                    var methodType = attribute?.ConstructorArguments.FirstOrDefault(arg => arg.Type?.Name == "MethodType").Value;

                    if (dtoType == null || methodType == null) continue;

                    // AreaName-i tapmaq üçün ContainingType-in atributlarını yoxlayırıq
                    var areaAttribute = methodSymbol.ContainingType.GetAttributes()
                        .FirstOrDefault(a => a.AttributeClass?.Name == "AreaAttribute");

                    // `AreaAttribute` varsa, `areaName`-i təyin edirik, əks halda boş saxlayırıq
                    var areaName = areaAttribute?.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;

                    // Generasiya edilmiş view kontentini yaradırıq
                    string viewContent = GenerateViewContent(methodSymbol, dtoType, methodType.ToString(), areaName);
                    var fileName = $"{methodSymbol.ContainingType.Name}_{methodSymbol.Name}.cshtml";

                    // Fiziki fayl yaratmaq üçün əlavə məntiq
                    //WriteViewToDisk(viewContent, fileName, areaName, methodSymbol.ContainingType.Name);


                    // Analyzers altında görünməsi üçün əlavə edin
                    context.AddSource(fileName, SourceText.From(viewContent, Encoding.UTF8));
                }
            }
        }
    }

    /// <summary>
    /// Verilən metod, DTO tipi, methodType və areaName-ə əsasən `.cshtml` fayl kontentini yaradır.
    /// </summary>
    /// <param name="methodSymbol">İşlənəcək metodun simvolu.</param>
    /// <param name="dtoType">DTO tipi.</param>
    /// <param name="methodType">Metod tipi (List, Create və ya Edit).</param>
    /// <param name="areaName">Area adı.</param>
    /// <returns>Generasiya olunmuş `.cshtml` fayl məzmunu.</returns>
    private string GenerateViewContent(IMethodSymbol methodSymbol, INamedTypeSymbol dtoType, string methodType, string areaName)
    {
        // MethodType dəyərinə əsasən uyğun metodu çağırırıq
        return methodType switch
        {
            "List" => GenerateIndexView(dtoType, areaName, methodSymbol.ContainingType.Name),
            "Create" => GenerateFormView(dtoType, isEditMode: false),
            "Edit" => GenerateFormView(dtoType, isEditMode: true),
            _ => string.Empty
        };
    }


    #region List
    /// <summary>
    /// `List` görünüşünü generasiya edir, daxilində axtarış, cədvəl və səhifələmə komponentləri var.
    /// </summary>
    /// <param name="dtoType">DTO tipi.</param>
    /// <param name="areaName">Area adı.</param>
    /// <param name="controllerName">Controller adı.</param>
    /// <returns>`List` görünüşünə uyğun `.cshtml` fayl məzmunu.</returns>
    private string GenerateIndexView(INamedTypeSymbol dtoType, string areaName, string controllerName)
    {
        var properties = dtoType.GetMembers().OfType<IPropertySymbol>().ToList();

        // Search Input sahələrini generasiya edirik
        var mainPropertyNames = properties.Where(p => p.Type.Name != "FilterOperation");
        var filterPropertyNames = properties.Where(p => p.Type.Name == "FilterOperation");
        var searchInputs = GenerateSearchInputsContent(mainPropertyNames, filterPropertyNames);


        // Cədvəl başlıqlarını (table headers) yaradırıq
        var tableHeaders = string.Join("", properties.Select(prop => $"<th>{prop.Name}</th>"));

        // Cədvəl sətirlərini `GenerateListViewBodyContent` ilə generasiya edirik
        var tableBody = GenerateListViewBodyContent(areaName, controllerName, properties);

        // Pagination üçün uyğun filed-ləri yaradırıq
        var paginationFields = GeneratFileds(mainPropertyNames, filterPropertyNames);

        // .cshtml kontentini qaytarırıq
        return $@"
                ﻿@using X.PagedList;
                @using X.PagedList.Web.Common;
                @using X.PagedList.Mvc.Core;

                @model IPagedList<{dtoType.ToDisplayString()}>

                @{{
                    ViewData[""Title""] = ""{dtoType.Name}"";
                    Layout = ""~/Views/Shared/_AdminLayout.cshtml"";
                }}

                <h1>@ViewData[""Title""]</h1>

                <!-- Axtarış paneli -->
                <div class=""row"">
                    <div class=""col-md-12"">
                        <div class=""white_card position-relative mb_20"">
                            <div class=""card-body"">
                                <h5 class=""mt-0"">Axtarış paneli</h5>
                                <div class=""search-area"">
                                    <form asp-action=""Index"" method=""get"" class=""form-inline"">
                                        <div class=""row"">
                                            {searchInputs}
                                        </div>
                                        <hr/>
                                        <button type=""submit"" class=""btn btn-secondary"">Axtar</button>
                                        <a id=""clearForm"" class=""btn btn-outline-dark"" onclick=""clearAllInputs()"">Təmizlə</a>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <p>
                    <a asp-action=""Create"" class=""btn btn-vimeo"" title=""Yeni məlumat"" style=""justify-content: center; align-content:center"">
                        <div class=""flex-md-column m-0 p-0"">
                            &nbsp;&nbsp; Yeni
                        </div>
                        <div class=""flex-md-column m-0 p-0"">
                            <lord-icon src=""https://cdn.lordicon.com/rfbqeber.json""
                                       trigger=""loop""
                                       delay=""2000""
                                       style=""width:70px;height:70px"">
                            </lord-icon>
                        </div>
                    </a>
                </p>
                <div class=""table-responsive"">
                    <table class=""table table-striped"">
                        <thead>
                            <tr>
                                {tableHeaders}
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {tableBody}
                        </tbody>
                    </table>
                    <h6 class=""card-subtitle mb_20"">
                        Məlumat sayı: <code>@Model.TotalItemCount</code>
                    </h6>
                </div>

                @Html.PagedListPager(Model, page => Url.Action(""Index"", new {{ {paginationFields} page = page }}),
                    new PagedListRenderOptions
                    {{
                        UlElementClasses = new string[] {{ ""pagination"" }},
                        LiElementClasses = new string[] {{ ""page-item"" }},
                        PageClasses = new string[] {{ ""page-link"" }},
                        MaximumPageNumbersToDisplay = 5,
                        LinkToFirstPageFormat = ""&lt;&lt; İlk"",
                        LinkToLastPageFormat = ""Son &gt;&gt;"",
                        LinkToPreviousPageFormat = ""&lt;&lt; Öncəki"",
                        LinkToNextPageFormat = ""Sonraki &gt;&gt;""
                    }}
                )

                @section AdminScripts{{
                    <script>
                        function clearAllInputs() {{
                            const textList = document.getElementsByClassName('text');
                            const checkboxList = document.getElementsByTagName('checkbox');
                            const dateList = document.getElementsByClassName('date');
                            const numberList = document.getElementsByClassName('number');
                            const selectList = document.getElementsByClassName('select');
                            // 👇️ clear input field
                            for (var i = 0; i < textList.length; ++i) {{
                                textList[i].value = '';
                            }}

                            for (var i = 0; i < numberList.length; ++i) {{
                                numberList[i].value = '';
                            }}

                            for (var i = 0; i < checkboxList.length; ++i) {{
                                ccheckboxList[i].checked = false;
                            }}

                            for (var i = 0; i < selectList.length; ++i) {{
                                selectList[i].options.selectedIndex = 0;
                            }}

                            for (var i = 0; i < dateList.length; ++i) {{
                                //erase the input value
                                // dateList[i].value = '0001-01-01';
                                dateList[i].value = '';

                                //prevent error on older browsers (aka IE8)
                                if (dateList[i].type === 'date') {{
                                    //update the input content (visually)
                                    dateList[i].type = 'text';
                                    dateList[i].type = 'date';
                                    dateList[i].value = '';
                                }}
                            }}
                        }}
                    </script>
                }}

                @section AdminStyles{{
                    <style>
                        .form-check {{
                            width:100px;
                            padding-left:inherit;
                        }}
                    </style>
                }}";
    }


    /// <summary>
    /// Axtarış sahələrinin HTML məzmununu generasiya edir.
    /// </summary>
    /// <param name="mainPropertyNames">Əsas xüsusiyyətlərin siyahısı.</param>
    /// <param name="filterPropertyNames">Filtr xüsusiyyətlərinin siyahısı.</param>
    /// <returns>Axtarış sahələri üçün HTML kontenti.</returns>
    private static string GenerateSearchInputsContent(IEnumerable<IPropertySymbol> mainPropertyNames, IEnumerable<IPropertySymbol> filterPropertyNames)
    {
        var searchInputs = new StringBuilder();

        // FilterOperation tipləri üçün loop yaradılır
        var loopModel = filterPropertyNames
            .Where(prop => prop.Type.Name == "FilterOperation")
            .Select(x => new { x.Name, x.Type });

        foreach (var filterProp in loopModel)
        {
            // MainPropertyNames içində uyğun sahəni tapırıq
            var mainProp = mainPropertyNames
                .SingleOrDefault(prop => prop.Name.Equals(filterProp.Name.Replace("FilterType", ""), StringComparison.OrdinalIgnoreCase));

            if (mainProp != null)
            {
                string inputText;

                if (mainProp.Type.Name == "Boolean" || mainProp.Type.Name == "Nullable<Boolean>")
                {
                    // Boolean və Nullable<Boolean> üçün Select elementi yaradılır
                    inputText = $@"
                    <select name=""{mainProp.Name}"" id=""{filterProp.Name}"" class=""form-select select"">
                        @if (ViewBag.Model?.{mainProp.Name} == null)
                        {{
                            <option value=""null"" selected>-- Seç --</option>
                            <option value=""true"">Aktivlər</option>
                            <option value=""false"">Passivlər</option>
                        }}
                        else if (ViewBag.Model?.{mainProp.Name} == true)
                        {{
                            <option value=""null"">-- Seç --</option>
                            <option value=""true"" selected>Aktivlər</option>
                            <option value=""false"">Passivlər</option>
                        }}
                        else if (ViewBag.Model?.{mainProp.Name} == false)
                        {{
                            <option value=""null"">-- Seç --</option>
                            <option value=""true"">Aktivlər</option>
                            <option value=""false"" selected>Passivlər</option>
                        }}
                    </select>";
                }
                else if (mainProp.Type.Name == "DateTime" || mainProp.Type.Name == "Nullable<DateTime>")
                {
                    // DateTime və Nullable<DateTime> üçün input type="date" elementi
                    inputText = $@"<input name=""{mainProp.Name}"" id=""{filterProp.Name}"" class=""form-control date"" type=""date"" value=""@((ViewBag.Model?.{mainProp.Name} != null) ? ((DateTime)ViewBag.Model?.{mainProp.Name}).ToString(""yyyy-MM-dd"") : """")""/>";
                }
                else
                {
                    // Digər tiplər üçün input type="text" və ya uyğun tip
                    inputText = $@"<input name=""{mainProp.Name}"" id=""{filterProp.Name}"" class=""form-control {GetTypeProperty(mainProp)}"" type=""{GetTypeProperty(mainProp)}"" value=""@ViewBag.Model?.{mainProp.Name}""/>";
                }

                // Bütün input sahələri üçün HTML kontentini əlavə edirik
                searchInputs.AppendLine($@"
                <div class=""col-3 p-2"">
                    <div class=""form-group"">
                        <label for=""{filterProp.Name}"" class=""control-label"">{mainProp.Name}</label>
                        {inputText}
                    </div>
                </div>");
            }
        }

        return searchInputs.ToString();
    }

    /// <summary>
    /// `List` görünüşü üçün cədvəl sətirlərini yaradır. Xüsusiyyətləri və `_ToolsButtonPartial` komponentini əlavə edir.
    /// </summary>
    /// <param name="areaName">Area adı.</param>
    /// <param name="controllerName">Controller adı.</param>
    /// <param name="properties">DTO modelinin xüsusiyyətləri.</param>
    /// <returns>Cədvəl sətirləri üçün HTML məzmunu.</returns>
    private static string GenerateListViewBodyContent(string areaName, string controllerName, List<IPropertySymbol> properties)
    {
        var tableBody = new StringBuilder();

        tableBody.AppendLine("<tr>");

        // Xüsusiyyətlərə əsasən hər sətir üçün `td` elementləri əlavə olunur
        foreach (var prop in properties)
        {
            tableBody.AppendLine($"<td>@item.{prop.Name}</td>");
        }

        // Hər sətirin sonunda `_ToolsButtonPartial` komponenti əlavə olunur
        tableBody.AppendLine($@"
                                <td>
                                    @await Html.PartialAsync(""~/Views/Shared/_ToolsButtonPartial.cshtml"", new ToolsButtonViewModel 
                                    {{ 
                                        Area = ""{areaName}"", 
                                        Controller = ""{controllerName.Replace("Controller", "")}"", 
                                        RouteId = @item.Id 
                                    }})
                                </td>
                            </tr>");

        return tableBody.ToString();
    }

    /// <summary>
    /// Səhifələmə və axtarış nəticələrini idarə edən `Filed`-ləri yaradır.
    /// </summary>
    /// <param name="mainPropertyNames">Əsas xüsusiyyətlərin siyahısı.</param>
    /// <param name="filterPropertyNames">Filtr xüsusiyyətlərinin siyahısı.</param>
    /// <returns>Səhifələmə üçün lazım olan `Filed`-lərin tərtibi.</returns>
    private static string GeneratFileds(IEnumerable<IPropertySymbol> mainPropertyNames, IEnumerable<IPropertySymbol> filterPropertyNames)
    {
        var resultString = new StringBuilder();

        var loopModel = filterPropertyNames
            .Where(prop => prop.Type.Name == "FilterOperation") // FilterOperation tipləri seçilir
            .Select(x => new { x.Name, x.Type });

        foreach (var filterProp in loopModel)
        {
            // FilterProperty adını əsas xüsusiyyətlərdən axtarırıq
            var mainProp = mainPropertyNames
                .SingleOrDefault(prop => prop.Name.Equals(filterProp.Name.Replace("FilterType", ""), StringComparison.OrdinalIgnoreCase));

            if (mainProp != null)
            {
                // Uyğun xüsusiyyət varsa, resultString-ə əlavə edirik
                resultString.Append($"{mainProp.Name} = ViewBag.Model?.{mainProp.Name}, ");
            }
        }

        return resultString.ToString().TrimEnd(' ', ',');
    }

    #endregion

    #region Edit Or Create
    /// <summary>
    /// Yeni və ya redaktə formu üçün `.cshtml` fayl məzmununu generasiya edir.
    /// </summary>
    /// <param name="dtoType">DTO tipi.</param>
    /// <param name="isEditMode">Redaktə rejimi (true/redaktə, false/yeni).</param>
    /// <returns>Form görünüşü üçün `.cshtml` fayl məzmunu.</returns>
    private string GenerateFormView(INamedTypeSymbol dtoType, bool isEditMode)
    {
        var properties = dtoType.GetMembers().OfType<IPropertySymbol>().ToList();
        var propertyInputs = GeneratePropertyInputsContent(properties);

        return $@"
                @model {dtoType}

                @{{
                    ViewData[""Title""] = ""{(isEditMode ? "Yenilə - " : "Yeni - ")} {dtoType.Name}"";
                    Layout = ""~/Views/Shared/_AdminLayout.cshtml"";
                }}

                <h1>@ViewData[""Title""]</h1>

                <form asp-action=""{(isEditMode ? "Edit" : "Create")}"" method=""post"">
                    <div asp-validation-summary=""All"" class=""text-danger""></div>
                    {(isEditMode ? "<input asp-for=\"Id\" type=\"hidden\" value=\"@Model.Id\" />" : string.Empty)}

                    {propertyInputs}

                    <div class=""row"">
                        {propertyInputs}
                        <hr/>
                        <div class=""form-group"">
                            <input type=""submit"" value=""{(isEditMode ? "Yenilə" : "Əlavə Et")}"" class=""btn btn-primary"" />
                            <a asp-action=""Index"" class=""btn btn-dark"">Geri</a>
                        </div>
                    </div>
                </form>";
    }

    /// <summary>
    /// Form üçün lazımi input sahələrini yaradır və uyğun `.cshtml` məzmununu qaytarır.
    /// </summary>
    /// <param name="properties">DTO modelinin xüsusiyyətləri.</param>
    /// <returns>Form üçün input sahələri üçün HTML kontenti.</returns>
    private string GeneratePropertyInputsContent(List<IPropertySymbol> properties)
    {
        var propertyInputs = new StringBuilder();
        foreach (var prop in properties)
        {
            propertyInputs.AppendLine($@"
                <div class=""form-group"">
                    <label asp-for=""{prop.Name}"" class=""control-label"">{prop.Name}</label>
                    <input type=""{GetTypeProperty(prop)}"" asp-for=""{prop.Name}"" class=""form-control"" />
                    <span asp-validation-for=""{prop.Name}"" class=""text-danger""></span>
                </div>");
        }
        return propertyInputs.ToString();
    }

    /// <summary>
    /// Verilən xüsusiyyətin tipinə uyğun input növünü müəyyən edir.
    /// </summary>
    /// <param name="propertySymbol">Xüsusiyyət simvolu (`IPropertySymbol`).</param>
    /// <returns>Input növü (`text`, `checkbox`, `date`, `number`).</returns>
    private static string GetTypeProperty(IPropertySymbol propertySymbol)
    {
        // IPropertySymbol istifadə edərək giriş növünü təyin edirik
        var inputType = propertySymbol.Type.Name switch
        {
            "String" or "Empty" or "Char" => "text",
            "Boolean" => "checkbox",
            "DateTime" => "date",
            "Decimal" or "Int16" or "Int32" or "Int64" or "Double" or "Single" or "SByte" or "Byte" or "UInt16" or "UInt32" or "UInt64" => "number",
            _ => "text"
        };
        return inputType;
    }
    #endregion


    //private void WriteViewToDisk(string viewContent, string fileName, string areaName, string controllerName)
    //{
    //    // Layihə kök qovluğunu əldə edirik
    //    string projectDirectory = Directory.GetCurrentDirectory();

    //    // `Views` qovluğu yolu
    //    string viewsDirectory = string.IsNullOrWhiteSpace(areaName)
    //        ? Path.Combine(projectDirectory, "Views", controllerName)
    //        : Path.Combine(projectDirectory, "Areas", areaName, "Views", controllerName);

    //    // Qovluğu yoxlayırıq, yoxdursa yaradılır
    //    if (!Directory.Exists(viewsDirectory))
    //    {
    //        Directory.CreateDirectory(viewsDirectory);
    //    }

    //    // Fayl tam yolunu yaradırıq
    //    string filePath = Path.Combine(viewsDirectory, fileName);

    //    // Faylı yazırıq
    //    File.WriteAllText(filePath, viewContent, Encoding.UTF8);

    //    Console.BackgroundColor = ConsoleColor.Green;
    //    Console.WriteLine($"File Path: {filePath}");
    //    Console.ResetColor();
    //}

}
