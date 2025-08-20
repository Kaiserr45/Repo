namespace IngoX.Client.Bff.Core.Models;

public enum ErrorKind
{
    /// <summary>
    /// default value.
    /// </summary>
    None,

    /// <summary>
    /// Unhandled server-side exception.
    /// </summary>
    Technical,

    /// <summary>
    /// Buisness logic exception.
    /// </summary>
    Business
}
