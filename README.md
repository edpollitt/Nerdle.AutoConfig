# Nerdle.AutoConfig

[![Build Status](https://travis-ci.org/edpollitt/Nerdle.AutoConfig.svg?branch=master)](https://travis-ci.org/edpollitt/Nerdle.AutoConfig)
[![Nuget](https://img.shields.io/nuget/v/Nerdle.AutoConfig.svg)](https://www.nuget.org/packages/Nerdle.AutoConfig/)
[![Nuget](https://img.shields.io/nuget/dt/Nerdle.AutoConfig.svg)](https://www.nuget.org/packages/Nerdle.AutoConfig/)

##Quickstart

Install via NuGet
```csharp
Install-Package Nerdle.AutoConfig
```

Define an interface for your configuration settings (no concrete class is required)

```csharp
public interface IMyServiceConfiguration
{
    string Endpoint { get; }
    int Port { get; }
    bool UseSSL { get; }
}
```

Add a configuration section in app.config / web.config
```xml
<configuration>
  <configSections>
    <section name="myService" type="Nerdle.AutoConfig.Section, Nerdle.AutoConfig" />
  </configSections>
  <myService>
    <endpoint>http://localhost</endpoint>
    <port>42</port>
    <useSSL>true</useSSL>
  </myService>
</configuration>
```

Call AutoConfig

```csharp
var config = AutoConfig.Map<IMyServiceConfiguration>();
```

You're done!

:+1:

####Check [the wiki](https://github.com/edpollitt/Nerdle.AutoConfig/wiki) for fun times
