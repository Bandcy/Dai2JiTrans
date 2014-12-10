using System.IO;

namespace RomLibrary.Helpers
{
    public static class ByteHelper
    {
        public static byte[] ReadFile(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(string.Format("{0} not found.", file));

            using (FileStream stream = new FileStream(file, FileMode.Open))
            using (MemoryStream memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                byte[] bytes = memory.ToArray();

                memory.Close();
                stream.Close();

                return bytes;
            }
        }

        public static void WriteFile(string file, byte[] bytes)
        {
            if (string.IsNullOrEmpty(file) || bytes == null)
                return;

            using (FileStream stream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
        }
    }
}
