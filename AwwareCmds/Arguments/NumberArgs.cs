using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AwwareCmds.Arguments
{
    public class NumberArgs : ArgsTemplate
    {
        public Regex NumberArgument;
        public List<double> IntArgs;
        public NumberArgs(ArgsController controller) : base(controller)
        {
            NumberArgument = new Regex("(-?\\d+(?:\\,\\d+)?)|(\\\"-?\\d+(?:\\,\\d+)?\\\")");
        }

        public override void Handle() => IntArgs = GetAllNumbersArguments();

        public double this[int index]
        {
            get
            {
                return (!HasNumbers() && (IntArgs.Count - 1) < index) ? 0 : IntArgs[index];
            }
        }

        public bool HasNumbers() => IntArgs.Count > 0;

        public double GetNumberArgument(int index = 0)
        {
            try
            {
                string value = NumberArgument.Match(CONTROLLER.ROWArguments).Groups[index].Value;
                if (value.Contains("\""))
                    throw new Exception("Unknown symbol '\"'");
                return double.Parse(value);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n" + ex.StackTrace); return -1; }
        }
        //Retrieving int argument by matches index | no groups
        public double GetNumberArguments(int index = 0)
        {
            try
            {
                MatchCollection matches = NumberArgument.Matches(CONTROLLER.ROWArguments);
                string value = matches[index].Groups[1].Value;
                if (value.Contains("\""))
                    throw new Exception("Unknown symbol '\"'");
                return double.Parse(value);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n" + ex.StackTrace); return -1; }
        }
        //Retrieving all numeric arguments
        public List<double> GetAllNumbersArguments()
        {
            try
            {
                MatchCollection matches = NumberArgument.Matches(CONTROLLER.ROWArguments);
                List<double> array = new List<double>();

                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].Groups[1].Value.Contains("\"") || string.IsNullOrEmpty(matches[i].Groups[1].Value))
                        continue;
                    array.Add(double.Parse(matches[i].Groups[1].Value));
                }

                return array;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n" + ex.StackTrace); return null; }
        }
    }
}
