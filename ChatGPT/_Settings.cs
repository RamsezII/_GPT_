using System;
using System.IO;
using UnityEngine;

namespace _GPT_
{
    partial class ChatGPT
    {
        [Serializable]
        class Settings
        {
            [Serializable]
            public class Role
            {
                public string name, description;
            }

            public Role[] roles = new Role[]
            {
                new() { name = "Admin_FR", description = "Can do anything" },
                new() { name = "Admin_EN", description = "Peut faire n'importe quoi" },
                new() { name = "ATHENA_FR", description = "Tu es ATHÉNA, une IA intense, brutale, énervée et philosophique" },
                new() { name = "ATHENA_EN", description = "You are ATHENA, an intense, brutal, angry and philosophical AI" },
            };

            public static string FilePath => Path.Combine(GPT_streamdir, typeof(Settings).FullName + JSon.json);

            //--------------------------------------------------------------------------------------------------------------

            public bool TryGetRole(in string name, out Role role)
            {
                foreach (Role r in roles)
                    if (r.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        role = r;
                        return true;
                    }
                role = null;
                return false;
            }
        }

        public static string GPT_streamdir => Path.Combine(Application.streamingAssetsPath, nameof(_GPT_)).GetDir().FullName;

        [Header("~@ Settings @~")]
        [SerializeField] Settings settings;

        //--------------------------------------------------------------------------------------------------------------

        void LoadSettings()
        {
            settings = new Settings();
            if (File.Exists(Settings.FilePath))
                settings = JsonUtility.FromJson<Settings>(File.ReadAllText(Settings.FilePath));
            else
                File.WriteAllText(Settings.FilePath, JsonUtility.ToJson(settings, true));
        }
    }
}