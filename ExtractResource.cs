
using System.Reflection;
using System.IO;

public class ExtractResource
{
    /// <summary>
    /// Extract an Embedded Resource from an Assembly
    /// </summary>
    /// <param name="nameSpace">Calling assembly namespace</param>
    /// <param name="outDirectory">output file directory</param>
    /// <param name="internalFilePath">Resource file path witthin the calling assembly</param>
    /// <param name="resourceName">Name of resource to be extracted cluing its extension</param>
    public void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        var path = $"{nameSpace}.{(string.IsNullOrWhiteSpace(internalFilePath) ? string.Empty : internalFilePath)}.{resourceName}";
        using (Stream s = assembly.GetManifestResourceStream(path))
        using (BinaryReader r = new BinaryReader(s))
        using (FileStream fs = new FileStream($"{outDirectory}\\{resourceName}", FileMode.OpenOrCreate))
        using (BinaryWriter w = new BinaryWriter(fs))
            w.Write(r.ReadBytes((int)s.Length));
    }
    
      public static string Extract(string fileName)
        {
            var assembly = Assembly.GetCallingAssembly();

            string resourceName = assembly.GetManifestResourceNames()
                                        .Single(str => str.EndsWith(fileName));

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
}

