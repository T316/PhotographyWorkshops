using PhotographyWorkshops.Data;
using PhotographyWorkshops.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PhotographyWorkshops.XMLImport
{
    public class Program
    {
        private static string AccessoriesPath = "../../../datasets/accessories.xml";
        private static string WorkshopsPath = "../../../datasets/workshops.xml";
        private static string Error = "Error. Invalid data provided";

        static void Main()
        {
            UnitOfWork unit = new UnitOfWork();
            ImportAccessories(unit);
            ImportWorkshops(unit);
        }

        private static void ImportWorkshops(UnitOfWork unit)
        {
            XDocument document = XDocument.Load(WorkshopsPath);
            var workshopsXmls = document.Descendants("workshop");
            foreach (var workshopXml in workshopsXmls)
            {
                if (workshopXml.Attribute("name") == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                var workshopName = workshopXml.Attribute("name").Value;

                if (workshopXml.Attribute("location") == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                var workshopLocation = workshopXml.Attribute("location").Value;

                if (workshopXml.Attribute("price") == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }
                var workshopPrice = workshopXml.Attribute("price").Value;

                if (workshopXml.Element("trainer") == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                DateTime? workshopStartDate = null;
                if (workshopXml.Attribute("start-date") != null)
                {
                    workshopStartDate = DateTime.Parse(workshopXml.Attribute("start-date").Value);
                }

                DateTime? workshopEndDate = null;
                if (workshopXml.Attribute("end-date") != null)
                {
                    workshopEndDate = DateTime.Parse(workshopXml.Attribute("end-date").Value);
                }

                var workshopTrainerFullName = workshopXml.Element("trainer").Value.Split();
                var trainerFirstName = workshopTrainerFullName[0];
                var trainerLastName = workshopTrainerFullName[1];

                var participians = new HashSet<Photographer>();
                if (workshopXml.Element("participants") != null)
                {
                    var participantsXmls = workshopXml.Element("participants").Descendants("participant");
                    foreach (var parXml in participantsXmls)
                    {
                        var parFirstName = parXml.Attribute("first-name").Value;
                        var parLastName = parXml.Attribute("last-name").Value;
                        Photographer photographer = unit.Photographers.First(p => p.FirstName == parFirstName && p.LastName == parLastName);
                        participians.Add(photographer);
                    }
                }

                Workshop workshop = new Workshop();
                workshop.Name = workshopName;
                workshop.Startdate = workshopStartDate;
                workshop.EndDate = workshopEndDate;
                workshop.Location = workshopLocation;
                workshop.PricePerParticipant = Decimal.Parse(workshopPrice.Replace('.', ','));
                workshop.Participant = participians;
                workshop.Trainer = unit.Photographers.First(p => p.FirstName == trainerFirstName && p.LastName == trainerLastName);
                unit.Workshops.Add(workshop);
                unit.Commit();
                Console.WriteLine($"Successfully imported {workshop.Name}");
            }
        }

        private static void ImportAccessories(UnitOfWork unit)
        {
            XDocument document = XDocument.Load(AccessoriesPath);
            var accessoriesXmls = document.Descendants("accessory");
            foreach (var accessoriesXml in accessoriesXmls)
            {
                var accessoryName = accessoriesXml.Attribute("name");
                Random rand = new Random();
                int randomPhotographer = rand.Next(1, unit.Photographers.Count());
                var owner = unit.Photographers.First(p => p.Id == randomPhotographer);
                Accessory accessory = new Accessory();
                accessory.Name = accessoryName.Value;
                accessory.Owner = owner;
                unit.Accessories.Add(accessory);
                unit.Commit();
                Console.WriteLine($"Successfully imported {accessory.Name}");
            }
        }
    }
}
