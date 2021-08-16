public static byte[] Compress(byte[] inputData)
{
    if (inputData == null)
        throw new ArgumentNullException("inputData must be non-null");

    using (var compressIntoMs = new MemoryStream())
    {
        using (var gzs = new BufferedStream(new GZipStream(compressIntoMs,
            CompressionMode.Compress), 64 * 1024))
            gzs.Write(inputData, 0, inputData.Length);
        return compressIntoMs.ToArray();
    }
}