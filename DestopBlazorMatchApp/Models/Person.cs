using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DestopBlazorMatchApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public bool IsMale { get; set; }
        public List<Person> Likes { get; set; }
        public List<Person> Matches { get; set; }
    }
}
