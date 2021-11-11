using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace ExamCSharp_FarManagerAndCMD
{
    class CmdMenu
    {
        private readonly Cmd cmd = new Cmd();
        public void MainMenu()
        {
            const int width = 100, height = 50, maxPathSize = 3;
            string[] splitPath = new string[maxPathSize];
            Cmd.Menu["cls"].Invoke(splitPath);
            while (true)
            {
                try
                {
                    Console.Title = "CMD";
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetWindowSize(width, height);
                    Console.SetWindowPosition(0, 0);
                    Console.CursorSize = 100;

                    cmd.GetCurrentDirectory();
                    string path = Console.ReadLine();
                    Cmd.Commands.Add(path);
                    splitPath = new string[maxPathSize];
                    if (path.Contains('"'))
                    {
                        splitPath[0] = Regex.Replace(path.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                        string path2 = path.Substring(path.IndexOf(' '));
                        string[] newpath = path2.Split('"');
                        for (int i = 0, j = 0; i < newpath.Length; i++)
                        {
                            newpath[i] = newpath[i].Trim('"');
                            if (newpath[i] != " " && newpath[i] != "")
                            {
                                j++;
                                splitPath[j] = newpath[i];
                            }
                        }
                    }
                    else splitPath = path.Split(' ');

                    if (splitPath[0] == "exit") break;
                    Cmd.Menu[splitPath[0]](splitPath);
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

    }
}
