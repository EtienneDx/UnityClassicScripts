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

#### Condition System
This asset adds the support for conditions for the different other assets from this pack. Said conditions are any world relative condition (or even player related, in case of ginleplayer games).

To use it, just enable the 'Conditions' define using the Define Manager, and all the editor related features will get enabled directly. For example, the spawn manager will display a condition field. Conditions are an assembly of conditions defined in the code, and of composite conditions, themselves composed of both. You can also choose wether the condition should be true for any valid subcondition, or if all are valid. This system allows you to create complex conditions, without difficulty.

From a code perspective, you can very easily use the condition for your own scripts : just create a `Condition` field, and verify wether it is valid : 

```csharp
public Condition myCondition;

private void Update()
{
    if(myCondition.IsValid)
    {
        Debug.Log("The condition is valid");
    }
}
```

To create your own condition, you can simply use the following attribute on a static boolean function defined as follow :

```csharp
[Condition("Is Night")]
public static bool IsInstanceNight()
{
    return instance != null && instance.IsNight;
}
```

The function must be static, return a boolean and take no parameter.

#### Spawn Manager
The spawn manager allows you to create a spawn area, in which multiple entities can spawn onto the navmesh, with a spawn probability for each entity, a maximum amount of entities, etc...

#### [WIP] Growable
Still a work in progress
