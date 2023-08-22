namespace GameRental.Domain.Exceptions;

public class AccountNotFoundException:NotFoundException
{
    public AccountNotFoundException(int id)
        : base($"Категория с id: {id} не найдена.")
    {
    }
}