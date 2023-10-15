# NLog PerformanceCounter

NLog extensions to [Display Windows Performance Counters](https://github.com/NLog/NLog/wiki/PerformanceCounter-Layout-Renderer) and [Update Windows Performance Counters](https://github.com/NLog/NLog/wiki/PerfCounter-target)

## Register Extension

NLog will only recognize type-alias `performancecounter` when loading from `NLog.config`-file, if having added extension to `NLog.config`-file:

```xml
<extensions>
    <add assembly="NLog.PerformanceCounter"/>
</extensions>
```

Alternative register from code using [fluent configuration API](https://github.com/NLog/NLog/wiki/Fluent-Configuration-API):

```csharp
LogManager.Setup().SetupExtensions(ext => {
   ext.RegisterTarget<NLog.Targets.PerformanceCounterTarget>();
   ext.RegisterLayoutRenderer<NLog.LayoutRenderers.PerformanceCounterLayoutRenderer>();
});
```
