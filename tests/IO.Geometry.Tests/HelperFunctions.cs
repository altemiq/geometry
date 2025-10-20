namespace Altemiq.IO.Geometry;

internal static class HelperFunctions
{
    public static byte[] GetByteArrayFromResource(string resourceName)
    {
        using var stream = typeof(HelperFunctions).Assembly.GetManifestResourceStream(typeof(HelperFunctions).Namespace + ".Resources." + resourceName) ?? throw new InvalidOperationException();
        var byteArray = new byte[stream.Length];
        _ = stream.Read(byteArray, 0, byteArray.Length);
        return byteArray;
    }
}