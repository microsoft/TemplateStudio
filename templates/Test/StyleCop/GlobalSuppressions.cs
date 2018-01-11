// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "We follow the C# Core Coding Style which avoids using `this` unless absolutely necessary.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:Using directives must be placed correctly", Justification = "We follow the C# Core Coding Style which puts using statements outside the namespace.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements must appear in the correct order", Justification = "We do not support the recommended element order as doing so would add unnecessary complexity to template composition.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements must be ordered by access", Justification = "We do not support the recommended element order as doing so would add unnecessary complexity to template composition.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements must appear before instance elements", Justification = "We do not support the recommended element order as doing so would add unnecessary complexity to template composition.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:Field names must not begin with underscore", Justification = "We follow the C# Core Coding Style which uses underscores as prefixes rather than using `this`.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "For simplicity we're allowing generic and non-generic versions of Activationhandler in one file.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1633:File must have header", Justification = "By default we do not include a header in the geenrated files.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "We leave this as a choice for the developer to enable. As the generated code doesn't include XML comments, enablinng this would just create more issues with the generated code.")]
