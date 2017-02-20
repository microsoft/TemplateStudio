using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Code
{
    public class CodePostAction : PostAction<CodePostActionConfig>
    {
        public override string Execute(CodePostActionConfig config, string sourceContent)
        {
            //TODO: VERIFY CONFIG
            var codePath = CodePath.Parse(config.path);

            if (codePath == null)
            {
                return null;
            }

            //TODO: VERIFY PATH NOT NULL
            var tree = CSharpSyntaxTree.ParseText(sourceContent);
            var root = tree.GetRoot() as CompilationUnitSyntax;

            //TODO: VERIFY CODE
            var method = codePath.FindMethod(root);

            if (method == null)
            {
                return null;
            }

            var statements = GetStatements(method, config.content);
            var newRoot = AddStatements(root, method, statements);

            var usings = GetUsings(config.usings);
            newRoot = AddUsings(newRoot, usings);

            return newRoot
                    .NormalizeWhitespace()
                    .ToFullString();
        }

        private CompilationUnitSyntax AddUsings(CompilationUnitSyntax root, IEnumerable<UsingDirectiveSyntax> usings)
        {
            if (usings == null)
            {
                return root;
            }

            return root.AddUsings(usings.ToArray());
        }

        private static IEnumerable<UsingDirectiveSyntax> GetUsings(string[] usings)
        {
            foreach (var u in usings)
            {
                yield return SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(u))
                                                                .WithTrailingTrivia(
                                                                    SyntaxFactory.EndOfLine(Environment.NewLine));
            }
        }

        private static CompilationUnitSyntax AddStatements(CompilationUnitSyntax root, MethodDeclarationSyntax method, IEnumerable<StatementSyntax> statements)
        {
            var newStatements = method.Body.Statements.AddRange(statements);
            var newBody = method.Body.Update(method.Body.OpenBraceToken, newStatements, method.Body.CloseBraceToken);

            var newMethod = method.ReplaceNode(method.Body, newBody);

            var newRoot = root.ReplaceNode(method, newMethod);
            return newRoot;
        }


        private static IEnumerable<StatementSyntax> GetStatements(MethodDeclarationSyntax method, string text)
        {
            var startTrivia = method.GetLeadingTrivia().Span.Length;

            var lines = Regex.Split(text, Environment.NewLine);

            foreach (var line in lines)
            {
                yield return SyntaxFactory.ParseStatement(line)
                                                .WithLeadingTrivia(SyntaxFactory.Whitespace(new string(' ', startTrivia + 4)))
                                                .WithTrailingTrivia(method.GetTrailingTrivia());
            }
        }

        private class CodePath
        {
            public string Namespace { get; set; }
            public string ClassName { get; set; }
            public string MethodName { get; set; }

            public static CodePath Parse(string path)
            {
                var instance = new CodePath();

                if (string.IsNullOrWhiteSpace(path))
                {
                    return instance;
                }

                var methodChunks = Regex.Split(path, "::");
                if (methodChunks.Length > 1)
                {
                    instance.MethodName = methodChunks[1];
                }

                var namespaceChunks = Regex.Split(methodChunks[0], @"\.");

                if (namespaceChunks.Length > 1)
                {
                    instance.ClassName = namespaceChunks.Last();
                    instance.Namespace = string.Join(".", namespaceChunks.Take(namespaceChunks.Length - 1));
                }
                else
                {
                    instance.ClassName = namespaceChunks[0];
                }

                return instance;
            }

            public MethodDeclarationSyntax FindMethod(SyntaxNode root)
            {
                var @namespace = root.DescendantNodes()
                                    .OfType<NamespaceDeclarationSyntax>()
                                    .FirstOrDefault(n => n.Name.ToString().Equals(Namespace));

                if (@namespace == null)
                {
                    return null;
                }

                var @class = @namespace.DescendantNodes()
                                            .OfType<ClassDeclarationSyntax>()
                                            .FirstOrDefault(c => c.Identifier.ValueText == ClassName);


                if (@class == null)
                {
                    return null;
                }

                var method = @namespace.DescendantNodes()
                                            .OfType<MethodDeclarationSyntax>()
                                            .FirstOrDefault(m => m.Identifier.ValueText == MethodName);

                return method;
            }
        }
    }
}
