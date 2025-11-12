# nf.unitylibs.managers.resourcesextra

[![GitHub](https://img.shields.io/badge/GitHub-%23121011.svg?logo=github&logoColor=white)](https://github.com/netpyoung/nf.unitylibs.managers.resourcesextra)
[![Document](https://img.shields.io/badge/document-docfx-blue)](https://netpyoung.github.io/nf.unitylibs.managers.resourcesextra/)
[![License](https://img.shields.io/badge/license-MIT-C06524)](https://github.com/netpyoung/nf.unitylibs.managers.resourcesextra/blob/main/LICENSE.md)

## Installation

### using [UPM](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

using #{version} for versioning.

``` json
"nf.unitylibs.managers.resourcesextra": "https://github.com/netpyoung/nf.unitylibs.managers.resourcesextra.git?path=LocalPackage#0.0.1"
```

## Description

- You can check if a resource exists without using Resources.Load, like this: `ResourcesExtraSettingsAsset.RuntimeInst.IsExist("hello");`
- When calling `Resources.Load("blabla");`, a warning will appear if the resource blabla cannot be found.

![./docfx/images/NF6001.png](./docfx/images/NF6001.png)

## Documentation

- [Documentation](https://netpyoung.github.io/nf.unitylibs.managers.resourcesextra/)
