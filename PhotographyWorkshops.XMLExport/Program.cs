using PhotographyWorkshops.Data;
using System.Linq;
using System.Xml.Linq;

namespace PhotographyWorkshops.XMLExport
{
    class Program
    {
        static void Main()
        {
            UnitOfWork unit = new UnitOfWork();
            ExportPhotographerWithSameCamerasMake(unit);
            ExportWorkshopByLocation(unit);
        }

        private static void ExportWorkshopByLocation(UnitOfWork unit)
        {
            var exportWorkshopsByLocation = unit.Workshops
                .GetAll(w => w.Participant.Count >= 5).GroupBy(w => w.Location);

            XElement root = new XElement("locations");
            foreach (var location in exportWorkshopsByLocation)
            {
                XElement locationXml = new XElement("location");
                locationXml.SetAttributeValue("name", location.Key);

                foreach (var workshop in location)
                {
                    XElement workshopXml = new XElement("workshop");
                    var totalProfit = (workshop.Participant.Count * workshop.PricePerParticipant) * 0.8m;
                    workshopXml.SetAttributeValue("name", workshop.Name);
                    workshopXml.SetAttributeValue("total-profit", totalProfit);                    

                    XElement participantsXml = new XElement("participants");
                    participantsXml.SetAttributeValue("count", workshop.Participant.Count);
                    foreach (var participant in workshop.Participant)
                    {
                        XElement participantXml = new XElement("participant");
                        participantXml.SetValue(participant.FirstName + " " + participant.LastName);
                        participantsXml.Add(participantXml);
                    }

                    workshopXml.Add(participantsXml);
                    locationXml.Add(workshopXml);
                }

                root.Add(locationXml);
            }

            root.Save("../../../results/workshops-by-location.xml ");
        }

        private static void ExportPhotographerWithSameCamerasMake(UnitOfWork unit)
        {
            var exportPhotographers = unit.Photographers.GetAll(p => p.PrimaryCamera.Make == p.SecondaryCamera.Make).Select(p => new
            {
                FullName = p.FirstName + " " + p.LastName,
                PrimaryCameraMaek = p.PrimaryCamera.Make,
                PrimaryCameraModel = p.PrimaryCamera.Model,
                Lenses = p.Lens
            });

            XElement root = new XElement("photographers");
            foreach (var photographer in exportPhotographers)
            {
                XElement anomalyXml = new XElement("photographer");
                anomalyXml.SetAttributeValue("name", photographer.FullName);
                anomalyXml.SetAttributeValue("primary-camera", photographer.PrimaryCameraMaek + " " + photographer.PrimaryCameraModel);

                if (photographer.Lenses.Count != 0)
                {
                    XElement lensesXml = new XElement("lenses");
                    foreach (var lens in photographer.Lenses)
                    {
                        XElement lensXml = new XElement("lens");
                        lensXml.SetAttributeValue("name", $"{lens.Make} {lens.FocalLength}mm f{lens.MaxAperture}");
                        lensesXml.Add(lensXml);
                    }
                    anomalyXml.Add(lensesXml);
                }

                root.Add(anomalyXml);
            }

            root.Save("../../../results/anomalies.xml");
        }
    }
}
