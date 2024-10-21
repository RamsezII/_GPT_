using _TERMINAL_;
using UnityEngine;

namespace _GPT_
{
    public partial class ChatGPT : MonoBehaviour
    {
        public static ChatGPT instance;

        static readonly string[] cmdKeys = new string[]
        {
            "GPT",
            "ChatGPT",
        };

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            Util.InstantiateOrCreate<ChatGPT>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            TryReadKey();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            Commands.AddCommandKeys(OnCmd, cmdKeys);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            Commands.RemoveCommand(OnCmd);
            if (this == instance)
                instance = null;
        }
    }
}