# Common Lib

This is the common library that is used in most of my blog post source code.  I have updated it to .Net 6.  Including changes to utilize some of the new .Net 6 features.

by [David Gallivan](http://twitter.com/CodingwithDavid)


## Why

I wanted to make it available in case others need to use it and to allow them access to it if they are having issue with any of my blog post source code projects  

## Getting Started

1. Clone this repository

   ```Command Line
   git clone https://github.com/CodingWithDavid/CodingwithDavidCommonLib
   cd CodingwithDavidCommonLib
   ```

1.	Open in Visual Studio or Visual Code
a.	With Visual Code you will need to install the C# extensions
2.	Press F5

## What's in the App

This is a .NET 10 class library. Public APIs are in the `CodingWithDavid.Common`
namespace.

1. Method Extenstions
2. Helper Functions
3. Common Utilities

## Encryption

File encryption uses AES-GCM with a key derived from a caller-provided password.
Always use the password overloads:

```csharp
FileFolderHelper.EncryptFile("input.txt", "input.bin", password);
FileFolderHelper.DecryptFile("input.bin", "output.txt", password);
```

The legacy two-argument overloads are obsolete and only work when
`CODINGWITHDAVID_COMMON_ENCRYPTION_PASSWORD` is configured. They are retained
to make migration possible, but new code should pass the password explicitly.

## API documentation

XML documentation is generated when the library is built. The package is targeted at `net10.0`.

## .NET API overlap

The library intentionally does not wrap APIs that .NET already provides
directly. The former numeric, parsing, Base64, null/empty string, GUID/date,
binary conversion, and stream-copy extensions were removed. Use the
corresponding framework APIs such as `Math.Min`, `Convert`, `Guid.TryParse`,
`DateTime.TryParse`, `string.IsNullOrEmpty`, `Encoding`, and `Stream.CopyTo`.

## Problems or Suggestions

[Open an issue here](https://github.com/CodingWithDavid/CodingwithDavidCommonLib/issues)

## Thank You


## Resources

- [VS Code](https://code.visualstudio.com)
- [Visual Studio]( https://visualstudio.microsoft.com/)
