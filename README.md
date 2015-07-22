# Nerdle.AutoConfig

[![Build Status](https://travis-ci.org/edpollitt/Nerdle.AutoConfig.svg?branch=master)](https://travis-ci.org/edpollitt/Nerdle.AutoConfig)
[![Nuget](https://img.shields.io/nuget/v/Nerdle.AutoConfig.svg)](https://www.nuget.org/packages/Nerdle.AutoConfig/)
[![Nuget](https://img.shields.io/nuget/dt/Nerdle.AutoConfig.svg)](https://www.nuget.org/packages/Nerdle.AutoConfig/)

##Quickstart

Create an interface for your configuration settings (no concrete class required!)

```csharp
public interface IMyServiceConfiguration
{
    string Endpoint { get; }
    int Port { get; }
    bool EnableCache { get; }
}
```

Add a custom configuration section in app.config / web.config (the section and element names should match your configuration object in camelCase)
```xml
<configuration>

  <configSections>
    <section name="myServiceConfiguration" type="Nerdle.AutoConfig.Section, Nerdle.AutoConfig" />
  </configSections>

  <myServiceConfiguration>
    <endpoint>http://localhost</endpoint>
    <port>42</port>
    <enableCache>true</enableCache>
  </myServiceConfiguration>

</configuration>
```

Ask AutoConfig to map your object

```csharp
var config = AutoConfig.Map<IMyServiceConguration>();
```

Done! 

:+1:

