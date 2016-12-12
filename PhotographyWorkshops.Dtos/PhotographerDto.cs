using PhotographyWorkshops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographyWorkshops.Dtos
{
    public class PhotographerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<int> Lenses { get; set; }
    }
}
