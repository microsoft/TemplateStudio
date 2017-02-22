using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Injection.Code
{
    public class CodeInjector : ContentInjector<CodeInjectorConfig>
    {
        public CodeInjector(string filePath) : base(filePath)
        {
        }

        public CodeInjector(CodeInjectorConfig config) : base(config)
        {
        }

        public override string Inject(string sourceContent)
        {
            //TODO: VERIFY PATH NOT NULL
            var tree = CSharpSyntaxTree.ParseText(sourceContent);
            var root = tree.GetRoot() as CompilationUnitSyntax;

            var usings = GetUsings(Config.usings).ToList();
            root = AddUsings(root, usings);

            //TODO: VERIFY CONFIG
            var codePath = CodePath.Parse(Config.path);


            //TODO: VERIFY CODE
            var method = codePath.FindMethod(root);

            if (method != null)
            {
                var statements = GetStatements(method, Config.content);
                root = AddStatements(root, method, statements);
            }

            return root
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
            if (usings == null)
            {
                yield break;
            }

            foreach (var u in usings)
            {
                yield return SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(u))
                                                                .WithTrailingTrivia(
                                                                    SyntaxFactory.EndOfLine(Environment.NewLine));
            }
        }

        private CompilationUnitSyntax AddStatements(CompilationUnitSyntax root, BaseMethodDeclarationSyntax method, IEnumerable<StatementSyntax> statements)
        {
            var newStatements = InsertStatement(method, statements);

            var newBody = method.Body.Update(method.Body.OpenBraceToken, newStatements, method.Body.CloseBraceToken);

            var newMethod = method.ReplaceNode(method.Body, newBody);

            var newRoot = root.ReplaceNode(method, newMethod);
            return newRoot;
        }

        private SyntaxList<StatementSyntax> InsertStatement(BaseMethodDeclarationSyntax method, IEnumerable<StatementSyntax> statements)
        {
            if (!string.IsNullOrEmpty(Config.before))
            {
                var tree = CSharpSyntaxTree.ParseText(GetMethodSnippet(Config.before));
                var root = tree.GetRoot();

                var dummyMethod = root
                                    .DescendantNodes()
                                    .OfType<MethodDeclarationSyntax>()
                                    .FirstOrDefault();

                var beforeStatement = dummyMethod.Body.Statements.FirstOrDefault();
                if (beforeStatement == null)
                {
                    throw new ArgumentException($"Statement {Config.before} not found in source!!");
                }

                var targetStatement = method.Body.Statements.FirstOrDefault(s => s.ToString().Equals(beforeStatement.ToString(), StringComparison.OrdinalIgnoreCase));

                if (targetStatement == null)
                {
                    throw new ArgumentException($"Statement {Config.before} not found in destination!!");
                }

                var index = method.Body.Statements.IndexOf(targetStatement);

                return method.Body.Statements.InsertRange(index, statements);
            }
            else
            {
                return method.Body.Statements.AddRange(statements);
            }
        }

        private static IEnumerable<StatementSyntax> GetStatements(BaseMethodDeclarationSyntax method, string text)
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

        private static string GetMethodSnippet(string statement)
        {
            return $"void a(){{{statement}}}";
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

            public BaseMethodDeclarationSyntax FindMethod(SyntaxNode root)
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

                BaseMethodDeclarationSyntax method = @class.DescendantNodes()
                                                                .OfType<MethodDeclarationSyntax>()
                                                                .FirstOrDefault(m => m.Identifier.ValueText == MethodName);

                if (method == null)
                {
                    method = @class.DescendantNodes()
                                        .OfType<ConstructorDeclarationSyntax>()
                                        .FirstOrDefault(m => m.Identifier.ValueText == MethodName);
                }


                return method;
            }
        }
    }
}
