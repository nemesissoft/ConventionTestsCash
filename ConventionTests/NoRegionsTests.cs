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
    public class NoRegionsTests
    {
        class MyClass
        {
            #region super region

            #endregion
        }

        [Test]
        public void CheckRegion()
        {
            var compilation = CreateCompilation(@"
class MyClass
{
    #region super region

    #endregion
}");

            var issues = CompilationUtils.GetCompilationIssues(compilation);
            Assert.That(issues, Is.Empty);


            var root = compilation.SyntaxTrees.First().GetCompilationUnitRoot();

            var regionNodes = root.DescendantNodes(descendIntoChildren: null, descendIntoTrivia: true)
                .OfType<RegionDirectiveTriviaSyntax>()
                .Select(r => string.Join(", ", r.EndOfDirectiveToken.GetAllTrivia()))
                ;

            Assert.That(regionNodes, Is.Empty);
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