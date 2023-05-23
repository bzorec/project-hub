namespace Direct4Me.Blazor.Utils;

public static class Exstensions
{
    private const string Admin = "bzorec@gmail.com";

    public static bool IsAdmin(this string email)
    {
        return Admin == email;
    }
}