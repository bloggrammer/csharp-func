Public void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            var path = $"{nameSpace}.{(string.IsNullOrWhiteSpace(internalFilePath) ? string.Empty : internalFilePath)}.{resourceName}";
            using (Stream s =assembly.GetManifestResourceStream(path))
                using(BinaryReader r =new BinaryReader(s))
                    using(FileStream fs = new FileStream($"{outDirectory}\\{resourceName}", FileMode.OpenOrCreate))
                        using (BinaryWriter w = new BinaryWriter(fs))
                             w.Write(r.ReadBytes((int)s.Length));
        }