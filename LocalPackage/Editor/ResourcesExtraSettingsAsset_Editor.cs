using UnityEditor;
using UnityEngine;

namespace NF.UnityLibs.Managers.ResourcesExtra.Editors
{
    [CustomEditor(typeof(ResourcesExtraSettingsAsset))]
    public sealed class ResourcesExtraSettingsAsset_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Force UpdateAsset"))
            {
                ResourcesExtraSettingsAsset.Editor_UpdateResourcesExtraSettingsAsset();
            }

            base.OnInspectorGUI();
        }
    }
}
