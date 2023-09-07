using CifsMount.Abstractions;
using CifsMount.Exceptions;
using CifsMount.Models;

namespace CifsMount.UnitTests.CifsMount;

public class CifsMountClientTests
{
    [Fact]
    public void Mount_WithMoqExecutors_ShouldSuccessful()
    {
        var sharedDirectory = "//server/RootFolder/SubFolder";
        var localDirectory = "./mnt/test";
        var idMessageResult = "uid=1000(test) gid=1000(test)";

        try
        {
            var isCheckedPossibleExecute = false;
            var options = new CifsMountOptions();
            var mockExecutor = new Mock<ICifsMountExecutor>();
            mockExecutor.Setup(r => r.RunCommand("id", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(true, idMessageResult));
            mockExecutor.Setup(r => r.RunCommand("findmnt", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(true, string.Empty));
            mockExecutor.Setup(r => r.RunCommand("mount", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(true, string.Empty));
            var mockParser = new Mock<ICifsMountParser>();
            mockParser.Setup(r => r.ParseCurrentUserInfo(idMessageResult))
                .Returns(new CurrentUserInfo(1000, 1000));
            var parserExecutor = new Mock<ICifsMountValidator>();
            parserExecutor.Setup(r => r.ThrowIfNotPossibleExecuteInCurrentSystemEnvironment())
                .Callback(() => isCheckedPossibleExecute = true);
            var mountClient = new CifsMountClient(options, mockExecutor.Object, mockParser.Object, parserExecutor.Object);

            var result = mountClient.Mount(sharedDirectory, localDirectory);

            result.Should().NotBeNull();
            result.ShareDirectory.Should().Be(sharedDirectory);
            result.Directory.Exists.Should().Be(true);
            isCheckedPossibleExecute.Should().Be(true);
        }
        finally
        {
            CleanupAfterTest(localDirectory);
        }
    }

    [Fact]
    public void Mount_WithMoqExecutors_ErrorCheckIdCommand()
    {
        var sharedDirectory = "//server/RootFolder/SubFolder";
        var localDirectory = "./mnt/test";
        var idMessageResult = "uid=1000(test) gid=1000(test)";

        try
        {
            var options = new CifsMountOptions();
            var mockExecutor = new Mock<ICifsMountExecutor>();
            mockExecutor.Setup(r => r.RunCommand("id", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(false, idMessageResult));
            var mockParser = new Mock<ICifsMountParser>();
            var parserExecutor = new Mock<ICifsMountValidator>();
            var mountClient = new CifsMountClient(options, mockExecutor.Object, mockParser.Object, parserExecutor.Object);

            Action act = () =>  mountClient.Mount(sharedDirectory, localDirectory);

            act.Should().Throw<CurrentUserReadException>();
        }
        finally
        {
            CleanupAfterTest(localDirectory);
        }
    }

    [Fact]
    public void Mount_WithMoqExecutors_ErrorFindMntCommand()
    {
        var sharedDirectory = "//server/RootFolder/SubFolder";
        var localDirectory = "./mnt/test";
        var idMessageResult = "uid=1000(test) gid=1000(test)";

        try
        {
            var options = new CifsMountOptions();
            var mockExecutor = new Mock<ICifsMountExecutor>();
            mockExecutor.Setup(r => r.RunCommand("id", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(true, idMessageResult));
            mockExecutor.Setup(r => r.RunCommand("findmnt", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(false, string.Empty));
            var mockParser = new Mock<ICifsMountParser>();
            var parserExecutor = new Mock<ICifsMountValidator>();
            var mountClient = new CifsMountClient(options, mockExecutor.Object, mockParser.Object, parserExecutor.Object);

            new DirectoryInfo(localDirectory).Create();
            Action act = () =>  mountClient.Mount(sharedDirectory, localDirectory);

            act.Should().Throw<FindMntException>();
        }
        finally
        {
            CleanupAfterTest(localDirectory);
        }
    }

    [Fact]
    public void Mount_WithMoqExecutors_ErrorMountCommand()
    {
        var sharedDirectory = "//server/RootFolder/SubFolder";
        var localDirectory = "./mnt/test";
        var idMessageResult = "uid=1000(test) gid=1000(test)";

        try
        {
            var options = new CifsMountOptions();
            var mockExecutor = new Mock<ICifsMountExecutor>();
            mockExecutor.Setup(r => r.RunCommand("id", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(true, idMessageResult));
            mockExecutor.Setup(r => r.RunCommand("findmnt", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(true, string.Empty));
            mockExecutor.Setup(r => r.RunCommand("mount", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new RunCommandResult(false, string.Empty));
            var mockParser = new Mock<ICifsMountParser>();
            mockParser.Setup(r => r.ParseCurrentUserInfo(idMessageResult))
                .Returns(new CurrentUserInfo(1000, 1000));
            var parserExecutor = new Mock<ICifsMountValidator>();
            var mountClient = new CifsMountClient(options, mockExecutor.Object, mockParser.Object, parserExecutor.Object);

            Action act = () =>  mountClient.Mount(sharedDirectory, localDirectory);

            act.Should().Throw<UnableMountException>();
        }
        finally
        {
            CleanupAfterTest(localDirectory);
        }
    }

    private void CleanupAfterTest(string localDirectory)
    {
        var localMountDirectory = new DirectoryInfo(localDirectory);
        if (localMountDirectory.Exists)
            localMountDirectory.Parent?.Delete(true);
    }
}