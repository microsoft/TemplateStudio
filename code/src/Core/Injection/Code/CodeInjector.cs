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

            root = InjectUsings(root, Config.usings);
            root = InjectProperties(root, Config.properties);
            root = InjectElements(root, Config.elements);

            return root
                    .NormalizeWhitespace()
                    .ToFullString();
        }

        private static CompilationUnitSyntax InjectUsings(CompilationUnitSyntax root, string[] usings)
        {
            var usingsSyntax = GetUsings(usings).ToList();
            return AddUsings(root, usingsSyntax);
        }

        private static CompilationUnitSyntax AddUsings(CompilationUnitSyntax root, IEnumerable<UsingDirectiveSyntax> usings)
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
                                                                            .WithTrailingTrivia(SyntaxFactory.EndOfLine(Environment.NewLine));
            }
        }

        private static CompilationUnitSyntax InjectProperties(CompilationUnitSyntax root, Dictionary<string, string> properties)
        {
            if (properties == null || properties.Count == 0)
            {
                return root;
            }

            var propertiesDeclaration = GetProperties(properties).ToList();
            root = AddProperties(root, propertiesDeclaration);

            return root;
        }

        private static CompilationUnitSyntax AddProperties(CompilationUnitSyntax root, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var @class = root.DescendantNodes()
                                    .OfType<ClassDeclarationSyntax>()
                                    .FirstOrDefault();

            //TODO: THROW EXCEPTION IF CLASS NOT FOUND

            var newClass = @class.AddMembers(properties.ToArray());

            root = root.ReplaceNode(@class, newClass);
            return root;
        }

        private static IEnumerable<PropertyDeclarationSyntax> GetProperties(Dictionary<string, string> properties)
        {
            foreach (var prop in properties)
            {
                yield return SyntaxFactory.PropertyDeclaration(
                                                SyntaxFactory.IdentifierName(prop.Value),
                                                SyntaxFactory.Identifier(prop.Key))
                                            .WithModifiers(
                                                SyntaxFactory.TokenList(
                                                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                                            .WithAccessorList(
                                                SyntaxFactory.AccessorList(
                                                    SyntaxFactory.SingletonList<AccessorDeclarationSyntax>(
                                                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                                        .WithSemicolonToken(
                                                            SyntaxFactory.Token(SyntaxKind.SemicolonToken)))));
            }
        }

        private CompilationUnitSyntax InjectElements(CompilationUnitSyntax root, CodeInjectorElement[] elements)
        {
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    //TODO: VERIFY CONFIG
                    var codePath = CodePath.Parse(element.path);

                    //TODO: VERIFY CODE
                    var body = codePath.FindBody(root);

                    if (body != null)
                    {
                        var statements = GetStatements(body, element.content);
                        var newStatements = InsertStatement(body, statements, element.before);

                        root = AddStatements(root, body, newStatements);
                    }
                }
            }

            return root;
        }

        private CompilationUnitSyntax AddStatements(CompilationUnitSyntax root, BlockSyntax body, SyntaxList<StatementSyntax> statements)
        {
            var method = body.Parent;

            var newBody = body.Update(body.OpenBraceToken, statements, body.CloseBraceToken);

            var newMethod = method.ReplaceNode(body, newBody);

            var newRoot = root.ReplaceNode(method, newMethod);
            return newRoot;
        }

        private static SyntaxList<StatementSyntax> InsertStatement(BlockSyntax body, IEnumerable<StatementSyntax> statements, string beforeOf)
        {
            if (!string.IsNullOrEmpty(beforeOf))
            {
                var tree = CSharpSyntaxTree.ParseText(GetMethodSnippet(beforeOf));
                var root = tree.GetRoot();

                var dummyMethod = root
                                    .DescendantNodes()
                                    .OfType<MethodDeclarationSyntax>()
                                    .FirstOrDefault();

                var beforeStatement = dummyMethod.Body.Statements.FirstOrDefault();
                if (beforeStatement == null)
                {
                    throw new ArgumentException($"Statement {beforeOf} not found in source!!");
                }

                var targetStatement = body.Statements.FirstOrDefault(s => s.ToString().Equals(beforeStatement.ToString(), StringComparison.OrdinalIgnoreCase));

                if (targetStatement == null)
                {
                    throw new ArgumentException($"Statement {beforeOf} not found in destination!!");
                }

                var index = body.Statements.IndexOf(targetStatement);

                return body.Statements.InsertRange(index, statements);
            }
            else
            {
                return body.Statements.AddRange(statements);
            }
        }

        private static IEnumerable<StatementSyntax> GetStatements(BlockSyntax body, string text)
        {
            var startTrivia = body.GetLeadingTrivia().Span.Length;

            var lines = Regex.Split(text, Environment.NewLine);

            foreach (var line in lines)
            {
                yield return SyntaxFactory.ParseStatement(line)
                                                .WithLeadingTrivia(SyntaxFactory.Whitespace(new string(' ', startTrivia + 4)))
                                                .WithTrailingTrivia(body.GetTrailingTrivia());
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
            public PropertyInfo PropertyName { get; set; }


            //TODO: DO THIS WITH REGEX
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
                    if (methodChunks[1].EndsWith("_get") || methodChunks[1].EndsWith("_set"))
                    {
                        var propChunks = Regex.Split(methodChunks[1], "_");

                        instance.PropertyName = new PropertyInfo
                        {
                            Name = propChunks[0],
                            Accesor = propChunks[1] == "get" ? AccesorType.Get : AccesorType.Set
                        };

                    }
                    else
                    {
                        instance.MethodName = methodChunks[1];
                    }
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

            public BlockSyntax FindBody(SyntaxNode root)
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

                if (PropertyName != null)
                {
                    //TODO: VERIFY PROP IS NOT NULL
                    var property = @class.DescendantNodes()
                                                .OfType<PropertyDeclarationSyntax>()
                                                .FirstOrDefault(m => m.Identifier.ValueText == PropertyName.Name);

                    var method = property.AccessorList.Accessors.FirstOrDefault(a => a.Kind() == SyntaxKind.GetAccessorDeclaration);

                    return method.Body;

                }
                else if (!string.IsNullOrEmpty(MethodName))
                {
                    BaseMethodDeclarationSyntax method = @class.DescendantNodes()
                                                                    .OfType<MethodDeclarationSyntax>()
                                                                    .FirstOrDefault(m => m.Identifier.ValueText == MethodName);

                    if (method == null)
                    {
                        method = @class.DescendantNodes()
                                            .OfType<ConstructorDeclarationSyntax>()
                                            .FirstOrDefault(m => m.Identifier.ValueText == MethodName);
                    }

                    return method.Body;
                }
                return null;
            }
        }

        public class PropertyInfo
        {
            public string Name { get; set; }
            public AccesorType Accesor { get; set; }
        }

        public enum AccesorType
        {
            Get,
            Set
        }
    }
}
