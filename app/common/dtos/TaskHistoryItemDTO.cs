using System;
using System.Runtime.Serialization;

namespace Hum.Common.DTOs
{
    [DataContract]
    public class TaskHistoryItemDTO
    {
        [DataMember(Order = 1, Name = "status")]
        public string Status {get;set;}

        [DataMember(Order = 2, Name = "dttm")]
        public string HistoryDate {get;set;}
    }
}
