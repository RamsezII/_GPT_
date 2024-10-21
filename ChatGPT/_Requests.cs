using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace _GPT_
{
    partial class ChatGPT
    {
        [Serializable]
        public class OpenAIRequest
        {
            public string model = "gpt-3.5-turbo";
            public Message[] messages;
            public int max_tokens = 100;
            public float temperature = 0.7f;

            [Serializable]
            public class Message
            {
                public string role = "user";
                public string content;
            }
        }

        [Serializable]
        public class OpenAIResponse
        {
            public Choice[] choices;

            [Serializable]
            public class Choice
            {
                public Message message;

                [Serializable]
                public class Message
                {
                    public string role;
                    public string content;
                    public string refusal;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        IEnumerator ESendRequest(string role, string prompt)
        {
            OpenAIRequest openAIRequest = new()
            {
                messages = new OpenAIRequest.Message[]
                {
                    new()
                    {
                        role = "system",
                        content = role,
                    },
                    new()
                    {
                        role = "user",
                        content = prompt,
                    },
                }
            };
            string jsonString = JsonConvert.SerializeObject(openAIRequest);

            // Créer une requête HTTP POST
            using UnityWebRequest request = new("https://api.openai.com/v1/chat/completions", "POST")
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString)),
                downloadHandler = new DownloadHandlerBuffer()
            };

            // Ajouter les en-têtes (clé API et type de contenu)
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // Envoyer la requête
            UnityWebRequestAsyncOperation sending = request.SendWebRequest();
            while (!sending.isDone)
                yield return null;

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Récupérer la réponse
                string responseText = request.downloadHandler.text;
                OpenAIResponse openAIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseText);

                if (openAIResponse != null)
                    Debug.Log(openAIResponse.choices[0].message.content);
                else
                    Debug.LogWarning($"Erreur de désérialisation de la réponse de l'IA: \n{responseText}");
            }
            else
                Debug.LogWarning($"Error: \"{request.error}\"");
        }
    }
}