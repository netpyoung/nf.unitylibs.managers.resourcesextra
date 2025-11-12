# Introduction


- [repo](https://github.com/netpyoung/nf.unitylibs.managers.resourcesextra/)

## Installation

### using [UPM](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

using #{version} for versioning.

``` json
"nf.unitylibs.managers.resourcesextra": "https://github.com/netpyoung/nf.unitylibs.managers.resourcesextra.git?path=LocalPackage#0.0.1"
```

## Description

- You can check if a resource exists without using Resources.Load, like this: `ResourcesExtraSettingsAsset.RuntimeInst.IsExist("hello");`
- When calling `Resources.Load("blabla");`, a warning will appear if the resource blabla cannot be found.

## Project Settings

![../images/ProjectSettings.png](../images/ProjectSettings.png)

- IsUpdateAsset : It produce `<UnityProject>/Assets/__NF/Resources/ResourcesExtraSettingsAsset.asset` to check resources exists and update Resources/ list.
- IsUpdateList : It produce `<UnityProject>/__NF/ResourcesExtra.txt` to diagnostic analyze.

## .editorconfig

- add `<UnityProject>/Assets/.editorconfig` like that

[!code-ini[Default](~/../nf.unitylibs.managers.resourcesextra/Assets/.editorconfig)]


![../images/NF6001.png](../images/NF6001.png)