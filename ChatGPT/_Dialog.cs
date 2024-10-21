using _TERMINAL_;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using _ARK_;
using System;

namespace _GPT_
{
    partial class ChatGPT
    {
        class Dialog : Command
        {
            [Serializable]
            public class Message
            {
                public string role;
                public string content;
                public string refusal;
            }

            [Serializable]
            public class OpenAIRequest
            {
                public string model = "gpt-3.5-turbo";
                public List<Message> messages;
                public int max_tokens = 4096;
                public float temperature = 1;
            }

            [Serializable]
            public class OpenAIResponse
            {
                [Serializable]
                public class Choice
                {
                    public Message message;
                }

                public Choice[] choices;
            }

            readonly OpenAIRequest openAIRequest = new() { messages = new(), };

            //----------------------------------------------------------------------------------------------------------

            public Dialog(string role, string prompt)
            {
                flags &= ~(Flags.Stdin | Flags.Killable);

                openAIRequest.messages.Add(new Message
                {
                    role = "system",
                    content = role,
                });

                openAIRequest.messages.Add(new Message
                {
                    role = "user",
                    content = prompt,
                });

                NUCLEOR.instance.scheduler.AddRoutine(ESendRequest());
            }

            //--------------------------------------------------------------------------------------------------------------

            public override void OnCmdLine(in LineParser line)
            {
                openAIRequest.messages.Add(new Message
                {
                    role = "user",
                    content = line.ReadAll(),
                });
                flags &= ~(Flags.Stdin | Flags.Killable);
                NUCLEOR.instance.scheduler.AddRoutine(ESendRequest());
            }

            IEnumerator ESendRequest()
            {
                string jsonString = JsonUtility.ToJson(openAIRequest, true);

                using UnityWebRequest request = new("https://api.openai.com/v1/chat/completions", "POST")
                {
                    uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString)),
                    downloadHandler = new DownloadHandlerBuffer()
                };

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + instance.apiKey);

                UnityWebRequestAsyncOperation sending = request.SendWebRequest();
                while (!sending.isDone)
                {
                    flags |= Flags.Status;
                    status = $"{sending.GetType().FullName}... {Mathf.RoundToInt(100 * sending.progress)}%";
                    yield return null;
                }
                flags &= ~Flags.Status;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseText = request.downloadHandler.text;
                    OpenAIResponse openAIResponse = JsonUtility.FromJson<OpenAIResponse>(responseText);

                    if (openAIResponse != null)
                    {
                        OpenAIResponse.Choice choice = openAIResponse.choices[0];
                        Debug.Log(choice.message.content);
                        openAIRequest.messages.Add(choice.message);
                        flags |= Flags.Stdin | Flags.Killable;
                    }
                    else
                    {
                        Debug.LogWarning("Erreur de désérialisation de la réponse de l'IA:");
                        Debug.Log(responseText);
                        OnFailure();
                    }
                }
                else
                {
                    Debug.LogWarning($"Error: \"{request.error}\"");
                    OnFailure();
                }
            }
        }
    }
}