namespace GameRental.Domain.Exceptions;

public class OrderNotFoundException:NotFoundException
{
    public OrderNotFoundException(int id)
        : base($"Заказ с id: {id} не найден.")
    {
        
    }
}