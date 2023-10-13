using FluentAssertions;
using FluentValidation;
using SportBids.Application.Tournaments.Commands.CreateTournament;

namespace SportBids.Application.UnitTests.Tournaments.Create;

public class ValidationsTest
{
    private readonly AbstractValidator<CreateTournamentCommand> _sut;
    public ValidationsTest()
    {
        _sut = new CreateTournamentCommandValidation();
    }

    [Fact]
    public void CreateTournamentCommandValidation_When_NameIsNull_Should_Fail()
    {
        // Arrange
        var command = CreateValidCreateTournamentCommand();
        command.Name = "";

        // Act
        var response = _sut.Validate(command);

        // Assert
        response.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateTournamentCommandValidation_When_NumberOfGroupsLessThenOne_Should_Fail()
    {
        // Arrange
        var command = CreateValidCreateTournamentCommand();
        command.NumberOfGroups = 0;

        // Act
        var response = _sut.Validate(command);

        // Assert
        response.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateTournamentCommandValidation_When_NumberOfGroupsLessThen2_Should_Fail()
    {
        // Arrange
        var command = CreateValidCreateTournamentCommand();
        command.NumberOfTeams = 1;

        // Act
        var response = _sut.Validate(command);

        // Assert
        response.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(4, 4, true)]
    [InlineData(16, 4, true)]
    [InlineData(16, 5, false)]
    public void CreateTournamentCommandValidation_When_NumberOfTeamsNotDividableByNumberOfGroups_Should_Fail(int numOfTeams, int numOfGroups, bool isValid)
    {
        // Arrange
        var command = CreateValidCreateTournamentCommand();
        command.NumberOfTeams = numOfTeams;
        command.NumberOfGroups = numOfGroups;

        // Act
        var response = _sut.Validate(command);

        // Assert
        response.IsValid.Should().Be(isValid);
    }

    [Fact]
    public void CreateTournamentCommandValidation_When_StartAtGreaterThenFinishAt_Should_Fail()
    {
        // Arrange
        var command = CreateValidCreateTournamentCommand();
        command.StartAt = DateTime.Today;
        command.FinishAt = DateTime.Today.AddDays(-10);

        // Act
        var response = _sut.Validate(command);

        // Assert
        response.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateTournamentCommandValidation_When_Valid_Should_Success()
    {
        // Arrange
        var command = CreateValidCreateTournamentCommand();

        // Act
        var response = _sut.Validate(command);

        // Assert
        response.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateTournamentCommandValidation_When_FinishAtNotSet_Should_Fail()
    {
        // Arrange
        var command = CreateValidCreateTournamentCommand();
        command.FinishAt = default;

        // Act
        var response = _sut.Validate(command);

        // Assert
        response.IsValid.Should().BeFalse();
    }

    private static CreateTournamentCommand CreateValidCreateTournamentCommand()
    {
        return new CreateTournamentCommand()
        {
            Name = "asd",
            NumberOfGroups = 1,
            NumberOfTeams = 2,
            StartAt = DateTime.Now.AddMonths(-1),
            FinishAt = DateTime.Now.AddMonths(2),
        };
    }
}
