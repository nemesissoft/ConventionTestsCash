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
    public class OneClassPerFileTests
    {
        [SetUp]
        public void Setup() { }

        private static IEnumerable<TestCaseData> _oneClassPerFileSources = new[]
        {
            (@"class MyClass { } class MyClassExtensions { }", true),
            (@"class MyClass { } class OtherExtensions { }", false),
            ("class MyClassTop { class Inner { } }", true),
            ("class MyClass1 { } class MyClass2 { }", false),
        }.Select((t, i) => new TestCaseData(t.Item1, t.Item2).SetName($"Names{i:00}"));

        [TestCaseSource(nameof(_oneClassPerFileSources))]
        public void CheckContent(string source, bool isValid)
        {
            var compilation = CreateCompilation(source);

            var issues = CompilationUtils.GetCompilationIssues(compilation);
            Assert.That(issues, Is.Empty);


            var root = compilation.SyntaxTrees.First().GetCompilationUnitRoot();
            var types = root.ChildNodes().OfType<TypeDeclarationSyntax>().Select(t => t.Identifier.ValueText).ToList();


            foreach (var extensionName in types.FindAll(t => t.EndsWith("Extensions")))
            {
                var typeName = extensionName[..^10];

                if (types.Contains(typeName))
                    types.Remove(extensionName);
            }

            if (isValid)
                Assert.That(types, Has.Count.LessThanOrEqualTo(1));
            else
                Assert.That(types, Has.Count.GreaterThan(1));
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