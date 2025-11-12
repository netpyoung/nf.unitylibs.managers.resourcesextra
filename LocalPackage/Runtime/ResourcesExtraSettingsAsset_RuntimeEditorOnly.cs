#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NF.UnityLibs.Managers.ResourcesExtra
{
    public sealed partial class ResourcesExtraSettingsAsset : ScriptableObject
    {
        public static ResourcesExtraSettingsAsset Editor_GetOrCreate()
        {
            ResourcesExtraSettingsAsset settings = UnityEditor.AssetDatabase.LoadAssetAtPath<ResourcesExtraSettingsAsset>(ASSET_PATH);
            if (settings != null)
            {
                return settings;
            }

            if (!Directory.Exists("Assets/__NF"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "__NF");
            }

            if (!Directory.Exists("Assets/__NF/Resources"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets/__NF", "Resources");
            }
            settings = CreateInstance<ResourcesExtraSettingsAsset>();
            UnityEditor.AssetDatabase.CreateAsset(settings, ASSET_PATH);
            UnityEditor.EditorUtility.SetDirty(settings);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            Editor_UpdateResourcesExtraSettingsAsset(settings);

            return settings;
        }

        public static void Editor_UpdateResourcesExtraSettingsAsset()
        {
            Editor_UpdateResourcesExtraSettingsAsset(Editor_GetOrCreate());
        }

        private static void Editor_UpdateResourcesExtraSettingsAsset(ResourcesExtraSettingsAsset asset)
        {
            if (!asset.IsUpdateAsset && !asset.IsUpdateList)
            {
                return;
            }

            ScanAllResources(out List<Item> underAssets, out List<Item> underPackageCache);

            if (asset.IsUpdateAsset)
            {
                asset._underAssets = underAssets;
                asset._underPackageCache = underPackageCache;
                UnityEditor.EditorUtility.SetDirty(asset);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }

            if (asset.IsUpdateList)
            {
                const string xPath = "__NF/ResourcesExtra.txt";

                Directory.CreateDirectory(Path.GetDirectoryName(xPath));

                using (StreamWriter writer = new StreamWriter(xPath))
                {
                    foreach (string x in underAssets.Select(x => x.LoadPath))
                    {
                        writer.WriteLine(x);
                    }

                    foreach (string x in underPackageCache.Select(x => x.LoadPath))
                    {
                        writer.WriteLine(x);
                    }
                }
            }

            Debug.Log($"updated: underAssets={underAssets.Count}, underPackageCache={underPackageCache.Count}");
        }

        private static void ScanAllResources(out List<Item> outUnderAssets, out List<Item> outUnderPackageCache)
        {
            List<Item> underAssets = new List<Item>(100);
            foreach (string dir in Directory.GetDirectories("Assets", "Resources", SearchOption.AllDirectories).OrderBy(d => d, StringComparer.OrdinalIgnoreCase))
            {
                foreach (string file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories).OrderBy(d => d, StringComparer.OrdinalIgnoreCase))
                {
                    if (file.EndsWith(".meta"))
                    {
                        continue;
                    }

                    string relative = file.Replace("\\", "/");
                    if (relative.Contains("~/"))
                    {
                        continue;
                    }

                    if (relative.Contains("/Editor/"))
                    {
                        continue;
                    }

                    int idx = relative.IndexOf("/Resources/");
                    if (idx == -1)
                    {
                        continue;
                    }

                    string loadPath = relative.Substring(idx + "/Resources/".Length);
                    loadPath = Path.ChangeExtension(loadPath, null);
                    underAssets.Add(new Item { LoadPath = loadPath, RealPath = relative });
                }
            }
            outUnderAssets = underAssets;

            List<Item> underPackageCache = new List<Item>(100);
            foreach (string dir in Directory.GetDirectories("Library/PackageCache", "Resources", SearchOption.AllDirectories).OrderBy(d => d, StringComparer.OrdinalIgnoreCase))
            {
                foreach (string file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories).OrderBy(d => d, StringComparer.OrdinalIgnoreCase))
                {
                    if (file.EndsWith(".meta"))
                    {
                        continue;
                    }

                    string relative = file.Replace("\\", "/");
                    if (relative.Contains("~/"))
                    {
                        continue;
                    }

                    if (relative.Contains("/Editor/"))
                    {
                        continue;
                    }

                    int idx = relative.IndexOf("/Resources/");
                    if (idx == -1)
                    {
                        continue;
                    }

                    string loadPath = relative.Substring(idx + "/Resources/".Length);
                    loadPath = Path.ChangeExtension(loadPath, null);
                    underPackageCache.Add(new Item { LoadPath = loadPath, RealPath = relative });
                }
            }
            outUnderPackageCache = underPackageCache;
        }
    }
}

#endif // UNITY_EDITOR