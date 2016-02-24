using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UoC_API.Models
{
    public class Assignment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int       ID          { get; set; }
        public string    Name        { get; set; }
        public DateTime? DueDateTime { get; set; }
        public double?   Weighting   { get; set; }
        public double?   Marks       { get; set; }
        public double?   Score       { get; set; }
        public string    LinkToWork  { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; }
    }
}
