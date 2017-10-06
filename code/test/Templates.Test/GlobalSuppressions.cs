// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "All current violations are due to Tuple shorthand and so valid.")]

[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:PrefixLocalCallsWithThis", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]

[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:DoNotUseRegions", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1127:Generic type constraints must be on their own line", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1128:Constructor initializers must be on their own line", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]

[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1203:ConstantsMustAppearBeforeFields", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1209:UsingAliasDirectivesMustBePlacedAfterOtherUsingDirectives", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1210:UsingDirectivesMustBeOrderedAlphabeticallyByNamespace", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]

[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]

[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1516:ElementsMustBeSeparatedByBlankLine", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]

[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileNameMustMatchTypeName", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1652:EnableXmlDocumentationOutput", Justification = "Initially suppressing everything before evaluating what we want and what's appropriate.")]

[assembly: SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Scope = "member", Target = "Microsoft.Templates.Core.Locations.TemplatesSynchronization.#SyncStatusChanged", Justification = "Using an Action<object, SyncStatusEventArgs> does not allow the required notation")]
