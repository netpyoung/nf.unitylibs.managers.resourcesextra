using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace NF.UnityLibs.Managers.ResourcesExtra.RoslynCodeAnalysis.DiagnosticAnalyzers
{
    public sealed class EditorConfigResourcePlus
    {
        public string OptStartWith { get; private set; }
        public ImmutableHashSet<string> FullTypeNameSet { get; private set; }

        public bool Init(AnalyzerConfigOptions options)
        {
            if (!options.TryGetValue("resourceplus_startswith", out string optStartWith))
            {
                return false;
            }

            if (!options.TryGetValue("resourceplus_fulltypenames", out string optFullTypeNames))
            {
                return false;
            }

            ImmutableHashSet<string> fullTypeNameSet = optFullTypeNames.Split(',').ToImmutableHashSet();
            OptStartWith = optStartWith;
            FullTypeNameSet = fullTypeNameSet;
            return true;
        }
    }
}