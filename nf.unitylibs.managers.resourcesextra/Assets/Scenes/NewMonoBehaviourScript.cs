using NF.UnityLibs.Managers.ResourcesExtra;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var x = ResourcesExtraSettingsAsset.RuntimeInst.IsExist("ResourcesExtraSettingsAsset");
        Debug.Log(x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
