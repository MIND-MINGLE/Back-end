using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Entity
{
    public class Category
    {
        [Key]
        public required string CategoryId { get; set; }
        public required QuestionType Name { get; set; }
        public required string Description { get; set; }

        //navigate property
        public ICollection<Question>? Questions { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuestionType
    {
        [EnumMember(Value = "PHQ-9")]
        PHQ9,
        [EnumMember(Value = "GAD-7")]
        GAD7,
        [EnumMember(Value = "PC-PTSD-5")]
        PCPTSD5,
    }
}