using _ARK_;
using _TERMINAL_;
using System;
using System.Linq;
using UnityEngine;

namespace _GPT_
{
    partial class ChatGPT
    {
        enum Codes : byte
        {
            Start,
            ListModels,
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnCmd(in string arg0, in LineParser line)
        {
            string cmd = line.Read();
            if (line.IsCplThis)
                line.OnCpls(cmd, Enum.GetNames(typeof(Codes)));
            else if (Enum.TryParse(cmd, true, out Codes code))
                switch (code)
                {
                    case Codes.Start:
                        {
                            LoadSettings();
                            string role = line.Read();

                            if (line.IsCplThis)
                                line.OnCpls(role, settings.roles.Select(r => r.name));
                            else if (line.IsExec)
                                if (string.IsNullOrWhiteSpace(role))
                                    Debug.LogWarning("Role not specified");
                                else if (settings.TryGetRole(role, out var roleInfos))
                                {
                                    string prompt = line.ReadAll();
                                    print($"starting {GetType().FullName} with {nameof(role)}: \"{role}\"".ToSubLog());
                                    Terminal.instance.commands.Add(new Dialog(roleInfos.description));
                                }
                                else
                                    Debug.LogWarning($"Role not found: \"{role}\"");
                        }
                        break;

                    case Codes.ListModels:
                        if (line.IsExec)
                            NUCLEOR.instance.scheduler.AddRoutine(EListModels());
                        break;
                }
            else
                Debug.LogWarning($"Unknown command: \"{cmd}\"");
        }
    }
}