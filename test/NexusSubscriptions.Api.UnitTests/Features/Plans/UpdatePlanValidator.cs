using FluentValidation.TestHelper;
using NexusSubscriptions.Api.Features.Plans;

namespace NexusSubscriptions.UnitTests.Features.Plans;

public class UpdatePlanValidatorTests
{
    private readonly UpdatePlanValidator validator;

    public UpdatePlanValidatorTests()
    {
        validator = new UpdatePlanValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new UpdatePlanDTO("", 1m);

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(request => request.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Length_Is_Out_Of_Range()
    {
        var model = new UpdatePlanDTO("This is a very long description", 1m);

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(request => request.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Less_Than_Or_Equal_To_Zero()
    {
        var model = new UpdatePlanDTO("Plan description", -1m);

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(request => request.Price);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Request_Is_Perfect()
    {
        var model = new UpdatePlanDTO("Plan description", 10m);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
