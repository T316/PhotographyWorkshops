using AutoMapper;
using Newtonsoft.Json;
using PhotographyWorkshops.Data;
using PhotographyWorkshops.Dtos;
using PhotographyWorkshops.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PhotographyWorkshops.ImportJson
{
    class Program
    {
        private static string LensesPath = "../../../datasets/lenses.json";
        private static string CamerasPath = "../../../datasets/cameras.json";
        private static string PhotographersPath = "../../../datasets/photographers.json";
        private static string Error = "Error. Invalid data provided";
        static void Main()
        {
            UnitOfWork unit = new UnitOfWork();
            ConfigureMapping(unit);
            ImportLenses(unit);
            ImportCameras(unit);
            ImportPhotographers(unit);
        }

        private static void ImportPhotographers(UnitOfWork unit)
        {
            string json = File.ReadAllText(PhotographersPath);
            IEnumerable<PhotographerDto> photographersDto = JsonConvert.DeserializeObject<IEnumerable<PhotographerDto>>(json);
            foreach (PhotographerDto photographerDto in photographersDto)
            {
                if (photographerDto.FirstName == null || photographerDto.LastName == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                Random rand = new Random();
                int randomCamera = rand.Next(1, unit.Cameras.Count());
                var primaryCamera = unit.Cameras.First(p => p.Id == randomCamera);
                randomCamera = rand.Next(1, unit.Cameras.Count());
                var secondaryCamera = unit.Cameras.First(p => p.Id == randomCamera);

                var photographerLenses = new HashSet<Len>();
                foreach (var lenId in photographerDto.Lenses)
                {
                    if (unit.Lenses.First(l => l.Id == lenId) != null)
                    {
                        var len = unit.Lenses.First(l => l.Id == lenId);
                        photographerLenses.Add(len);
                    }
                }

                var photographerPhone = photographerDto.Phone;
                var regex = new Regex(@"\+\d{1,3}\/\d{8,10}");
                if (photographerPhone != null)
                {
                    if (!regex.IsMatch(photographerDto.Phone))
                    {
                        photographerPhone = null;
                        Console.WriteLine(Error);
                        continue;
                    }
                }

                Photographer photographer = Mapper.Map<Photographer>(photographerDto);
                photographer.Phone = photographerPhone;
                photographer.PrimaryCamera = primaryCamera;
                photographer.SecondaryCamera = secondaryCamera;
                photographer.Lens = photographerLenses;
                unit.Photographers.Add(photographer);
                unit.Commit();
                Console.WriteLine($"Successfully imported {photographer.FirstName} {photographer.LastName} | Lenses: {photographer.Lens.Count}");
            }
        }

        private static void ImportCameras(UnitOfWork unit)
        {
            string json = File.ReadAllText(CamerasPath);
            IEnumerable<CamerasDto> camerasDto = JsonConvert.DeserializeObject<IEnumerable<CamerasDto>>(json);
            foreach (CamerasDto cameraDto in camerasDto)
            {
                if (cameraDto.Make == null || cameraDto.Model == null || cameraDto.Type == null || cameraDto.MinISO == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                switch (cameraDto.Type)
                {
                    case "DSLR":
                        DSLRCamera dsleCamera = Mapper.Map<DSLRCamera>(cameraDto);
                        unit.DSLRCameras.Add(dsleCamera);
                        unit.Commit();
                        Console.WriteLine($"Successfully imported DSLR {dsleCamera.Make} {dsleCamera.Model}");
                        break;
                    case "Mirrorless":
                        MirrorlessCamera mirrorlessCamera = Mapper.Map<MirrorlessCamera>(cameraDto);
                        unit.MirrorlessCameras.Add(mirrorlessCamera);
                        unit.Commit();
                        Console.WriteLine($"Successfully imported Mirrorless {mirrorlessCamera.Make} {mirrorlessCamera.Model}");
                        break;
                    default:
                        Console.WriteLine(Error);
                        break;
                }
            }
        }

        private static void ImportLenses(UnitOfWork unit)
        {
            string json = File.ReadAllText(LensesPath);
            IEnumerable<LenDto> lensesDto = JsonConvert.DeserializeObject<IEnumerable<LenDto>>(json);
            foreach (LenDto lenDto in lensesDto)
            {
                if (lenDto.Make == null || lenDto.FocalLength == null || lenDto.MaxAperture == null || lenDto.CompatibleWith == null)
                {
                    Console.WriteLine(Error);
                    continue;
                }

                Len len = Mapper.Map<Len>(lenDto);
                unit.Lenses.Add(len);
                unit.Commit();
                Console.WriteLine($"Successfully imported {len.Make} {len.FocalLength}mm f{len.MaxAperture}");
            }
        }

        private static void ConfigureMapping(UnitOfWork unit)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<LenDto, Len>();
                config.CreateMap<CamerasDto, DSLRCamera>();
                config.CreateMap<CamerasDto, MirrorlessCamera>();
                config.CreateMap<PhotographerDto, Photographer>();
            });
        }
    }
}
