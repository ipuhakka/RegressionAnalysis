using System;
using System.IO;
using System.Linq;

namespace Linear_Console
{
    class ReadFile
    {
        /// <summary>
        /// Reads a csv-file, and sets variables based on the first row of the file.
        /// </summary>
        public static void Read(string filepath)
        {           
            Console.WriteLine(filepath);
            if (Path.GetExtension(filepath) != ".csv")
            {
                Write.Error("File needs to be in csv-format");
                return;
            } 
            string[] text;

            try
            {
                text = File.ReadAllLines(filepath);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console_Main.variables = null;
                Console_Main.filepath = null;
                return;
            }

            var names = text[0].Split(',');

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = names[i].Replace("\"", "").Trim();
            }

            Console_Main.variables = names.ToList();
            Console_Main.filepath = filepath;

            Write.Success("File read\n\n");
            return;
        }

    }
}
