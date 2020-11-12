using System.Threading;

namespace AwwareCmds.Types
{
    public interface Interactor
    {
        void Debug(string msg);
        void Error(string msg);
        void Info(string msg);
        void Warning(string msg);
        void Success(string msg);
        //Clear command input place
        void Clear();
        //Before command execution
        void Before();
        //After command execution
        void After(CommandResult result);
        //If command timeout.
        void Sleep(CancellationTokenSource cancelation);
    }
}
