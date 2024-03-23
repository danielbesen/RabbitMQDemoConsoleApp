using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitSender.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public bool Processed { get; set; }

        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}, Age: {Age}, Email: {Email}, Processed: {Processed}";
        }
    }
}
