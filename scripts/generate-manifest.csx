using System.Security.Cryptography;

Console.WriteLine("Generating manifest...");

var publishDir = new DirectoryInfo("bin/Release/net8.0/publish");
var manifestFile = new FileInfo(Path.Combine(publishDir.FullName, "manifest.json"));

if (!publishDir.Exists)
{
    Console.WriteLine("Publish directory not found. Skipping manifest generation.");
    return;
}

var manifest = new {
    Version = "1.0.0",
    Files = new Dictionary<string, string>()
};

using (var md5 = MD5.Create())
{
    foreach (var file in publishDir.GetFiles("*", SearchOption.AllDirectories))
    {
        // Skip the manifest file itself
        if (file.FullName == manifestFile.FullName) continue;

        using (var stream = file.OpenRead())
        {
            var hash = md5.ComputeHash(stream);
            var relativePath = Path.GetRelativePath(publishDir.FullName, file.FullName);
            manifest.Files.Add(relativePath, BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant());
        }
    }
}

File.WriteAllText(manifestFile.FullName, System.Text.Json.JsonSerializer.Serialize(manifest, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

Console.WriteLine("Manifest generated successfully.");
