using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwwareCmds.CommandAnalyser
{
    public class RawCommand
    {
        public string Command;
        public List<string> Subcommands;
        public List<object> Attributes;
        public static bool TryParse(string raw, out RawCommand cmd)
        {
            cmd = null;
            try
            {
                cmd = Parse(raw);
                return true;
            }
            catch { return false; }
        }
        public static RawCommand Parse(string raw)
        {
            List<object> attributes = new List<object>();
            List<string> sCmds = new List<string>();
            int spaceIndex = raw.IndexOf(' ');
            string command = raw;
            if (spaceIndex != -1)
            {
                command = raw.Substring(0, spaceIndex);
                attributes = ParseAttributes(raw.Substring(spaceIndex + 1, raw.Length - spaceIndex - 1));
                if (attributes.FindIndex(a => a.ToString().StartsWith("$SUB_")) != -1)
                {
                    sCmds = attributes.Where(a => a.ToString().StartsWith("$SUB_")).Cast<string>().Select((a) => a.Replace("$SUB_", "")).ToList();
                    attributes.RemoveRange(attributes.FindIndex(a => a.ToString().StartsWith("$SUB_")), sCmds.Count);
                }
            }
            return new RawCommand() { Command = command, Attributes = attributes, Subcommands = sCmds };
        }
        //*ждёт рефактор*
        private static List<object> ParseAttributes(string rawLineAfterCommand)
        {
            List<object> Attributes = new List<object>();
            int index = 0;
            Func<int, char> Peek = new Func<int, char>((a) => {
                if (index + a >= rawLineAfterCommand.Length)
                    return '\0';
                return rawLineAfterCommand[index + a];
            });
            Func<char> Current = new Func<char>(() => Peek(0));
            Func<char> Lookahead = new Func<char>(() => Peek(1));
            for (index = 0; index < rawLineAfterCommand.Length; index++)
            {
                switch (Current())
                {
                    case '\0':
                        break;
                    case '"':
                        index++;
                        bool escape = false;
                        StringBuilder value = new StringBuilder();
                        while (!escape)
                        {
                            switch (Current())
                            {
                                case '\\':
                                    switch (Lookahead())
                                    {
                                        case '"':
                                            value.Append(Lookahead());
                                            index += 2;
                                            break;
                                    }
                                    break;
                                case '\0':
                                case '\r':
                                case '\n':
                                    //escape = true;
                                    throw new Exception($"Unterminated string `{index}`");
                                case '"':
                                    index++;
                                    escape = true;
                                    break;
                                default:
                                    value.Append(Current());
                                    index++;
                                    break;
                            }
                        }
                        Attributes.Add(value.ToString());
                        break;
                    default:
                        if (char.IsDigit(Current()))
                        {
                            int start = index;
                            object val = null;
                            bool isDouble = false;
                            while (char.IsDigit(Current()) || Current() == '-' || Current() == '+')
                            {
                                if (Lookahead() is '.' || Lookahead() is ',')
                                {
                                    index++;
                                    isDouble = true;
                                }
                                index++;
                            }
                            string str = rawLineAfterCommand.Substring(start, index - start);
                            if (!isDouble)
                            {
                                if (int.TryParse(str, out int resultInt))
                                    val = resultInt;
                                else if (long.TryParse(str, out long resultLong))
                                    val = resultLong;
                                else
                                    throw new Exception($"Invalid number type `{str}`");
                            }
                            else
                            {
                                if (!double.TryParse(str.Replace('.', ','), out double dValue))
                                    throw new Exception($"Invalid double number `{str}`");
                                val = dValue;
                            }
                            Attributes.Add(val);
                        }
                        else if (char.IsWhiteSpace(Current()))
                        {
                            while (char.IsWhiteSpace(Current()))
                                index++;
                        }
                        else if (char.IsLetter(Current()))
                        {
                            int start = index;
                            while (char.IsLetterOrDigit(Current()))
                                index++;
                            string val = rawLineAfterCommand.Substring(start, index - start);
                            if (bool.TryParse(val, out bool bResult))
                                Attributes.Add(bResult);
                            else
                                Attributes.Add($"$SUB_{val}");
                        }
                        else
                            throw new Exception($"Unknown symbol `{Current()}`");
                        break;
                }
            }
            return Attributes;
        }
    }
}
