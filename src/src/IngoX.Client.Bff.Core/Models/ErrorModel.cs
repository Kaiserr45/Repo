namespace IngoX.Client.Bff.Core.Models;

public sealed record ErrorModel(
    string Title,
    ErrorKind ErrorKind,
    string Detail)
{
    public static readonly ErrorModel None = new(string.Empty, ErrorKind.None, string.Empty);
}
