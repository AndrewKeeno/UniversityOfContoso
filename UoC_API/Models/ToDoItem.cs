using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UoC_API.Models
{
    public class ToDoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int       ID          { get; set; }
        public string    Message     { get; set; }
        public DateTime? SubmittedOn { get; set; }

        [JsonIgnore]
        public virtual Student Student { get; set; }
    }
}
