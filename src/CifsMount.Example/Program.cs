using CifsMount;
using CifsMount.Models;

Console.WriteLine("Start");

var shareDirectory = "//server123.domain.xyz/RootFolder/SubFolder";
var targetDirectory = "/mnt/my_mount/";
var options = new CifsMountOptions("user", "password", "domain.xyz")
{
    IsPersistence = false,
    Arguments = new []{ "rw" }
};

using (var cifsClient = new CifsMountClient(false,options))
using (var cifsMounted = cifsClient.Mount(shareDirectory, targetDirectory))
{
    Console.WriteLine(cifsMounted.Directory.Exists);
    Console.WriteLine(cifsMounted.Directory.EnumerateFiles().Count());

    var filePath = Path.Combine(cifsMounted.Directory.FullName, $"{Guid.NewGuid().ToString()}.txt");
    await File.AppendAllTextAsync(filePath, string.Empty);

    Console.WriteLine(cifsMounted.Directory.EnumerateFiles().Count());

    await Task.Delay(2_000);

    File.Delete(filePath);

    Console.WriteLine(cifsMounted.Directory.EnumerateFiles().Count());
}

Console.WriteLine("end");