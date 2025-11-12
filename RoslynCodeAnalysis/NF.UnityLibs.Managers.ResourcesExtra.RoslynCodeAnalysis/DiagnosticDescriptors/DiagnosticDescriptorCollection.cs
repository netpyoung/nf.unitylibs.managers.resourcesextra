using Microsoft.CodeAnalysis;

namespace NF.UnityLibs.Managers.ResourcesExtra.RoslynCodeAnalysis.DiagnosticDescriptors
{
    public sealed class DiagnosticDescriptorCollection
    {
        public static readonly DiagnosticDescriptor NF6001 = new DiagnosticDescriptor(
            id: "NF6001",
            title: "Resource not found in Resources/",
            messageFormat: "'{0}' does not exist in Resources/",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Failed to find in Resources/. Please double check the path."
        );
    }
}