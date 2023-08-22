namespace GameRental.Domain.Exceptions;

public class ProductNotFoundExeption:NotFoundException
{
    public ProductNotFoundExeption(int id) 
        : base($"Продукт с id: {id} не найден.")
    {
        
    }
}