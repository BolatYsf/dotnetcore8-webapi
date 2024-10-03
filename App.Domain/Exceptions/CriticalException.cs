namespace App.Domain.Exceptions;

public sealed class CriticalException(string message):Exception(message);

