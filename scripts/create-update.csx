using System.Text.Json;

Console.WriteLine("Creating update package...");

var releaseDir = new DirectoryInfo("release");
var publishDir = new DirectoryInfo("bin/Release/net8.0/publish");
var updateDir = new DirectoryInfo(Path.Combine(releaseDir.FullName, "update"));

var newManifestFile = new FileInfo(Path.Combine(publishDir.FullName, "manifest.json"));
var oldManifestFile = new FileInfo(Path.Combine(releaseDir.FullName, "manifest.json"));

if (!newManifestFile.Exists)
{
    Console.WriteLine("New manifest not found. Please run a full publish first.");
    return;
}

if (!oldManifestFile.Exists)
{
    Console.WriteLine("Old manifest not found. Copying all files from publish directory.");
    CopyDirectory(publishDir.FullName, updateDir.FullName);
    File.Copy(newManifestFile.FullName, oldManifestFile.FullName, true);
    return;
}

var newManifest = JsonSerializer.Deserialize<Manifest>(File.ReadAllText(newManifestFile.FullName));
var oldManifest = JsonSerializer.Deserialize<Manifest>(File.ReadAllText(oldManifestFile.FullName));

if (updateDir.Exists)
{
    updateDir.Delete(true);
}
updateDir.Create();

foreach (var newFile in newManifest.Files)
{
    if (!oldManifest.Files.ContainsKey(newFile.Key) || oldManifest.Files[newFile.Key] != newFile.Value)
    {
        var sourceFile = new FileInfo(Path.Combine(publishDir.FullName, newFile.Key));
        var destFile = new FileInfo(Path.Combine(updateDir.FullName, newFile.Key));

        if (!destFile.Directory.Exists)
        {
            destFile.Directory.Create();
        }

        sourceFile.CopyTo(destFile.FullName);
        Console.WriteLine($"Added file: {newFile.Key}");
    }
}

// After creating the update, copy the new manifest to the release directory for the next update.
File.Copy(newManifestFile.FullName, oldManifestFile.FullName, true);

Console.WriteLine("Update package created successfully.");


void CopyDirectory(string sourceDir, string destinationDir)
{
    var dir = new DirectoryInfo(sourceDir);
    if (!dir.Exists) return;

    Directory.CreateDirectory(destinationDir);

    foreach (var file in dir.GetFiles())
    {
        file.CopyTo(Path.Combine(destinationDir, file.Name), true);
    }

    foreach (var subDir in dir.GetDirectories())
    {
        CopyDirectory(subDir.FullName, Path.Combine(destinationDir, subDir.Name));
    }
}

public class Manifest
{
    public string Version { get; set; }
    public Dictionary<string, string> Files { get; set; }
}
