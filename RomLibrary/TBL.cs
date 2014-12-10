using RomLibrary.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RomLibrary
{
    public class TBL : Dictionary<string, string>
    {
        private const string EUC_KR = "euc-kr";

        public TBL() { }

        public TBL(string file)
            : this()
        {
            LoadFile(file);
        }

        private void LoadFile(string file)
        {
            using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding(EUC_KR)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Pass if line is unformal.
                    try
                    {
                        string[] temp = line.Split('=');
                        this.Add(temp.First(), temp.Last());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                reader.Close();
            }
        }

        public void WriteFile(string file)
        {
            using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(EUC_KR)))
            {
                this.ToList()
                    .ForEach(dic => writer.WriteLine(string.Format("{0}={1}", dic.Key, dic.Value)));

                writer.Close();
                stream.Close();
            }
        }

        public string ApplyTBL(byte[] bytes)
        {
            return string.Concat(bytes.AsHex()
                .ToList()
                .Select(hex => this.ContainsKey(hex)
                    ? this[hex]
                    : hex));
        }

        [Obsolete]
        public string ApplyTBL(string hex)
        {
            string applied = string.Empty;
            this.OrderByDescending(dic => dic.Key.Length)
                .ToList()
                .ForEach(dic => applied = hex.Replace(dic.Key, dic.Value));

            return applied;
        }
    }
}
