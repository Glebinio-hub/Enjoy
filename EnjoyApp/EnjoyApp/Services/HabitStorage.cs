using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnjoyApp.Services
{
    public class HabitStorage
    {
        private string PathToSFolder;
        private string AppFolder;
        private string PathToFile;
        private readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true
        };

        public HabitStorage()
        {
            PathToSFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            AppFolder = Path.Combine(PathToSFolder, "EnjoyApp");
            Directory.CreateDirectory(AppFolder);
            PathToFile = Path.Combine(AppFolder, "habits.json");
            System.Diagnostics.Debug.WriteLine(PathToFile);
        }

        public async Task SaveAsync(List<HabitData> habits)
        {
            var json = JsonSerializer.Serialize(habits, jsonOptions);

            string tempPath = PathToFile + ".tmp";

            await File.WriteAllTextAsync(tempPath, json);

            File.Move(tempPath, PathToFile, overwrite: true);
        }

        public async Task<List<HabitData>> LoadAsync()
        {
            var habits = new List<HabitData>();
            if (File.Exists(PathToFile))
            {
                var json = await File.ReadAllTextAsync(PathToFile);
                habits = System.Text.Json.JsonSerializer.Deserialize<List<HabitData>>(json, jsonOptions) ?? new List<HabitData>();
                return habits;
            }
            else
            {
                return new List<HabitData>();
            }
        }
    }
}
