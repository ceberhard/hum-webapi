using System;
using System.Runtime.Serialization;

namespace hum_webapi.DTOs
{
    [DataContract]
    public class TaskHistoryItemDTO
    {
        [DataMember(Order = 1, Name = "status")]
        public string Status {get;set;}

        [DataMember(Order = 2, Name = "dttm")]
        public DateTime HistoryDate {get;set;}
    }
}
