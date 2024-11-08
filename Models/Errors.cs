namespace medium_app_back.Models
{
    public class NotFoundException(string message) : Exception(message)
    {
    }

    public class ValidationException(string message) : Exception(message)
    {
    }
}
