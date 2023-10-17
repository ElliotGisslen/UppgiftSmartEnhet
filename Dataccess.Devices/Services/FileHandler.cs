﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Devices.Services
{
    public class FileHandler
    {
        public static void SaveToFile(string path, string content)
        {
            using var sw = new StreamWriter(path);
            sw.WriteLine(content);
        }

        public static string ReadFromFile(string path)
        {
            if(File.Exists(path))
            {
                using var reader = new StreamReader(path);
                return reader.ReadToEnd();
            }

            return null!;
        }
    }
}
