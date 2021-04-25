using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Nemesis.CodeAnalysis;

using NUnit.Framework;

namespace ConventionTests
{
    public class ReferencesTests
    {
        //[TestCaseSource(nameof(_oneClassPerFileSources))]
        public void CheckContent(string source, bool isValid)
        {
           //TODO
        }

        public static Compilation CreateCompilation(string source, OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
        {
            var (compilation, _, _) = CompilationUtils.CreateTestCompilation(source, new[]
            {
                typeof(BigInteger).GetTypeInfo().Assembly,
            }, outputKind);

            return compilation;
        }
    }
}