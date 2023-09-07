<img src="https://github.com/alek5ey/CifsMount/blob/main/docs/img/CifsMount-Icon-64.png" alt="CifsMount"/>

# CifsMount

[![Nuget downloads](https://img.shields.io/nuget/v/cifsmount.svg)](https://www.nuget.org/packages/CifsMount/)
[![Nuget](https://img.shields.io/nuget/dt/cifsmount)](https://www.nuget.org/packages/CifsMount/)
[![Tests](https://github.com/alek5ey/CifsMount/actions/workflows/tests.yml/badge.svg)](https://github.com/alek5ey/CifsMount/actions/workflows/tests.yml)
[![CodeQL](https://github.com/alek5ey/CifsMount/actions/workflows/codeql.yml/badge.svg)](https://github.com/alek5ey/CifsMount/actions/workflows/codeql.yml)

**CifsMount is a lightweight .NET library that allows you to automatically mount shared Windows folders on Linux using the cifs-utils**

You can install [CifsMount with NuGet](https://www.nuget.org/packages/CifsMount/):

```
Install-Package CifsMount
```

## Requirements

1. Disable the password verification prompt for the user. For example, `test` user (change for your user!)
    ```shell
    sudo bash -c 'cat >> /etc/sudoers <<< "test ALL=(ALL) NOPASSWD: /usr/bin/mount, /usr/bin/umount"'
    ```
2. Create a directory to mount. Grant write permissions
    ```shell
    sudo mkdir -p /mnt/my_mount
    sudo chown test:test -R /mnt/my_mount
    ```

## Usage

1. Create mount options and define folders
2. Create `CifsMountClient` and `Mount()` target folder

## Example

```csharp
var shareDirectory = "//server123.domain.xyz/RootFolder/SubFolder"; // Windows shared folder
var targetDirectory = "/mnt/my_mount/";     // Local mounted directory
var options = new CifsMountOptions("user", "password", "domain.xyz")
{
    IsPersistence = false,
    Arguments = new []{ "rw" }
};

using (var cifsClient = new CifsMountClient(options))
using (var cifsMounted = cifsClient.Mount(shareDirectory, targetDirectory))
{
    Console.WriteLine(cifsMounted.Directory.Exists);
    
    // Do something with 'cifsMounted.Directory' like the plain DirectoryInfo type
}
```