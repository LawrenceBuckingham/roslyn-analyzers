// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using Analyzer.Utilities.Extensions;
using Microsoft.CodeAnalysis;

namespace Analyzer.Utilities.FlowAnalysis.Analysis.TaintedDataAnalysis
{
    internal static class XamlSinks
    {
        /// <summary>
        /// <see cref="SinkInfo"/>s for tainted data XAML injection sinks.
        /// </summary>
        public static ImmutableHashSet<SinkInfo> SinkInfos { get; }

        static XamlSinks()
        {
            var builder = PooledHashSet<SinkInfo>.GetInstance();

            builder.AddSinkInfo(
                WellKnownTypeNames.SystemWindowsMarkupXamlReader,
                SinkKind.Xaml,
                isInterface: false,
                isAnyStringParameterInConstructorASink: false,
                sinkProperties: null,
                sinkMethodParameters: new[] {
                    ( "Load", new[] { "stream", "reader", "xaml" }),
                    ( "LoadAsync", new[] { "stream", "reader" }),
                    ( "LoadWithInitialTemplateValidation", new[] { "xaml" }),
                });

            SinkInfos = builder.ToImmutableAndFree();
        }
    }
}
