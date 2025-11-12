using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace NF.UnityLibs.Managers.ResourcesExtra.RoslynCodeAnalysis.DiagnosticAnalyzers
{
    public sealed class EditorConfigResourcePlus
    {
        public string OptStartWith { get; private set; }
        public ImmutableHashSet<string> Set { get; private set; }

        public bool Init(AnalyzerConfigOptions options)
        {
            if (!options.TryGetValue("resourceplus_startswith", out string optStartWith))
            {
                return false;
            }

            if (!options.TryGetValue("resourceplus_namespaces", out string optNamespaces))
            {
                return false;
            }

            ImmutableHashSet<string> set = optNamespaces.Split(',').ToImmutableHashSet();
            OptStartWith = optStartWith;
            Set = set;
            return true;
        }
    }
}