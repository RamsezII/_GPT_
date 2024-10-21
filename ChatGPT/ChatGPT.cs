using _TERMINAL_;
using UnityEngine;

namespace _GPT_
{
    public partial class ChatGPT : MonoBehaviour
    {
        public static ChatGPT instance;

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
            Commands.AddCommandKeys(OnCmd, "GPT", "ChatGPT");
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (this == instance)
                instance = null;
        }
    }
}