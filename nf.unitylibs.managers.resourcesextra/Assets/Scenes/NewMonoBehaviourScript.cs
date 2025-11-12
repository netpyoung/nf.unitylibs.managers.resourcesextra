using NF.UnityLibs.Managers.ResourcesExtra;
using UnityEngine;
using UnityEngine.Assertions;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Assert.IsTrue(ResourcesExtraSettingsAsset.RuntimeInst.IsExist("ResourcesExtraSettingsAsset"));
        Assert.IsFalse(ResourcesExtraSettingsAsset.RuntimeInst.IsExist("blabla"));

        Resources.Load("ResourcesExtraSettingsAsset");
        Resources.Load("blabla");
    }
}
