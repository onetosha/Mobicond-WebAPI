using System.Text.Json.Serialization;

namespace Mobicond_WebAPI.Models
{
    public class User
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string RoleCode { get; set; } = "U";
    }
}
