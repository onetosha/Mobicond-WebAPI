using System.Text.Json.Serialization;

namespace Mobicond_WebAPI.Models
{
    public class Employee
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public int UserId { get; set; }
        public int DeptId { get; set; }
        public int PosId { get; set; }
    }
}
