# csharp-func

This repo consists of C# functions to help you with your .Net app development. Feel free to modify the code to meet your need. 

| C# File|  Method        | Descriptions    |
| --------------   | ---------------|-----------------|
| [ExtractResource.cs](https://github.com/Blogrammer/csharp-func/blob/main/ExtractResource.cs "ExtractResource.cs") | Extract(`string` nameSpace, `string` outDirectory, `string` internalFilePath, `string` resourceName)| Extract an embedded resource to a specified output directory|
|[WPFGlobalExceptionHandler.cs](https://github.com/Blogrammer/csharp-func/blob/main/WPFGlobalExceptionHandler.cs "WPFGlobalExceptionHandler.cs")| GlobalExceptionHandler()| Handled every unhandled exception in WPF application.|
|[EmbedAssembly.cs](https://github.com/Blogrammer/csharp-func/blob/main/EmbedAssembly.cs "EmbedAssembly.cs")|CurrentDomain_AssemblyResolve(`object`  sender, `ResolveEventArgs`  args)|Embed and reference an external .NET assembly (dll) inside of your own Windows Form/WPF Application. This is useful if you want to ship an application that relies on external libraries, but you only want to ship one executable file.|
| [ReStartWPFApp.cs](https://github.com/Blogrammer/csharp-func/blob/main/ReStartWPFApp.cs "ReStartWPFApp.cs") | | Restart a WPF application.|
