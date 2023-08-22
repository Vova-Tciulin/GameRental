namespace GameRental.Application.SomeLogic;

public class OperationDetails
{
    public OperationDetails( bool succedeed, string message,string property)
    {
        Property = property;
        Succedeed = succedeed;
        Message = message;
    }

    public bool Succedeed { get; private set;}
    public string Message { get; private set;}
    public string Property { get; private set;}
}