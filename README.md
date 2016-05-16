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
  
  <myService endpoint="http://localhost" port="42" useSSL="true" />

</configuration>
```

Call AutoConfig

```csharp
var config = AutoConfig.Map<IMyServiceConfiguration>();
```

You're done!

:+1:

**AutoConfig is a fully customisable and extensible library that can map simple types, strings, enums, nullables, enumerables, collections, lists, arrays, dictionaries, and arbitrarily complex nested types by convention out of the box - no boilerplate required.**

**Check [the wiki](https://github.com/edpollitt/Nerdle.AutoConfig/wiki) for fun times!**
