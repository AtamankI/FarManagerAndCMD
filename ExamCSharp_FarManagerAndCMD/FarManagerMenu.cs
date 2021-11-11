using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExamCSharp_FarManagerAndCMD
{
    class FarManagerMenu
    {
        private readonly KeyButton keyButton = new KeyButton();
        private readonly FarManager farManager = new FarManager();
        private readonly ConsoleSettings console = new ConsoleSettings();
        private const int width = 105, height = 47, step = 3;
        private const string path = "C:\\";
        public void StartMenu()
        {
            Console.Title = "Far Manager";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            while (cki.Key != ConsoleKey.Escape)
            {
                Console.Clear();
                try
                {
                    console.DrawAll();
                    farManager.LeftSide.Clear();
                    farManager.RightSide.Clear();
                    string leftDir = farManager.LeftDirectory;
                    string rightDir = farManager.RightDirectory;
                    if (!Directory.Exists(leftDir))
                    {
                        farManager.LeftDirectory = path;
                        leftDir = farManager.LeftDirectory;
                    }
                    if (!Directory.Exists(rightDir))
                    {
                        farManager.LeftDirectory = farManager.RightDirectory = path;
                        rightDir = farManager.RightDirectory;
                    }
                    PrintCurrentPathAndFilesSize(leftDir, rightDir);
                    if (farManager.Side == true)
                    {
                        LeftSelectedMenu(leftDir, rightDir);
                    }
                    else
                    {
                        RightSelectedMenu(leftDir, rightDir);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void PrintCurrentPathAndFilesSize(string leftDir, string rightDir)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition((width - leftDir.Length) / 2, 1);
            Console.Write($"{leftDir}>");
            farManager.ProcessDirectory(leftDir, true);
            Console.SetCursorPosition(width / 2, height);
            Console.Write(farManager.GetSize(farManager.LeftDirectory, true));

            Console.SetCursorPosition((width - rightDir.Length) / 2 + width, 1);
            Console.Write($"{rightDir}>");
            farManager.ProcessDirectory(rightDir, false);
            Console.SetCursorPosition(width / 2 + width, height);
            Console.Write(farManager.GetSize(farManager.RightDirectory, false));
            console.DrawKeys();
        }

        private void LeftReadFile(int size)
        {
            string path = Path.Combine(farManager.LeftDirectory + "\\" + farManager.LeftSide[size - 1].ToString());
            if (Directory.Exists(path))
            {
                farManager.LeftDirectory = path;
            }
            else
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int pos = farManager.LeftSide.Count() + step * 2;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        Console.SetCursorPosition(step + 1, pos++);
                        Console.WriteLine(line);
                    }
                    Console.SetCursorPosition(step + 1, pos++);
                    Console.ReadKey();
                }
            }
        }
        private void RightReadFile(int size)
        {
            string path = Path.Combine(farManager.RightDirectory + "\\" + farManager.RightSide[size - 1].ToString());
            if (Directory.Exists(path))
            {
                farManager.RightDirectory = path;
            }
            else
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int pos = farManager.RightSide.Count() + step * 2;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        Console.SetCursorPosition(width + 4, pos++);
                        Console.WriteLine(line);
                    }
                    Console.SetCursorPosition(width + 4, pos++);
                    Console.ReadKey();
                }
            }
        }
        public bool LeftSelectedMenu(string leftDir, string rightDir)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            int leftMenu = 0;
            Console.SetCursorPosition(step, leftMenu + step);
            while (cki.Key != ConsoleKey.Escape)
            {
                Console.SetCursorPosition(step, leftMenu + step);
                cki = Console.ReadKey();
                if (keyButton.Menu.ContainsKey(cki.Key))
                {
                    keyButton.Menu[cki.Key].Invoke(leftDir, rightDir, farManager.Side, farManager.LeftSide[leftMenu - 1]);
                    continue;
                }
                else if (cki.Key == ConsoleKey.UpArrow)
                {
                    if (leftMenu > 0) leftMenu--;
                    else leftMenu = farManager.LeftSide.Count;
                    Console.SetCursorPosition(step, leftMenu + step);
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    if (leftMenu < farManager.LeftSide.Count) leftMenu++;
                    else leftMenu = 0;
                    Console.SetCursorPosition(step, leftMenu + step);
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    if (leftMenu == 0)
                    {
                        farManager.LeftDirectory = farManager.GotoRootDirectory(leftDir, true);
                    }
                    else
                    {
                        LeftReadFile(leftMenu);
                    }
                    return farManager.Side = true;
                }
                else if (cki.Key == ConsoleKey.RightArrow)
                {
                    return farManager.Side = false;
                }
            }
            return farManager.Side = true;
        }
        public bool RightSelectedMenu(string leftDir, string rightDir)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            int rightMenu = 0;
            while (cki.Key != ConsoleKey.Escape)
            {
                Console.SetCursorPosition(width + step, rightMenu + step);
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Enter)
                {
                    if (rightMenu == 0)
                    {
                        farManager.RightDirectory = farManager.GotoRootDirectory(rightDir, false);
                    }
                    else
                    {
                        RightReadFile(rightMenu);
                    }
                    return farManager.Side = false;
                }
                if (keyButton.Menu.ContainsKey(cki.Key))
                {
                    keyButton.Menu[cki.Key].Invoke(rightDir, leftDir, farManager.Side, farManager.RightSide[rightMenu - 1]);
                    continue;
                }
                else if (cki.Key == ConsoleKey.UpArrow)
                {
                    if (rightMenu > 0) rightMenu--;
                    else rightMenu = farManager.RightSide.Count;
                    Console.SetCursorPosition(width + step, rightMenu + step);
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    if (rightMenu < farManager.RightSide.Count) rightMenu++;
                    else rightMenu = 0;
                    Console.SetCursorPosition(width + step, rightMenu + step);
                }
                else if (cki.Key == ConsoleKey.LeftArrow)
                {
                    return farManager.Side = true;
                }
            }
            return farManager.Side = false;
        }
    }
}
