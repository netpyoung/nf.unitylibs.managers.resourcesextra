using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using NF.UnityLibs.Managers.ResourcesExtra.RoslynCodeAnalysis.DiagnosticDescriptors;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace NF.UnityLibs.Managers.ResourcesExtra.RoslynCodeAnalysis.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ResourceLoadAnalyzer : DiagnosticAnalyzer
    {
        private readonly static object _lockObj1 = new object();
        private readonly static object _lockObj2 = new object();
        private static DateTime _lastTime = DateTime.MinValue;
        private static string _ResourcePlusIndexPath = null;
        private static HashSet<string> _cachedLoadPaths = new HashSet<string>();
        private static EditorConfigResourcePlus _editorConfigResourcePlus = new EditorConfigResourcePlus();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(DiagnosticDescriptorCollection.NF6001);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(compilationContext =>
            {
                SyntaxTree firstTree = compilationContext.Compilation.SyntaxTrees.FirstOrDefault();
                if (firstTree == null)
                {
                    return;
                }

                AnalyzerConfigOptions options = compilationContext.Options.AnalyzerConfigOptionsProvider.GetOptions(firstTree);
                if (!_editorConfigResourcePlus.Init(options))
                {
                    return;
                }

                compilationContext.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
            });
        }

        private void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
        {
            InvocationExpressionSyntax invocation = (InvocationExpressionSyntax)context.Node;
            IMethodSymbol symbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            if (symbol == null)
            {
                return;
            }

            if (!symbol.Name.StartsWith(_editorConfigResourcePlus.OptStartWith))
            {
                return;
            }

            string fullTypeName = symbol.ContainingType.ToDisplayString();
            if (!_editorConfigResourcePlus.Set.Contains(fullTypeName))
            {
                return;
            }

            ArgumentSyntax arg = invocation.ArgumentList.Arguments.FirstOrDefault();
            if (arg == null)
            {
                return;
            }

            Optional<object> constValue = context.SemanticModel.GetConstantValue(arg.Expression);
            if (!constValue.HasValue)
            {
                return;
            }

            string filePath = context.Node.SyntaxTree.FilePath;
            if (!UpdateResourcesIndexPath(filePath))
            {
                return;
            }

            string loadPath = constValue.Value.ToString();
            lock (_lockObj2)
            {
                UpdateCachedLoadPaths();
                if (_cachedLoadPaths.Contains(loadPath))
                {
                    return;
                }
            }

            Diagnostic diag = Diagnostic.Create(DiagnosticDescriptorCollection.NF6001, arg.GetLocation(), loadPath);
            context.ReportDiagnostic(diag);
        }

        private static bool UpdateResourcesIndexPath(string filePath)
        {
            if (_ResourcePlusIndexPath is not null)
            {
                return true;
            }

            lock (_lockObj1)
            {
                if (_ResourcePlusIndexPath is not null)
                {
                    return true;
                }

                if (!IsOnUnityProject(filePath, out string unityRoot))
                {
                    return false;
                }

                _ResourcePlusIndexPath = $"{unityRoot.TrimEnd('/')}/__NF/ResourcesExtra.txt";
            }

            return true;
        }

        private static void UpdateCachedLoadPaths()
        {
            if (!File.Exists(_ResourcePlusIndexPath))
            {
                return;
            }

            DateTime currentTime = File.GetLastAccessTimeUtc(_ResourcePlusIndexPath);
            if (_lastTime > currentTime)
            {
                return;
            }

            _lastTime = currentTime;

            _cachedLoadPaths.Clear();

            string[] lines = File.ReadAllLines(_ResourcePlusIndexPath);
            foreach (string s in lines)
            {
                _cachedLoadPaths.Add(s);
            }
        }

        private static bool IsOnUnityProject(string fullpath, out string outUnityRoot)
        {
            if (!fullpath.Contains("Assets"))
            {
                outUnityRoot = string.Empty;
                return false;
            }

            DirectoryInfo dir = new DirectoryInfo(fullpath);

            while (dir != null)
            {
                if (!Directory.Exists(Path.Combine(dir.FullName, "Assets")))
                {
                    dir = dir.Parent;
                    continue;
                }

                string manifestPath = Path.Combine(dir.FullName, "Packages", "manifest.json");
                if (File.Exists(manifestPath))
                {
                    outUnityRoot = dir.FullName.Replace('\\', '/');
                    return true;
                }

                string versionPath = Path.Combine(dir.FullName, "ProjectSettings", "ProjectVersion.txt");
                if (File.Exists(versionPath))
                {
                    outUnityRoot = dir.FullName.Replace('\\', '/');
                    return true;
                }

                dir = dir.Parent;
            }


            outUnityRoot = string.Empty;
            return false;
        }
    }
}