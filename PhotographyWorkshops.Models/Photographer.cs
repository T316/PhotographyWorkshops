using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotographyWorkshops.Models
{
    public class Photographer
    {
        public Photographer()
        {
            this.Lens = new HashSet<Len>();
            this.Accessories = new HashSet<Accessory>();
            this.WorkshopsParticipate = new HashSet<Workshop>();
            this.WorkshopsTrainer = new HashSet<Workshop>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        public string Phone { get; set; }

        [Required]
        public virtual Camera PrimaryCamera { get; set; }

        [Required]
        public virtual Camera SecondaryCamera { get; set; }

        public virtual ICollection<Len> Lens { get; set; }

        public virtual ICollection<Accessory> Accessories { get; set; }

        public virtual ICollection<Workshop> WorkshopsParticipate { get; set; }

        public virtual ICollection<Workshop> WorkshopsTrainer { get; set; }
    }
}
