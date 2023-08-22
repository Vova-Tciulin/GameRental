namespace GameRental.Application.Exceptions;

public class NotValidateModelException:Exception
{
    public NotValidateModelException(string model,string error)
    :base($"Модель {model} не прошла валидацию.\n {error}" )
    {
        
    }
}