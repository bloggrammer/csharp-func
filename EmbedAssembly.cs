using System;
using System.Reflection;

public class EmbedAssembly
{
    /// <summary>
    /// Embed And Reference An External Assembly (DLL)
    /// </summary>
    /// <param name="embedAssembly">Example: NameSpace.Mylib.dll</param>
    public EmbedAssembly(string embeddedAssembly)
    {
        _embedAssembly = embeddedAssembly;
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
    }

    private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_embedAssembly))
        {
            byte[] assemblyData = new byte[stream.Length];
            stream.Read(assemblyData, 0, assemblyData.Length);
            return Assembly.Load(assemblyData);
        }
    }
    private string _embedAssembly;
}