﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System;
using Analyzer.Utilities;
using Analyzer.Utilities.Extensions;
using Microsoft.CodeAnalysis.Operations;

namespace Microsoft.CodeAnalysis.FlowAnalysis.DataFlow
{
    /// <summary>
    /// Conversion inference result.
    /// </summary>
    internal struct ConversionInference : IEquatable<ConversionInference>
    {
        public static ConversionInference Create(IConversionOperation operation)
            => Create(
                targetType: operation.Type,
                sourceType: operation.Operand.Type,
                isTryCast: operation.IsTryCast);

        public static ConversionInference Create(IIsPatternOperation operation)
            => Create(
                targetType: operation.Pattern.GetPatternType(),
                sourceType: operation.Value.Type,
                isTryCast: true);

        public static ConversionInference Create(
            ITypeSymbol? targetType,
            ITypeSymbol? sourceType,
            bool isTryCast)
        {
            return new ConversionInference
            {
                IsTryCast = isTryCast,
                AlwaysSucceed = !isTryCast, // For direct cast, we assume the cast will always succeed as the initial default value.
                AlwaysFail = false,
                IsBoxing = targetType != null &&
                       !targetType.IsValueType &&
                       sourceType?.IsValueType == true,
                IsUnboxing = targetType != null &&
                       targetType.IsValueType &&
                       sourceType != null &&
                       !sourceType.IsValueType
            };
        }

        public bool IsTryCast { get; set; }
        public bool AlwaysSucceed { get; set; }
        public bool AlwaysFail { get; set; }
        public bool IsBoxing { get; set; }
        public bool IsUnboxing { get; set; }

        public override bool Equals(object obj)
            => obj is ConversionInference other && Equals(other);

        public bool Equals(ConversionInference other)
        {
            return IsTryCast == other.IsTryCast &&
                AlwaysSucceed == other.AlwaysSucceed &&
                AlwaysFail == other.AlwaysFail &&
                IsBoxing == other.IsBoxing &&
                IsUnboxing == other.IsUnboxing;
        }

        public override int GetHashCode()
            => RoslynHashCode.Combine(IsTryCast, AlwaysSucceed, AlwaysFail, IsBoxing, IsUnboxing);

        public static bool operator ==(ConversionInference left, ConversionInference right)
            => left.Equals(right);

        public static bool operator !=(ConversionInference left, ConversionInference right)
            => !(left == right);
    }
}
