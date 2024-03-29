# NLog.PerformanceCounter
NLog extensions to [Display Windows Performance Counters](https://github.com/NLog/NLog/wiki/PerformanceCounter-Layout-Renderer) and [Update Windows Performance Counters](https://github.com/NLog/NLog/wiki/PerfCounter-target)

[![Version](https://badge.fury.io/nu/NLog.PerformanceCounter.svg)](https://www.nuget.org/packages/NLog.PerformanceCounter)
[![AppVeyor](https://img.shields.io/appveyor/ci/nlog/NLog-PerformanceCounter/master.svg)](https://ci.appveyor.com/project/nlog/NLog-PerformanceCounter/branch/master)


### How to install

1) Install the package

    `Install-Package NLog.PerformanceCounter` or in your csproj:

    ```xml
    <PackageReference Include="NLog.PerformanceCounter" Version="5.*" />
    ```

2) Add to your nlog.config:

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

### Example of displaying PerformanceCounter 
Example of `NLog.config`-file that displays Windows Performance Counter value:

```xml
<nlog>
    <extensions>
        <add assembly="NLog.PerformanceCounter"/>
    </extensions>
    <targets>
        <target name="console" xsi:type="console" layout="${message}|Memory=${performancecounter:category=Process:counter=Working Set}"  />
    </targets>
    <rules>
        <logger minLevel="Info" writeTo="console" />
    </rules>
</nlog>
```

### Example of updating PerformanceCounter 
Example of `NLog.config`-file that updates Windows Performance Counter value:

```xml
<nlog>
    <extensions>
        <add assembly="NLog.PerformanceCounter"/>
    </extensions>
    <targets>
        <target name="perf" xsi:type="PerfCounter" counterName="123" categoryName="xyz" />
    </targets>
    <rules>
        <logger minLevel="Info" writeTo="perf" />
    </rules>
</nlog>
```