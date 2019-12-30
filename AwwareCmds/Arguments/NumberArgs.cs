using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AwwareCmds.Arguments
{
    public class NumberArgs : ArgsTemplate
    {
        public Regex NumberArgument;
        public List<int> IntArgs;
        public NumberArgs(ArgsController controller) : base(controller)
        {
            NumberArgument = new Regex("(-?\\d+(?:\\.\\d+)?)|(\\\"-?\\d+(?:\\.\\d+)?\\\")");
        }

        public override void Handle() => IntArgs = GetAllNumbersArguments();

        public int this[int index]
        {
            get
            {
                return (!HasNumbers() && (IntArgs.Count - 1) < index) ? 0 : IntArgs[index];
            }
        }

        public bool HasNumbers() => IntArgs.Count > 0;

        public int GetNumberArgument(int index = 0)
        {
            try
            {
                string value = NumberArgument.Match(CONTROLLER.ROWArguments).Groups[index].Value;
                if (value.Contains("\""))
                    throw new Exception("Unknown symbol '\"'");
                return Int32.Parse(value);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n" + ex.StackTrace); return -1; }
        }
        //Retrieving int argument by matches index | no groups
        public int GetNumberArguments(int index = 0)
        {
            try
            {
                MatchCollection matches = NumberArgument.Matches(CONTROLLER.ROWArguments);
                string value = matches[index].Groups[0].Value;
                if (value.Contains("\""))
                    throw new Exception("Unknown symbol '\"'");
                return int.Parse(value);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n" + ex.StackTrace); return -1; }
        }
        //Retrieving all numeric arguments
        public List<int> GetAllNumbersArguments()
        {
            try
            {
                MatchCollection matches = NumberArgument.Matches(CONTROLLER.ROWArguments);
                List<int> array = new List<int>();

                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].Groups[0].Value.Contains("\""))
                        //throw new NumberArgumentException("Unknown symbol '\"'");
                        continue;
                    array.Add(int.Parse(matches[i].Groups[0].Value));
                }

                return array;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n" + ex.StackTrace); return null; }
        }
    }
}
