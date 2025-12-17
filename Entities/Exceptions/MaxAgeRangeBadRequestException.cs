namespace Entities.Exceptions
{
    public class MaxAgeRangeBadRequestException : BadRequestException
    {
        public MaxAgeRangeBadRequestException() : base("Invalid age range , minAge should be less that maxAge")
        {
        }
    }
}
