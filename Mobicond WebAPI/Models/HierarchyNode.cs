using Mobicond_WebAPI.Models.Enums;
using System.Text.Json.Serialization;

namespace Mobicond_WebAPI.Models
{
    public class HierarchyNode
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HierarchyType Type { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? ParentId { get; set; }
        public int DeptId { get; set; }
        public List<HierarchyNode> Children { get; set; } = new List<HierarchyNode>();

    }
}
