namespace Entities.Exceptions
{
    public class DtoBadRequestNullException : BadRequestException
    {
        public DtoBadRequestNullException(string message) : base(message)
        {
        }
    }
}
