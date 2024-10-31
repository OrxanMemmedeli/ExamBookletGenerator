using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EBC.Core.Helpers.HtmlViewGenerators;


class SyntaxReceiver : ISyntaxReceiver
{
    public List<MethodDeclarationSyntax> CandidateMethods { get; } = new List<MethodDeclarationSyntax>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is MethodDeclarationSyntax methodDeclaration &&
            methodDeclaration.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == "AutoGenerateActionView")))
            CandidateMethods.Add(methodDeclaration);
    }
}