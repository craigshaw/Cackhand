﻿using System;
using System.Collections.Generic;

namespace Cackhand.Core.Themes
{
    internal class ThemeManager
    {
        private int activeThemeIndex;
        private IList<Theme> registeredThemes;
        private static ThemeManager instance;

        private ThemeManager() 
        {
            registeredThemes = new List<Theme>();
            registeredThemes.Add(new Theme { PrimaryColour = ConsoleColor.Green, SecondaryColour = ConsoleColor.DarkGreen, TertiaryColour = ConsoleColor.DarkGray });
            activeThemeIndex = 0;
        }
        
        public static ThemeManager Instance
        {
            get
            {
                return instance ?? (instance = new ThemeManager());
            }
        }

        public void RegisterTheme(Theme theme)
        {
            if (theme == null)
                throw new ArgumentNullException("theme");

            if (!registeredThemes.Contains(theme))
                registeredThemes.Add(theme);
        }

        public void NextTheme()
        {
            activeThemeIndex = ++activeThemeIndex % registeredThemes.Count;
        }

        public Theme ActiveTheme
        {
            get
            {
                return registeredThemes[activeThemeIndex];
            }
        }
    }
}
