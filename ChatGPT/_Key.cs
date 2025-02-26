using System.IO;
using UnityEngine;

namespace _GPT_
{
    partial class ChatGPT
    {
        public static string GPT_homedir => Path.Combine(Util.home_path, nameof(_GPT_)).ForceDir().FullName;
        static string KeyPath => Path.Combine(GPT_homedir, "APIKEY" + JSon.txt);

        [Header("~@ Texts @~")]
        [SerializeField] string apiKey;

        //--------------------------------------------------------------------------------------------------------------

        void TryReadKey()
        {
            if (File.Exists(KeyPath))
                apiKey = File.ReadAllText(KeyPath);
            else
                File.WriteAllText(KeyPath, apiKey);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Debug.LogWarning($"API Key not set! Destroying {this}");
                Destroy(gameObject);
            }
        }
    }
}