using System.Linq;
using UnityEditor;

namespace NF.UnityLibs.Managers.ResourcesExtra.Editors
{
    public sealed class ResourcesFolderAssetPostprocessor : AssetPostprocessor
    {
        const string RESOURCES_DIR = "/Resources/";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool isResourcesFolderUpdated = false;

            if (importedAssets.Any(x => x.Contains(RESOURCES_DIR)))
            {
                isResourcesFolderUpdated = true;
            }

            if (deletedAssets.Any(x => x.Contains(RESOURCES_DIR)))
            {
                isResourcesFolderUpdated = true;
            }

            if (movedAssets.Any(x => x.Contains(RESOURCES_DIR)))
            {
                isResourcesFolderUpdated = true;
            }

            if (movedFromAssetPaths.Any(x => x.Contains(RESOURCES_DIR)))
            {
                isResourcesFolderUpdated = true;
            }

            if (!isResourcesFolderUpdated)
            {
                return;
            }

            if (importedAssets.Length == 1)
            {
                if (importedAssets[0] == ResourcesExtraSettingsAsset.ASSET_PATH)
                {
                    return;
                }
            }

            ResourcesExtraSettingsAsset.Editor_UpdateResourcesExtraSettingsAsset();
        }
    }
}
