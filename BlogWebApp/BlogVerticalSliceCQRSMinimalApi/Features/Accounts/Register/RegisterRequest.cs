namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Register;

public class RegisterRequest(string Email, string Password, string ConfirmPassword, string FirstName,
    string LastName, string UserName, string PhoneNumber, string ConcurrencyStamp, IEnumerable<string> Roles);