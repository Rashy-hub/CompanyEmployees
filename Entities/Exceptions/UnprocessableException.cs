using System.Collections.ObjectModel;

namespace Entities.Exceptions
{
    public class UnprocessableException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public UnprocessableException(IDictionary<string, string[]> errors): base("One or more validation errors occurred.")
        {
            Errors = new ReadOnlyDictionary<string, string[]>(errors);
        }
    }
}
