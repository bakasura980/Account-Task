using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Innexys.Models
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

        [Required]
        public string Name { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"0\d{9}")]
        public string Phone { get; set; }

        public virtual ICollection<Task> Tasks
        {
            get
            {
                return this.tasks;
            }
        }

    }
}
