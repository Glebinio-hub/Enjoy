
using System;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using EnjoyApp.Models;

namespace EnjoyApp.Services
{
    public class ThemeService
    {
        private ResourceInclude? activeThemeDictionary;

        public EnjoyTheme CurrentTheme { get; private set; }
            = EnjoyTheme.Burgundy;

        public void ApplyTheme(EnjoyTheme theme)
        {
            if (theme != EnjoyTheme.Burgundy &&
    theme != EnjoyTheme.Monochrome)
            {
                return;
            }
            var app = Application.Current;

            if (app == null)
                return;

            var dictionaries = app.Resources.MergedDictionaries;

            if (activeThemeDictionary == null)
            {
                foreach (var item in dictionaries)
                {
                    if (item is ResourceInclude resource &&
                        resource.Source?.AbsolutePath.Contains("/Themes/") == true)
                    {
                        activeThemeDictionary = resource;
                        break;
                    }
                }
            }

            string fileName = theme switch
            {
                EnjoyTheme.Burgundy => "Burgundy.axaml",
                EnjoyTheme.Monochrome => "Monochrome.axaml",
                EnjoyTheme.DeepBlue => "DeepBlue.axaml",
                EnjoyTheme.PorcelainBlue => "PorcelainBlue.axaml",
                EnjoyTheme.Sage => "Sage.axaml",
                EnjoyTheme.Terracotta => "Terracotta.axaml",
                _ => "Burgundy.axaml"
            };

            var dictionary = new ResourceInclude((Uri?)null)
            {
                Source = new Uri($"avares://EnjoyApp/Themes/{fileName}")
            };
            dictionaries.Add(dictionary);

            if (activeThemeDictionary != null)
            {
                dictionaries.Remove(activeThemeDictionary);
            }

            activeThemeDictionary = dictionary;
            CurrentTheme = theme;
        }
    }
}
