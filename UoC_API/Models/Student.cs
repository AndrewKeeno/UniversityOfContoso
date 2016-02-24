using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UoC_API.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int    ID           { get; set; }
        public string LastName     { get; set; }
        public string FirstMidName { get; set; }
        public string Email        { get; set; }

        [JsonIgnore]
        public virtual ICollection<Course>   Courses   { get; set; }
        [JsonIgnore]
        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
