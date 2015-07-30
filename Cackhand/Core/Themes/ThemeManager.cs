using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            registeredThemes.Add(new Theme { PrimaryColour = ConsoleColor.DarkGreen, SecondaryColour = ConsoleColor.Green, TertiaryColour = ConsoleColor.DarkGray });
            activeThemeIndex = 0;
        }
        
        public static ThemeManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ThemeManager();

                return instance;
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
