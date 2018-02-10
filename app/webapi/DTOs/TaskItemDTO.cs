using System;
using System.Runtime.Serialization;

namespace hum_webapi.DTOs
{
    [DataContract]
    public class TaskItemDTO
    {
        [DataMember(Order = 1, Name = "id")]
        public int Id {get;set;}

        [DataMember(Order = 2, Name = "title")]
        public string Title {get;set;}

        [DataMember(Order = 3, Name = "description")]
        public string Description {get;set;}

        // [DataMember(Order = 4, Name = "status")]
        // public string Status {get;set;}

        // [DataMember(Order = 5, Name = "status_update")]
        // public DateTime? StatusUpdate {get;set;}

        [DataMember(Order = 4, Name = "history")]
        public TaskHistoryItemDTO[] History {get;set;}
    }
}

