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
        public class Models
        {
            [Serializable]
            public class Model
            {
                public string id, owned_by;
            }

            public Model[] data;
        }

        //--------------------------------------------------------------------------------------------------------------

        IEnumerator EListModels()
        {
            using UnityWebRequest request = new("https://api.openai.com/v1/models", "GET")
            {
                downloadHandler = new DownloadHandlerBuffer(),
            };

            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            UnityWebRequestAsyncOperation sending = request.SendWebRequest();
            while (!sending.isDone)
                yield return null;

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogWarning(request.error);
            else
            {
                Models models = JsonUtility.FromJson<Models>(request.downloadHandler.text);
                if (models.data.Length > 0)
                {
                    StringBuilder log = new();
                    for (int i = 0; i < models.data.Length; i++)
                    {
                        Models.Model model = models.data[i];
                        log.AppendLine($"{model.id} ({model.owned_by})");
                    }
                    Debug.Log(log.ToString()[..^1], this);
                }
            }
        }
    }
}