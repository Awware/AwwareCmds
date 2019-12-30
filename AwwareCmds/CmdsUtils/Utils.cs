using System;
using System.IO;
using System.Linq;

namespace AwwareCmds
{
    public static class Utils
    {
        public static void OverrideDirectory(string path)
        {
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
        public static void PredictCreateFile(string path, string content = "")
        {
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                OverrideDirectory(dir);
            File.WriteAllText(path, content);
        }
        public static bool PathValidation(string path)
        {
            if (Path.GetExtension(path) == "" && !File.Exists(path))
                return Directory.Exists(path);
            else
                return File.Exists(path);
        }
        public static bool AdvExists(string path)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    return false;
                if (!File.Exists(path))
                    return false;
                return true;
            }
            catch { return false; }
        }
        private static readonly Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnoprstquvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,@#$%^&*!";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
