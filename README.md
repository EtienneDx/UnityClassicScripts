# UnityClassicScripts
Some Unity3D scripts that may be useful to someone someday :)

## What is it
I wanted to code some stuff for unity, so I figured I might as well share it. There will be a few assets added now and then.

## Current Assets

### Utils

Some utility assets are : 

#### Utils
The actual utility asset, with basic scripts such as counters, random ranged number pickers, etc

The utils also include a Define Manager window, which allows you to enable / disable all the defines for the different assets present
(Eg. If you don't import one asset, just disable it's define to disable all the compatibility features)

#### Package Exporter
This tool allows you to quickly export multiple assets as unitypackages, saving you the trouble of selecting one by one the files to export.

### The actual assets

#### Config File
This asset allows you to create some configuration files, saved in json, that your player may edit by going into the file himself, or just as a way to save game data. All the loading is automatic, all you need is a class implementing the IConfigFile interface, and that is serializable, such as : 

```csharp
[Serializable]// mark the class as serializable
[ConfigName("Test")]// optional - choose a name for this config
class TestConfig : IConfigFile
{
    public string someOption = "hello world";
}
```

You can then access it directly from anywhere : 

```csharp
public class ConfigLogger : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(ConfigFile.GetConfig<TestConfig>().someOption);
    }
}
```
