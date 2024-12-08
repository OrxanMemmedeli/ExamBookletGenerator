using EBC.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace EBC.Core.Helpers.HtmlViewGenerators;


class SyntaxReceiver : ISyntaxReceiver
{
    public List<MethodDeclarationSyntax> CandidateMethods { get; } = new List<MethodDeclarationSyntax>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        Debugger.Launch(); // Debugging başlatmaq üçün
        if (syntaxNode is MethodDeclarationSyntax methodDeclaration &&
            methodDeclaration.AttributeLists.Any(al => al.Attributes.Any(a => a.GetType() == typeof(AutoGenerateActionViewAttribute))))
            CandidateMethods.Add(methodDeclaration);
    }
}