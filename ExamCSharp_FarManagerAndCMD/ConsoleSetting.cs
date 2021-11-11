using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCSharp_FarManagerAndCMD
{
    class ConsoleSettings
    {
        private const int width = 211, height = 50;
        public void DrawAll()
        {
            Console.SetBufferSize(width, height);
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(width, height);
            Console.CursorSize = 100;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            char symbol1 = '-';
            char symbol2 = '|';
            int size1 = width - 4, size2 = height - 2;
            for (int i = 1; i < size1; i++)
            {
                Console.SetCursorPosition(i, 1);
                Console.Write(symbol1);
                Console.SetCursorPosition(i, 2);
                Console.Write(symbol1);
                Console.SetCursorPosition(i, size2 - 1);
                Console.Write(symbol1);
                Console.SetCursorPosition(i, size2);
                Console.Write(symbol1);
            }
            for (int i = 1; i < size2; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(symbol2);
                Console.SetCursorPosition(2, i);
                Console.Write(symbol2);
                Console.SetCursorPosition(size1 / 2 + 2, i);
                Console.Write(symbol2);
                Console.SetCursorPosition(size1 / 2 + 3, i);
                Console.Write(symbol2);
                Console.SetCursorPosition(size1 - 1, i);
                Console.Write(symbol2);
                Console.SetCursorPosition(size1, i);
                Console.Write(symbol2);
            }
        }
        public void DrawKeys()
        {
            const int size = 4, step = 16, pos = 45;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(size, pos);

            string str = "0.Help";
            Console.Write($"{ str,step }");

            str = "1.Copy";
            Console.Write($"{ str,step }");

            str = "2.MoveFile";
            Console.Write($"{ str,step }");

            str = "3.MakeFile";
            Console.Write($"{ str,step }");

            str = "4.RmFile";
            Console.Write($"{ str,step }");

            str = "5.MakeDir";
            Console.Write($"{ str,step }");

            str = "               6.RmDir";
            Console.Write($"{ str,step }");

            str = "7.Rename";
            Console.Write($"{ str,step }");

            str = "8.Attrib";
            Console.Write($"{ str,step }");

            str = "9.CMD";
            Console.Write($"{ str,step }");

            str = "Q.Exit";
            Console.Write($"{ str,step }");

            str = "";
            Console.Write($"{ str,step }");

            Console.BackgroundColor = ConsoleColor.DarkBlue;

        }
    }
}
