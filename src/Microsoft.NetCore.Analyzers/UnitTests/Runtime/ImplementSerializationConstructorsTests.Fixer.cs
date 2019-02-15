﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit;
using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    Microsoft.NetCore.Analyzers.Runtime.SerializationRulesDiagnosticAnalyzer,
    Microsoft.NetCore.Analyzers.Runtime.ImplementSerializationConstructorsFixer>;
using VerifyVB = Microsoft.CodeAnalysis.VisualBasic.Testing.XUnit.CodeFixVerifier<
    Microsoft.NetCore.Analyzers.Runtime.SerializationRulesDiagnosticAnalyzer,
    Microsoft.NetCore.Analyzers.Runtime.ImplementSerializationConstructorsFixer>;

namespace Microsoft.NetCore.Analyzers.Runtime.UnitTests
{
    public partial class ImplementSerializationConstructorsFixerTests
    {
        [Fact]
        public async Task CA2229NoConstructorFix()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Runtime.Serialization;
[Serializable]
public class {|CA2229:CA2229NoConstructor|} : ISerializable
{
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}", @"
using System;
using System.Runtime.Serialization;
[Serializable]
public class CA2229NoConstructor : ISerializable
{
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }

    protected CA2229NoConstructor(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
        throw new NotImplementedException();
    }
}");

            await VerifyVB.VerifyCodeFixAsync(@"
Imports System
Imports System.Runtime.Serialization
<Serializable>
Public Class {|CA2229:CA2229NoConstructor|}
    Implements ISerializable

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext) Implements ISerializable.GetObjectData
        throw new NotImplementedException()
    End Sub
End Class", @"
Imports System
Imports System.Runtime.Serialization
<Serializable>
Public Class CA2229NoConstructor
    Implements ISerializable

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext) Implements ISerializable.GetObjectData
        throw new NotImplementedException()
    End Sub

    Protected Sub New(serializationInfo As SerializationInfo, streamingContext As StreamingContext)
        Throw New NotImplementedException()
    End Sub
End Class");
        }

        [Fact]
        public async Task CA2229HasConstructorWrongAccessibilityFix()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Runtime.Serialization;
[Serializable]
public class CA2229HasConstructorWrongAccessibility : ISerializable
{
    public {|CA2229:CA2229HasConstructorWrongAccessibility|}(SerializationInfo info, StreamingContext context) { }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}", @"
using System;
using System.Runtime.Serialization;
[Serializable]
public class CA2229HasConstructorWrongAccessibility : ISerializable
{
    protected CA2229HasConstructorWrongAccessibility(SerializationInfo info, StreamingContext context) { }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}");

            await VerifyVB.VerifyCodeFixAsync(@"
Imports System
Imports System.Runtime.Serialization
<Serializable>
Public Class CA2229HasConstructorWrongAccessibility
    Implements ISerializable

    Public Sub {|CA2229:New|}(info As SerializationInfo, context As StreamingContext)
    End Sub

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext) Implements ISerializable.GetObjectData
        throw new NotImplementedException()
    End Sub
End Class", @"
Imports System
Imports System.Runtime.Serialization
<Serializable>
Public Class CA2229HasConstructorWrongAccessibility
    Implements ISerializable

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
    End Sub

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext) Implements ISerializable.GetObjectData
        throw new NotImplementedException()
    End Sub
End Class");
        }

        [Fact]
        public async Task CA2229HasConstructorWrongAccessibility2Fix()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using System;
using System.Runtime.Serialization;
[Serializable]
public sealed class CA2229HasConstructorWrongAccessibility2 : ISerializable
{
    protected internal {|CA2229:CA2229HasConstructorWrongAccessibility2|}(SerializationInfo info, StreamingContext context) { }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}", @"
using System;
using System.Runtime.Serialization;
[Serializable]
public sealed class CA2229HasConstructorWrongAccessibility2 : ISerializable
{
    private CA2229HasConstructorWrongAccessibility2(SerializationInfo info, StreamingContext context) { }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}");

            await VerifyVB.VerifyCodeFixAsync(@"
Imports System
Imports System.Runtime.Serialization
<Serializable>
Public NotInheritable Class CA2229HasConstructorWrongAccessibility2
    Implements ISerializable

    Protected Friend Sub {|CA2229:New|}(info As SerializationInfo, context As StreamingContext)
    End Sub

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext) Implements ISerializable.GetObjectData
        throw new NotImplementedException()
    End Sub
End Class", @"
Imports System
Imports System.Runtime.Serialization
<Serializable>
Public NotInheritable Class CA2229HasConstructorWrongAccessibility2
    Implements ISerializable

    Private Sub New(info As SerializationInfo, context As StreamingContext)
    End Sub

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext) Implements ISerializable.GetObjectData
        throw new NotImplementedException()
    End Sub
End Class");
        }
    }
}
