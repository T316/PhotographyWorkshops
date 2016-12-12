using Newtonsoft.Json;
using PhotographyWorkshops.Data;
using System.IO;
using System.Linq;

namespace PhotographyWorkshops.ExportJson
{
    class Program
    {
        static void Main()
        {
            UnitOfWork unit = new UnitOfWork();
            ExportOrderedPhotographers(unit);
            ExportLandscapePhotographers(unit);
        }

        private static void ExportLandscapePhotographers(UnitOfWork unit)
        {
            var photographers = unit.Photographers
                .GetAll(p => p.Lens.All(l => l.FocalLength <= 30))
                .Select(p => new
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PrimaryCameraMake = p.PrimaryCamera.Make,
                    NumberOfLenses = p.Lens.Count(),
                    Type = p.PrimaryCamera.GetType().ToString()
                })
                .OrderBy(p => p.FirstName);

            var result = photographers
                .Where(p => p.Type.Contains("DSLR"))
                .Select(p => new
                {
                    p.FirstName,
                    p.LastName,
                    p.PrimaryCameraMake,
                    p.NumberOfLenses
                });

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText("../../../results/landscape-photogaphers.json", json);
        }

        private static void ExportOrderedPhotographers(UnitOfWork unit)
        {
            var photographers = unit.Photographers
                .GetAll()
                .Select(p => new
                {
                    p.FirstName,
                    p.LastName,
                    p.Phone
                })
            .OrderBy(p => p.FirstName)
            .ThenByDescending(p => p.LastName);

            string json = JsonConvert.SerializeObject(photographers, Formatting.Indented);
            File.WriteAllText("../../../results/photographers-ordered.json", json);
        }
    }
}
