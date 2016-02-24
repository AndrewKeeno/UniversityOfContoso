using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UoC_API.Models
{
    public class Test
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int       ID           { get; set; }
        public string    Name         { get; set; }
        public DateTime? TestDateTime { get; set; }
        public double?   Weighting    { get; set; }
        public double?   Marks        { get; set; }
        public double?   Score        { get; set; }

        [JsonIgnore]
        public virtual Course Course { get; set; }
    }
}
