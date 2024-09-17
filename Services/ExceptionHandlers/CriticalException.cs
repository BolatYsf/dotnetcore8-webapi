namespace App.Services.ExceptionHandlers
{
    public sealed class CriticalException(string message):Exception(message);
   
}
