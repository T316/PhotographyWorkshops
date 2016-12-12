using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographyWorkshops.Dtos
{
    public class LenDto
    {
        public string Make { get; set; }

        public int? FocalLength { get; set; }

        public decimal? MaxAperture { get; set; }

        public string CompatibleWith { get; set; }
    }
}
