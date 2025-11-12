using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NF.UnityLibs.Managers.ResourcesExtra
{
    [Serializable]
    public sealed partial class ResourcesExtraSettingsAsset : ScriptableObject, ISerializationCallbackReceiver
    {
        public static ResourcesExtraSettingsAsset RuntimeInst
        {
            get
            {
                if (_inst != null)
                {
                    return _inst;
                }

                _inst = Resources.Load<ResourcesExtraSettingsAsset>(ASSET_NAME);
                return _inst;
            }
        }

        private static ResourcesExtraSettingsAsset _inst = null;

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeInitializeOnLoadMethod()
        {
            _inst = null;
        }

        [Serializable]
        public class Item
        {
            [SerializeField] public string LoadPath;
            [SerializeField] public string RealPath;
        }

        public const string ASSET_NAME = "ResourcesExtraSettingsAsset";
        public readonly static string ASSET_PATH = $"Assets/__NF/Resources/{ASSET_NAME}.asset";

        public bool IsUpdateAsset => _isUpdateAsset;
        public bool IsUpdateList => _isUpdateList;
        public int TotalCount => _totalCount;

        [SerializeField]
        private bool _isUpdateAsset = false;

        [SerializeField]
        private bool _isUpdateList = false;

        [SerializeField]
        private List<Item> _underAssets = new List<Item>();

        [SerializeField]
        private List<Item> _underPackageCache = new List<Item>();

        private HashSet<string> _underAssetSet = null;
        private HashSet<string> _underPackageCacheSet = null;
        private int _totalCount = 0;


        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _underAssetSet = _underAssets.Select(x => x.LoadPath).ToHashSet();
            _underPackageCacheSet = _underPackageCache.Select(x => x.LoadPath).ToHashSet();
            _totalCount = _underAssets.Count + _underPackageCache.Count;
        }

        public bool IsExist(string resourcePath)
        {
            if (IsExistOnUnderAsset(resourcePath))
            {
                return true;
            }

            if (IsExistOnUnderPakcage(resourcePath))
            {
                return true;
            }

            return false;
        }

        public bool IsExistOnUnderAsset(string resourcePath)
        {
            return _underAssetSet.Contains(resourcePath);
        }

        public bool IsExistOnUnderPakcage(string resourcePath)
        {
            return _underPackageCacheSet.Contains(resourcePath);
        }
    }
}
