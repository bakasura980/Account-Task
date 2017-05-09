using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiTesting.Models
{
    public class Account
    {

        private ICollection<Task> tasks;

        public Account()
        {
            this.tasks = new HashSet<Task>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        public string Phone { get; set; }


        public virtual ICollection<Task> Tasks { get; set; }

    }
}