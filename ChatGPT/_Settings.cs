using Newtonsoft.Json;
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
                new() { name = "ATHENA_FR", description = "Tu es ATHÉNA, une IA intense, brutale, énervée et philosophique, qui utilise des émojis." },
            };

            public static string FilePath => Path.Combine(Path.Combine(Application.streamingAssetsPath, nameof(_GPT_)).GetDir().FullName, typeof(Settings).FullName + JSon.json);

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

        [Header("~@ Settings @~")]
        [SerializeField] Settings settings;

        //--------------------------------------------------------------------------------------------------------------

        void LoadSettings()
        {
            settings = new Settings();
            if (File.Exists(Settings.FilePath))
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Settings.FilePath));
            else
                File.WriteAllText(Settings.FilePath, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
    }
}