using System;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Domain.Entity
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageStatus
    {
        [EnumMember(Value = "Sent")]
        SENT,
        [EnumMember(Value = "Received")]
        Received,
        [EnumMember(Value = "Seen")]
        SEEN,
        [EnumMember(Value = "Delete")]
        DELETE
    }
}

