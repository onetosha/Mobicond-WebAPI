using System.Text.Json.Serialization;

namespace Mobicond_WebAPI.Models
{
    public class JobTitle
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
