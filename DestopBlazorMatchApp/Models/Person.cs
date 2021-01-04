using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DestopBlazorMatchApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _name = _name.Substring(0, 1).ToUpper() + _name.Substring(1);
            }
        }
        private string _lastName;
        [Required]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                _lastName = _lastName.Substring(0, 1).ToUpper() + _lastName.Substring(1);
            }
        }
        private string _NickName;

        public string NickName
        {
            get
            {
                return _NickName = _name.Substring(0, 1) + "." + _lastName;
            }

        }


        [Required]
        public bool IsMale { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();
    }
}
