namespace EBC.Core.Exceptions;

public class EncryptionException : Exception
{
    public EncryptionException(string message, Exception innerException) : base(message, innerException) { }
}
