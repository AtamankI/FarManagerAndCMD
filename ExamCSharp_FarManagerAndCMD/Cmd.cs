using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ExamCSharp_FarManagerAndCMD
{
    class Cmd
    {
        public static List<string> Commands { get; set; }
        public static Dictionary<string, Action<string[]>> Menu { get; set; }
        private static Dictionary<string, string> helpMenu = new Dictionary<string, string>
        {
            ["cd"] = "Change the current directory.\ncd drive:\\path\ncd C:\\test",
            ["dir"] = "Lists files and subdirectories in the specified directory.\ndir drive:\\path\\filename\nExample: \ndir C:\\Program Files\ndir C:\\test\\file1.txt",
            ["move"] = "Moving files and renaming files and folders.\nmove drive:\\path\\filename destination\nExample: \nmove C:\\test\\file1.txt D:\\folder2\\file2.txt",
            ["copy"] = "Copy one or more files to another location.\ncopy drive:\\path\\filename destination\nExample: \ncopy C:\\test\\file1.txt D:\\folder2\\file2.txt",
            ["nul"] = "Creating an empty file.\nnul drive:\\path\\filename\nExample: \nnul C:\\test\\file1.txt",
            ["del"] = "Deleting a file.\ndel drive:\\path\\filename\nExample: \ndel C:\\test\\file1.txt",
            ["type"] = "Output the contents of a text file.\ntype drive:\\path\\filename\nExample: \ntype C:\\test\\file1.txt",
            ["mkdir"] = "Directory creation.\nmkdir drive:\\path\nExample: \nmkdir C:\\test",
            ["rmdir"] = "Removing a directory.\nrmdir drive:\\path\nExample: \nrmdir C:\\test",
            ["where"] = "Determining the location of files in directories\nwhere drive:\\path template\nExample: \nwhere C:\\test *.txt",
            ["attrib"] = "Displays file attributes\nattrib drive:\\path\\filename \nExample: \nattrib C:\\test\\file1.txt",
            ["ren"] = "Group renaming of files.\nren drive:\\path\filename,drive:\\path\filename new_name\nren C:\\test\\file1.txt,C:\\test\\file2.docx newName",
            ["cls"] = "Clears the contents of the screen.",
            ["view"] = "View the history of entered commands.",
            ["help"] = "Displaying help information about commands\nhelp command's_name\nExample: help dir",
            ["exit"] = "To exit cmd",
            ["quit"] = "To leave program",
        };
        public Cmd()
        {
            Commands = new List<string>();
            Menu = new Dictionary<string, Action<string[]>>
            {
                ["cd"] = ChangeCurrentDirectory,
                ["dir"] = ShowDirectoriesAndFiles,
                ["move"] = MoveDirectoriesAndFiles,
                ["copy"] = CopyDirectoriesAndFiles,
                ["nul"] = CreateFiles,
                ["del"] = DeleteFiles,
                ["type"] = DisplayFileContent,
                ["mkdir"] = CreateDirectory,
                ["rmdir"] = DeleteDirectory,
                ["where"] = FindFiles,
                ["attrib"] = ShowFileAttributes,
                ["ren"] = FileGroupRename,
                ["cls"] = ClearScreen,
                ["view"] = ViewCommandsHistory,
                ["help"] = HelpMenu,
                ["quit"] = Exit
            };
        }
        private static void Exit(string[] obj)
        {
            Console.Write("EXIT\t");
            Environment.Exit(0);
            Console.ReadKey();
        }
        private static void HelpMenu(string[] path)
        {
            if (path.Length == 1)
            {
                Console.WriteLine("");
                foreach (var com in Menu.Keys)
                {
                    Console.WriteLine(com);
                }
                Console.WriteLine();
                Console.WriteLine($"For information about a specific command, type help command's_name \n(Example: help dir)");
            }
            else
            {
                if (!helpMenu.ContainsKey(path[1]))
                {
                    throw new Exception("There is no such command.");
                }
                Console.WriteLine(helpMenu[path[1]].ToString());
            }
        }
        private static void FileGroupRename(string[] path)
        {
            string[] files = path[1].Split(',');
            for (int i = 0; i < files.Length; i++)
            {
                if (File.Exists(files[i]))
                    File.Move(files[i], Path.Combine(Path.GetDirectoryName(files[i]), path[2] + (i + 1) + Path.GetExtension(files[i])));
            }
        }
        private static void ViewCommandsHistory(string[] path)
        {
            foreach (string com in Commands)
            {
                Console.WriteLine(com);
            }
        }
        private static void ShowFileAttributes(string[] path)
        {
            if (!File.Exists(path[1]))
            {
                throw new Exception("There is no such file.");
            }
            Console.WriteLine(File.GetAttributes(path[1]));
        }
        private static void FindFiles(string[] path)
        {
            if (!Directory.Exists(path[1]))
            {
                throw new Exception("There is no such dir.");
            }
            string searchPattern = path[2];
            string[] fileList = Directory.GetFiles(path[1], searchPattern, SearchOption.AllDirectories);
            foreach (string f in fileList)
            {
                Console.WriteLine(f);
            }
        }
        private static void DeleteFiles(string[] path)
        {
            if (!File.Exists(path[1]))
            {
                throw new Exception("There is no such file.");
            }
            File.Delete(path[1]);
        }
        private static void CreateFiles(string[] path)
        {
            if (File.Exists(path[1]))
            {
                throw new Exception("Such file is already exist.");
            }
            File.Create(path[1]);
        }
        private static void DisplayFileContent(string[] path)
        {
            using (StreamReader sr = new StreamReader(path[1]))
            {
                string content = sr.ReadToEnd();
                Console.WriteLine(content);
            }
        }
        private static void DeleteDirectory(string[] path)
        {
            if (!Directory.Exists(path[1]))
            {
                throw new Exception("There is no such dir.");
            }
            Directory.Delete(path[1], true);
        }
        private static void CreateDirectory(string[] path)
        {
            if (Directory.Exists(path[1]))
            {
                throw new Exception("That path exists already.");
            }
            Directory.CreateDirectory(path[1]);
        }

        private static void CopyDirectoriesAndFiles(string[] path)
        {
            if (Directory.Exists(path[1]))
            {
                string[] fileList = Directory.GetFiles(path[1]);
                foreach (string f in fileList)
                {
                    string fName = f.Substring(path[1].Length + 1);
                    File.Copy(Path.Combine(path[1], fName), Path.Combine(path[2], fName), true);
                }
            }
            else
            {
                File.Copy(Path.Combine(path[1]), Path.Combine(path[2]), true);
            }
        }
        private static void MoveDirectoriesAndFiles(string[] path)
        {
            if (Directory.Exists(path[1]))
                Directory.Move(path[1], path[2]);
            else
            {
                if (File.Exists(path[2]))
                    File.Delete(path[2]);
                File.Move(path[1], path[2]);
            }
        }
        public void GetCurrentDirectory()
        {
            string path = Directory.GetCurrentDirectory();
            Console.Write($"{path}>");
        }
        private static void ChangeCurrentDirectory(string[] path)
        {
            if (!Directory.Exists(path[1]))
            {
                throw new Exception("There is no such dir.");
            }
            Environment.CurrentDirectory = (path[1]);
        }
        private static void ShowDirectoriesAndFiles(string[] path)
        {
            if (path.Length == 1)
                ProcessDirectory(Directory.GetCurrentDirectory());
            else if (Directory.Exists(path[1]))
            {
                ProcessDirectory(path[1]);
            }
            else if (File.Exists(path[1]))
            {
                ProcessFile(path[1]);
            }
            else throw new Exception("Wrong path");
        }
        public static void ClearScreen(string[] path)
        {
            Console.Clear();
        }
        public static void ProcessDirectory(string targetDirectory)
        {
            string[] dirs = Directory.GetDirectories(targetDirectory);
            foreach (string dir in dirs)
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(dir));
            }
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);
        }
        public static void ProcessFile(string path)
        {
            Console.WriteLine(Path.GetFileName(path));
        }
    }
}
