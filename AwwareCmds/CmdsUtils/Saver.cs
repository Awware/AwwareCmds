using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Soap;

namespace AwwareCmds
{
    public static class Saver
    {
        public static bool Save(Type @class, string filepath)
        {
            try
            {
                FieldInfo[] fields = @class.GetFields(BindingFlags.Static | BindingFlags.Public);
                object[,] a = new object[fields.Length, 2];
                for (int i = 0; i < fields.Length; i++)
                {
                    a[i, 0] = fields[i].Name;
                    a[i, 1] = fields[i].GetValue(null);
                }
                Utils.PredictCreateFile(filepath, "");
                Stream f = File.Open(filepath, FileMode.Open);
                SoapFormatter soap = new SoapFormatter();
                soap.Serialize(f, a);
                f.Close();
                return true;
            }
            catch { return false; }
        }

        public static bool Load(Type @class, string filepath)
        {
            try
            {
                FieldInfo[] fields = @class.GetFields(BindingFlags.Static | BindingFlags.Public);
                object[,] a;
                Stream f = File.Open(filepath, FileMode.Open);
                SoapFormatter soap = new SoapFormatter();
                a = soap.Deserialize(f) as object[,];
                f.Close();
                //if (a.GetLength(0) != fields.Length) return false;
                for (int i = 0; i < fields.Length; i++)
                        if (fields[i].Name == (a[i, 0] as string) && a[i, 1] != null)
                            fields[i].SetValue(null, a[i, 1]);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
