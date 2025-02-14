<p align="center">
    <img src="https://github.com/user-attachments/assets/76e8dfc8-cfce-4c97-b280-6b4a681a478d" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/AutoConst/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/AutoConst/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/AutoConst)
![Nuget](https://img.shields.io/nuget/dt/AutoConst)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/AutoConst/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/AutoConst)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--9.0-green)

# AutoConst

This is a small tool to automatically convert files with constant properties over to other language equivalents.

Say you have this following `example.cs` file:
```csharp
namespace Something
{
	public static class ImportantStatics
	{
		public const string SomeStatic1 = "abc";
		public const string SomeStatic2 = 1111;
	}
}

```

`autoconst -t example.cs -p TypeScriptProducer`

It will output a generated TS file with the static names as follows:
```ts
// This document is auto generated!
export const ImportantStatics = {
	SomeStatic1: "abc",
	SomeStatic2: 1111,
}
```

The currently available producers are:
* `TypeScriptProducer`, to make TS code

You can also use the flag `-m` to "merge" several const files that has the same class name into a single output file.
This is useful if you need to have some sort of common reference table on the output side.

This package is available as a tool on the [NuGet Package Manager](https://www.nuget.org/packages/AutoConst/), so you can install it by writing `dotnet tool install AutoConst` in a terminal.