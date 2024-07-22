using Mobicond_WebAPI.Models.Enums;
using System.Text.Json.Serialization;

namespace Mobicond_WebAPI.Models
{
    //Служит для представления внутренней структуры организации через дерево
    public class HierarchyNode
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string Name { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))] //Нужно,чтобы м.б. вводить строку, но приходило Enum 
        public HierarchyType Type { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? ParentId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int DeptId { get; set; }
        public List<HierarchyNode> Children { get; set; } = new List<HierarchyNode>();

    }
}
