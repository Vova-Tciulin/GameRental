namespace GameRental.Domain.Exceptions;

public class UserNotFoundException:NotFoundException
{
    public UserNotFoundException(string id) 
        : base($"Пользователь с id: {id} не найден.")
    {
    }
}