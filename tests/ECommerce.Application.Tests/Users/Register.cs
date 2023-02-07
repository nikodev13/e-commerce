namespace ECommerce.Application.Tests.Users;

[Collection(TestingCollection.Name)]
public class Register
{
    private readonly Testing _testing;

    public Register(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task UserRegister_WithEmailThatAlreadyExists_ThrowsBadRequest()
    {
        
    }
}