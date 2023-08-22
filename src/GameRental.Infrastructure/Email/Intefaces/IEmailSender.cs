namespace GameRental.Infrastructure.Email.Intefaces;

public interface IEmailSender
{
    Task SendEmailAsync(Message message);
}