# Calcver.NET

[![CodeFactor](https://www.codefactor.io/repository/github/sanisoclem/calcver.net/badge)](https://www.codefactor.io/repository/github/sanisoclem/calcver.net)
[![CodeCov](https://img.shields.io/codecov/c/github/sanisoclem/calcver.net.svg?logo=codecov&logoColor=%23fff&style=flat)](https://codecov.io/gh/sanisoclem/Calcver.Net)
[![AppVeyorTests](https://img.shields.io/appveyor/tests/sanisoclem/calcver-net.svg?style=flat&logo=appveyor)](https://ci.appveyor.com/project/sanisoclem/calcver-net/build/tests)
[![Dependencies](https://img.shields.io/librariesio/github/sanisoclem/calcver.net.svg)](https://libraries.io/github/sanisoclem/Calcver.Net)
[![Nuget Downloads](https://img.shields.io/nuget/dt/Calcver.svg)](https://www.nuget.org/packages/calcver/)

Automatic nuget library versioning using [semver2.0](https://semver.org/) and [conventional commits](https://www.conventionalcommits.org/)

## Build status

| Build server | Platform     | SDK Version | Build status                                                                                                                                                  |
|--------------|--------------|-------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AppVeyor     | Windows      |           ? |[![Build status](https://ci.appveyor.com/api/projects/status/njuge4kx0tits5eo/branch/master?svg=true)](https://ci.appveyor.com/project/sanisoclem/calcver-net/branch/master) |
| Travis       | Linux / OS X |     2.1.503 |[![Build Status](https://travis-ci.org/sanisoclem/Calcver.Net.svg?branch=master)](https://travis-ci.org/sanisoclem/Calcver.Net) |

## Nuget Packages

| Name                                  | Version                                                                                                                 | Framework        |
|---------------------------------------|-------------------------------------------------------------------------------------------------------------------------|------------------|
| `dotnet-calcver`                      | [![Nuget](https://img.shields.io/nuget/v/dotnet-calcver.svg)](https://www.nuget.org/packages/dotnet-calcver/)           | `netcoreapp2.1`  |
| `Calcver`                             | [![Nuget](https://img.shields.io/nuget/v/Calcver.svg)](https://www.nuget.org/packages/Calcver/)                         | `netstandard2.0`  |
| `Calcver.Git`                         | [![Nuget](https://img.shields.io/nuget/v/Calcver.Git.svg)](https://www.nuget.org/packages/Calcver.Git/)                 | `netstandard2.0` |
| `Calcver.Git.MsBuild`                 | - | - |

## Installation

### Prerequisites

1. [Net Core 2.1+ SDK](https://www.microsoft.com/net/download/core)

### Install the CLI

```shell
$ dotnet tool install -g dotnet-calcver

You can invoke the tool using the following command: dotnet-calcver
Tool 'dotnet-calcver' (version 'x.x.x') was successfully installed.
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