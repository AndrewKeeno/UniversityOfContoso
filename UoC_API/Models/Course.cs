using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UoC_API.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int    ID                   { get; set; }
        public string Title                { get; set; }
        public string Description          { get; set; }
        public int?   Credits              { get; set; }
        public int?   CompletionPercentage { get; set; }
        public Grade? Grade                { get; set; }

        [JsonIgnore]
        public virtual Student                 Student     { get; set; }
        [JsonIgnore]
        public virtual ICollection<Assignment> Assignments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Test>       Tests       { get; set; }
    }

    public enum Grade { Ap = 9, A = 8, Am = 7, Bp = 6, B = 5, Bm = 4, Cp = 3, C = 2, Cm = 1, Dp = 0, D = -1, Dm = -2, F = -3 }
}