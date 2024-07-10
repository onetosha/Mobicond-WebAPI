using System.Text.Json.Serialization;

namespace Mobicond_WebAPI.Models
{
    public class Department
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrgId { get; set; }
    }
}
