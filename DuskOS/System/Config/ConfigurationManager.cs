/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Config/ConfigurationManager.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */
using System;
using System.Collections.Generic;
using System.Text;
using DuskOS.System.Config.Managers;

namespace DuskOS.System.Config
{
    public class ConfigurationManager
    {
        public static void SaveConfig()
        {
            SystemManager.SaveConfig();
            TranslationManager.SaveConfig();
        }

        public static void LoadConfig()
        {
            SystemManager.LoadConfig();
            TranslationManager.LoadConfig();
        }

        public static void DiscardConfig()
        {
            SystemManager.DiscardConfig();
            TranslationManager.DiscardConfig();
        }
    }
}
