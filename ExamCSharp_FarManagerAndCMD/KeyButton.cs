using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExamCSharp_FarManagerAndCMD
{
    class KeyButton
    {
        public Dictionary<ConsoleKey, Action<string, string, bool, string>> Menu { get; set; }
        private static Dictionary<string, string> helpMenu = new Dictionary<string, string>
        {
            ["Help"] = "Displaying help information about commands",
            ["Copy"] = "Copy files or directories to another location",
            ["MoveFile"] = "Moving filesto another location",
            ["MakeFile"] = "Creating an empty file",
            ["RmFile"] = "Deleting a file",
            ["MakeDir"] = "Directory creation",
            ["RmDir"] = "Remove a directory",
            ["Rename"] = "Rename s file or directory",
            ["Attrib"] = "Displays file attributes",
            ["CMD"] = "Go to CMD console",
            ["Exit"] = "Exit the program",
        };
        public KeyButton()
        {
            Menu = new Dictionary<ConsoleKey, Action<string, string, bool, string>>
            {
                [ConsoleKey.D0] = HelpMenu,
                [ConsoleKey.D1] = CopyDirectoriesAndFiles,
                [ConsoleKey.D2] = MoveDirectoriesAndFiles,
                [ConsoleKey.D3] = CreateFiles,
                [ConsoleKey.D4] = DeleteFiles,
                [ConsoleKey.D5] = CreateDirectory,
                [ConsoleKey.D6] = DeleteDirectory,
                [ConsoleKey.D7] = RenameFilesAndDirectories,
                [ConsoleKey.D8] = ShowFileAttributes,
                [ConsoleKey.D9] = CMD,
                [ConsoleKey.Q] = Exit,
            };
        }
        private const int width = 105, height = 47, step = 4;
        private static void Exit(string left, string right, bool side, string file)
        {
            Console.SetCursorPosition(90, 30);
            Console.Write("EXIT\t");
            Environment.Exit(0);
        }
        private static void CMD(string left, string right, bool side, string file)
        {
            CmdMenu cmd = new CmdMenu();
            cmd.MainMenu();
        }

        private static void ShowFileAttributes(string targetDirectory, string right, bool side, string file)
        {
            string[] dirs = Directory.GetDirectories(targetDirectory);
            int i = 0, j = step;
            if (side == false) j = width + step;
            Console.SetCursorPosition(j, i + 3);
            Console.WriteLine("...");
            foreach (string dir in dirs)
            {
                Console.SetCursorPosition(j, i + 4);
                Console.WriteLine($"{Path.GetFileNameWithoutExtension(dir),-30} {File.GetAttributes(dir)}");
                i++;
            }
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                Console.SetCursorPosition(j, i + 4);
                Console.WriteLine($"{Path.GetFileName(fileName),-30} {File.GetAttributes(fileName)}");
                i++;
            }
            Console.SetCursorPosition(j, i + 4);
            Console.ReadKey();
        }

        private static void RenameFilesAndDirectories(string targetDirectory, string right, bool side, string file)
        {
            int i = 20, j = 30;
            if (side == false) j = 120;
            Console.SetCursorPosition(j++, i++);
            Console.WriteLine($"Enter The new name-> ");
            Console.SetCursorPosition(j++, i++);
            right = Console.ReadLine();
            if (Directory.Exists(file))
                Directory.Move(targetDirectory, Path.Combine(targetDirectory, right));
            else File.Move(Path.Combine(targetDirectory, file), Path.Combine(targetDirectory, right + Path.GetExtension(file)));
            Console.SetCursorPosition(j++, i++);
            Console.ReadKey();
        }

        private static void DeleteDirectory(string targetDirectory, string right, bool side, string file)
        {
            Directory.Delete(Path.Combine(targetDirectory, file), true);
        }

        private static void CreateDirectory(string targetDirectory, string right, bool side, string file)
        {
            int i = height / 2, j = height / 2;
            if (side == false) j = width + height / 2;
            Console.SetCursorPosition(j++, i++);
            Console.WriteLine($"Enter the name-> ");
            Console.SetCursorPosition(j++, i++);
            right = Console.ReadLine();
            if (!Directory.Exists(Path.Combine(targetDirectory, right)))
                Directory.CreateDirectory(Path.Combine(targetDirectory, right));
            Console.SetCursorPosition(j++, i++);
            Console.ReadKey();
        }

        private static void DeleteFiles(string targetDirectory, string right, bool side, string file)
        {
            File.Delete(Path.Combine(targetDirectory, file));
        }
        private static void CreateFiles(string targetDirectory, string right, bool side, string file)
        {
            int i = height / 2, j = height / 2;
            if (side == false) j = width + height / 2;
            Console.SetCursorPosition(j++, i++);
            Console.WriteLine($"Enter the name-> ");
            Console.SetCursorPosition(j++, i++);
            right = Console.ReadLine();
            if (!File.Exists(Path.Combine(targetDirectory, right)))
                File.Create(Path.Combine(targetDirectory, right));
            Console.SetCursorPosition(j++, i++);
            Console.ReadKey();
        }

        private static void MoveDirectoriesAndFiles(string targetDirectory, string right, bool side, string file)
        {
            if (Directory.Exists(Path.Combine(targetDirectory, file)))
            {
                Directory.Move(Path.Combine(targetDirectory, file), right);
            }
            else
            {
                if (File.Exists(Path.Combine(right, file)))
                    File.Delete(Path.Combine(right, file));
                File.Move(Path.Combine(targetDirectory, file), Path.Combine(right, file));
            }
        }
        private static void CopyDirectoriesAndFiles(string targetDirectory, string right, bool side, string file)
        {

            if (Directory.Exists(Path.Combine(targetDirectory, file)))
            {
                string[] fileList = Directory.GetFiles(Path.Combine(targetDirectory, file));
                foreach (string f in fileList)
                {
                    string fName = f.Substring(Path.Combine(targetDirectory).Length + 1);
                    File.Copy(f, Path.Combine(right, fName), true);
                }
            }
            else
            {
                File.Copy(Path.Combine(targetDirectory, file), Path.Combine(right, file), true);
            }
        }

        private static void HelpMenu(string left, string right, bool side, string file)
        {
            int i = height / 2 + step, j = height / 2;
            if (side == false) j = width + height / 2;
            foreach (var key in helpMenu)
            {
                Console.SetCursorPosition(j, i++);
                Console.WriteLine($"{key.Key} - {key.Value.ToString()}");
            }
            Console.SetCursorPosition(j, i++);
            Console.ReadKey();
        }
    }
}
