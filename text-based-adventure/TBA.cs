using System;
using System.Collections.Generic;
using System.Threading;

namespace TBA {
    class Selector {
        int cursorPos = 0;
        List<string> options;

        public Selector() {
            options = new List<string>();
        }

        public void Clear() {
            options = new List<string>();
        }

        public void Add(string item) {
            if(!options.Contains(item)) {
                options.Add(item);
            }
        }

        public void AddRange(string[] items) {
            options.AddRange(items);
        }

        private void moveCursorPos(int amount, int min, int max) {
            cursorPos = cursorPos + amount;
            if(cursorPos >= max) {
                cursorPos = max - 1;
            }
            if(cursorPos < min) {
                cursorPos = min;
            }
        }

        void drawAtPos(string text, int pos) {
            Console.SetCursorPosition(0, pos);
            Console.Write(text);
        }

        public int Run() {
            string[] selectOptions = options.ToArray();
            int intialTop = Console.CursorTop;

            string printString = "\n";
            foreach(string option in selectOptions) {
                printString += "  " + option + "\n";
            }
            Console.WriteLine(printString);
            drawAtPos(">", intialTop + cursorPos + 1);
            while (true) {
                switch(Console.ReadKey(true).Key) {
                    case ConsoleKey.DownArrow:
                        drawAtPos(" ", intialTop + cursorPos + 1);
                        moveCursorPos(1,0,selectOptions.Length);
                        drawAtPos(">", intialTop + cursorPos + 1);
                        break;
                    case ConsoleKey.UpArrow:
                        drawAtPos(" ", intialTop + cursorPos + 1);
                        moveCursorPos(-1,0,selectOptions.Length);
                        drawAtPos(">", intialTop + cursorPos + 1);
                        break;
                    case ConsoleKey.Enter:
                        int newCursor = intialTop;
                        for(int i = 0; i <= selectOptions.Length; i++) {
                            newCursor++;
                            Console.SetCursorPosition(0, newCursor);
                            Console.Write(new string(' ', Console.WindowWidth)); 
                        }
                        Console.SetCursorPosition(0, intialTop);
                        return cursorPos;
                }
            }
        }
    }

    class Stage {
        string[] printStrings;
        Selector selector;

        public Stage(string[] _printStrings, Selector _selector = null) {
            printStrings = _printStrings;
            selector = _selector;
        }

        public void slowPrint(string text) {
            int sleepDuration = 35;
            foreach(char letter in text) {
                Console.Write(letter);
                Thread.Sleep(sleepDuration);
            }
            Console.Write("\n");
            Thread.Sleep(500);
        }

        public int Run() {
            foreach(string printString in printStrings) {
                slowPrint(printString);
            }
            if(selector != null) {
                return selector.Run();
            }
            return -1;
        }
    }
}