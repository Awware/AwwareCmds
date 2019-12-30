using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwwareCmds
{
    public static class AEvents
    {
        public enum OutTypes
        {
            Info,
            Debug,
            Error,
            Warning,
            Success,
            Custom
        }
        #region Events
        //Events
        public static event Action OnCommandEnded;

        public static event Action<Task> OnCommandSleep;

        public static event Action CommandBeforeStart;

        public static event Action<string, object[]> Output;
        public static event Action<string> OutputError;
        public static event Action<string> OutputDebug;
        public static event Action<string> OutputWarning;
        public static event Action<string> OutputSuccess;
        public static event Action<string> OutputInfo;
        public static event Action ExitApplication;
        public static event Action ClearPlace;
        public static void InvokeAction(string @event, params object[] args)
        {
            switch (@event.ToLower())
            {
                case "clear":
                    ClearPlace?.Invoke();
                    break;
                case "exit":
                    ExitApplication?.Invoke();
                    break;
                case "ended":
                    OnCommandEnded?.Invoke();
                    break;
                case "sleep":
                    OnCommandSleep?.Invoke((Task)args[0]);
                    break;
                case "before":
                    CommandBeforeStart?.Invoke();
                    break;
                default:
                    throw new Exception("Unknown action!");
            }
        }
        //End events
        #endregion
        public static void OutputAction(string msg, OutTypes type, params object[] args)
        {
            switch (type)
            {
                case OutTypes.Info:
                    OutputInfo?.Invoke(msg);
                    break;
                case OutTypes.Error:
                    OutputError?.Invoke(msg);
                    break;
                case OutTypes.Warning:
                    OutputWarning?.Invoke(msg);
                    break;
                case OutTypes.Success:
                    OutputSuccess?.Invoke(msg);
                    break;
                case OutTypes.Debug:
                    OutputDebug?.Invoke(msg);
                    break;
                default:
                    Output?.Invoke(msg, args);
                    break;
            }
        }
    }
}
