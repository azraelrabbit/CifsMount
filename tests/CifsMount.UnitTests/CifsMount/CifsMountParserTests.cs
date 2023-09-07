using CifsMount.Abstractions;
using CifsMount.Exceptions;

namespace CifsMount.UnitTests.CifsMount;

public class CifsMountParserTests
{
    [Fact]
    public void ParseCurrentUser_WithIdCommandResponse_ShouldSuccessful()
    {
        var idCommandResponse = "uid=1000(test) gid=1000(test) groups=1000(test),4(adm),24(cdrom),27(sudo),30(dip),46(plugdev),115(lpadmin),137(sambashare)";
        ICifsMountParser parser = new CifsMountParser();

        var result = parser.ParseCurrentUserInfo(idCommandResponse);

        result.Should().NotBeNull();
        result.Uid.Should().Be(1000);
        result.Gid.Should().Be(1000);
    }

    [Fact]
    public void ParseCurrentUser_WithBrokenIdCommandResponse_ShouldError()
    {
        var idCommandResponse = "uid=(test) gid=(test)";
        ICifsMountParser parser = new CifsMountParser();

        Action act = () => parser.ParseCurrentUserInfo(idCommandResponse);

        act.Should().Throw<CurrentUserReadException>();
    }
}