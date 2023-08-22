namespace GameRental.Domain.Exceptions;

public class CategoryNotFoundException:NotFoundException
{
    public CategoryNotFoundException(int id) 
        : base($"Категория с id: {id} не найдена.")
    {
    }
}