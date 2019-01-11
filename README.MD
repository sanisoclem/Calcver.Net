# Calcver.NET

[![CodeFactor](https://www.codefactor.io/repository/github/potatoengineering/calcver.net/badge)](https://www.codefactor.io/repository/github/potatoengineering/calcver.net)
[![CodeCov](https://img.shields.io/codecov/c/github/Potatoengineering/calcver.net.svg?logo=codecov&logoColor=%23fff&style=flat)](https://codecov.io/gh/PotatoEngineering/Calcver.Net)
[![AppVeyorTests](https://img.shields.io/appveyor/tests/sanisoclem/calcver-net.svg?compact_message=true&style=flat&logo=appveyor)](https://ci.appveyor.com/project/sanisoclem/calcver-net/build/tests)
[![Dependencies](https://img.shields.io/librariesio/github/PotatoEngineering/calcver.net.svg?logo=nuget&logoColor=%23fff)](https://libraries.io/github/PotatoEngineering/Calcver.Net)
![Nuget Downloads](https://img.shields.io/nuget/dt/Calcver.Git.svg?logo=nuget&logoColor=%23fff)
![PreRelease](https://img.shields.io/nuget/vpre/Calcver.Git.svg?logo=nuget&logoColor=%23fff)

Automatic nuget library versioning using [semver2.0](https://semver.org/) and [conventional commits](https://www.conventionalcommits.org/)

## Build status

| Build server | Platform     | Build status                                                                                                                                                  |
|--------------|--------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AppVeyor     | Windows      | [![AppVeyor](https://img.shields.io/appveyor/ci/sanisoclem/calcver-net.svg?logo=appveyor&style=flat)](https://ci.appveyor.com/project/sanisoclem/calcver-net) |
| Travis       | Linux / OS X | - |

## Nuget Packages

| Name                                  | Version                                                                                                                 | Framework        |
|---------------------------------------|-------------------------------------------------------------------------------------------------------------------------|------------------|
| `dotnet-calcver`                      | [![Nuget](https://img.shields.io/nuget/v/dotnet-calcver.svg)](https://www.nuget.org/packages/dotnet.calcver/)           | `netcoreapp2.1`  |
| `Calcver`                             | [![Nuget](https://img.shields.io/nuget/v/Calcver.svg)](https://www.nuget.org/packages/Calcver/)                         | `netstandard2.0`  |
| `Calcver.Git`                         | [![Nuget](https://img.shields.io/nuget/v/Calcver.Git.svg)](https://www.nuget.org/packages/Calcver.Git/)                 | `netstandard2.0` |
| `Calcver.Git.MsBuild`                 | [![Nuget](https://img.shields.io/nuget/v/Calcver.Git.MsBuild.svg)](https://www.nuget.org/packages/Calcver.Git.MsBuild/) | - |

## Installation

### Prerequisites

1. [Net Core 2.1+ SDK](https://www.microsoft.com/net/download/core)

### Install the CLI

```shell
$ dotnet tool install -g dotnet-calcver

You can invoke the tool using the following command: dotnet-calcver
Tool 'dotnet-calcver' (version '0.2.0') was successfully installed.
```

### Install MsBuild Tasks

```shell
$ dotnet add package Calcver.Git.MsBuild

Writing C:\...\Temp\tmp5FDB.tmp
info : Adding PackageReference for package 'Calcver.Git.MsBuild' into project '...'.
...
log  : Restore completed in 2.33 sec for ...csproj.
```


## Usage

WIP