using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExamCSharp_FarManagerAndCMD
{
    class FarManager
    {
        public List<string> LeftSide { get; set; }
        public List<string> RightSide { get; set; }
        public string LeftDirectory { get; set; }
        public string RightDirectory { get; set; }
        public bool Side { get; set; }
        private const int width = 105, height = 47, step = 4;

        public FarManager()
        {
            LeftSide = new List<string>();
            RightSide = new List<string>();
            LeftDirectory = Directory.GetCurrentDirectory();
            RightDirectory = Directory.GetCurrentDirectory();
            Side = true;
        }
        public string GetSize(string path, bool side)
        {
            double bytes = 0;
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fiArr = { };
            try
            {
                fiArr = di.GetFiles("*" + "*", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException copyError)
            {
                if (side == true) Console.SetCursorPosition((60 - LeftDirectory.Length) / 2, height);
                else Console.SetCursorPosition((60 - LeftDirectory.Length) / 2 + width, height);
                Console.WriteLine(copyError.Message);
            }
            foreach (var s in fiArr)
            {
                bytes += s.Length;
            }
            double kBytes = Math.Round(bytes / 1024, 1);
            double mBytes = Math.Round(kBytes / 1024, 1);
            double gBytes = Math.Round(mBytes / 1024, 1);

            if (gBytes >= 1)
                return $"{gBytes} Gb";
            else if (mBytes >= 1)
                return $"{ mBytes} Mb";
            else if (kBytes >= 1)
                return $"{ kBytes} Kb";
            else return $"{ bytes} b";
        }
        public string GotoRootDirectory(string path, bool side)
        {
            string newPath = path.Substring(0, path.LastIndexOf('\\'));
            if (newPath.Contains("\\"))
            {
                return newPath;
            }
            int i = height, j = height / 2;
            if (side == false) i += width;
            int disk = 1, pos = 0;
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            while (cki.Key != ConsoleKey.Escape || cki.Key != ConsoleKey.Enter)
            {
                string[] d = new string[DriveInfo.GetDrives().Length];
                foreach (var drive in DriveInfo.GetDrives())
                {
                    Console.SetCursorPosition(i, j++);
                    d[pos++] = drive.Name;
                    Console.WriteLine(drive.Name);
                }
                j = height / 2;
                while (cki.Key != ConsoleKey.Escape || cki.Key != ConsoleKey.Enter)
                {
                    Console.SetCursorPosition(i - 1, j);
                    cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.UpArrow)
                    {
                        if (disk > 1) disk--;
                        else disk = DriveInfo.GetDrives().Length;
                        Console.SetCursorPosition(i - 1, disk + j - 1);
                    }
                    else if (cki.Key == ConsoleKey.DownArrow)
                    {
                        if (disk < DriveInfo.GetDrives().Length) disk++;
                        else disk = 1;
                        Console.SetCursorPosition(i - 1, disk + j - 1);
                    }
                    else if (cki.Key == ConsoleKey.Enter)
                    {
                        return d[disk - 1]; ;
                    }
                    Console.ReadKey();
                }
            }
            return $"{newPath}\\";
        }
        public void ProcessDirectory(string targetDirectory, bool side)
        {
            string[] dirs = Directory.GetDirectories(targetDirectory);
            int i = 0, j = step;
            if (side == false) j = width + step;
            Console.SetCursorPosition(j, i + step - 1);
            Console.WriteLine("...");
            foreach (string dir in dirs)
            {

                Console.SetCursorPosition(j, i + step);
                if (side == true) LeftSide.Add(Path.GetFileNameWithoutExtension(dir));
                else RightSide.Add(Path.GetFileNameWithoutExtension(dir));
                Console.WriteLine(Path.GetFileNameWithoutExtension(dir));
                i++;
            }
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {

                if (side == true) LeftSide.Add(Path.GetFileName(fileName));
                else RightSide.Add(Path.GetFileName(fileName));
                Console.SetCursorPosition(j, i + step);
                Console.WriteLine(Path.GetFileName(fileName));
                i++;
            }
        }

    }
}
