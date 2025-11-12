using System.Collections.Generic;
using UnityEditor;

namespace NF.UnityLibs.Managers.ResourcesExtra.Editors
{
    public sealed class ResourcesExtraSettingsProvider : SettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new ResourcesExtraSettingsProvider("Project/__NF/NF.UnityLibs.Managers.ResourcesExtra", SettingsScope.Project);
        }

        private Editor _editor = null;

        public ResourcesExtraSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnGUI(string searchContext)
        {
            if (_editor == null)
            {
                Editor editor = Editor.CreateEditor(ResourcesExtraSettingsAsset.Editor_GetOrCreate());
                _editor = editor;
            }

            _editor.OnInspectorGUI();
        }
    }
}
