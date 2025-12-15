using System.Text.Json;
using System.Text.Json.Serialization;

namespace Entities.ErrorModel
{
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]        
        public IReadOnlyDictionary<string, string[]>? Errors { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
