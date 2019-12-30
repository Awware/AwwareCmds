using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AwwareCmds.Arguments
{
    public class StringArgs : ArgsTemplate
    {
        public Regex StringArgument;
        public List<string> Args;
        public StringArgs(ArgsController controller) : base(controller)
        {
            StringArgument = new Regex("\".+?\"+");
        }

        public override void Handle() => Args = GetAllArguments();
        
        public string this[int index]
        {
            get
            {
                return ((Args.Count - 1) < index) ? "" : Args[index];
            }
        }

        private List<string> GetAllArguments()
        {
            try
            {
                MatchCollection matches = StringArgument.Matches(CONTROLLER.ROWArguments);
                List<string> array = new List<string>();

                for (int i = 0; i < matches.Count; i++)
                    array.Add(matches[i].Groups[0].Value.Replace("\"", ""));

                return array;
            }
            catch { return null; }
        }
        //Retrieving string argument by group index | no matches
        public string GetArgument(int index = 0)
        {
            try
            {
                return StringArgument.Match(CONTROLLER.ROWArguments).Groups[index].Value.Replace("\"", "");
            }
            catch { return ""; }
        }
        //Retrieving string argument by matches index | no groups
        public string GetArguments(int index = 0)
        {
            try
            {
                MatchCollection matches = StringArgument.Matches(CONTROLLER.ROWArguments);
                return matches[index].Groups[0].Value.Replace("\"", "");
            }
            catch { return ""; }
        }
    }
}
