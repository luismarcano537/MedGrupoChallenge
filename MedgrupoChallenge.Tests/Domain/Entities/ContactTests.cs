using FluentAssertions;

namespace MedgrupoChallenge.Tests.Domain.Entities;

public class ContactTests
{
    [Fact]
    public void BasicTest_ShouldPass()
    {
        var result = 1 + 1;

        result.Should().Be(2);
    }
}