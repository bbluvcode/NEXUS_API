namespace NEXUS_API.Helpers
{
    public interface IUserAuth
    {
        string Email { get; set; }
        string Password { get; set; }
        string? RefreshToken { get; set; }
        DateTime? RefreshTokenExpried { get; set; }
        int FailedLoginAttempts { get; set; }
        DateTime? ExpiredBan { get; set; }
        string? Code { get; set; }
        DateTime? ExpiredCode { get; set; }
        int SendCodeAttempts { get; set; }
        DateTime? LastSendCodeDate { get; set; }
    }
}
