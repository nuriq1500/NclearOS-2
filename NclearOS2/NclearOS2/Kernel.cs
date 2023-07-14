﻿using Cosmos.Core;
using System;
using System.Threading;
using Display = NclearOS2.GUI.GUI;
using Sys = Cosmos.System;

namespace NclearOS2
{
    public class Kernel : Sys.Kernel
    {
        public static readonly string OSVERSION = "NclearOS 2 Version Alpha 0.4 Pre-release";
        public static readonly string OSNAME = "NclearOS 2";
        public static string PCNAME = "pc";
        public static readonly string MAINDISK = "0:\\";
        public static readonly string SYSTEMPATH = MAINDISK + "NclearOS\\";
        public static readonly string USERSPATH = MAINDISK + "Users\\";

        public static readonly string PROGRAMSPATH = SYSTEMPATH + "Programs\\";
        public static readonly string PROGRAMSDATAPATH = SYSTEMPATH + "Config\\";
        public static readonly string SYSTEMCONFIG = PROGRAMSDATAPATH + "systemConfig.cfg";

        public static readonly string CURRENTUSER = USERSPATH + "Admin\\";
        public static readonly string USERPROGRAMSPATH = CURRENTUSER + "Programs\\";
        public static readonly string USERPROGRAMSDATAPATH = CURRENTUSER + "Config\\";
        public static readonly string USERCONFIG = USERPROGRAMSDATAPATH + "userConfig.cfg";

        public static bool GUIenabled = false;
        public static bool useDisks = true;
        public static bool useNetwork = false;
        public static bool safeMode = false;

        public static int debg;
        protected override void BeforeRun()
        {
            try
            {
                Console.Clear();
                //Console.CursorVisible = false;
                Console.CursorLeft = 35;
                Console.CursorTop = 11;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("NclearOS");
                Console.CursorLeft = 0;
                Console.CursorTop = Console.WindowHeight - 1;
                Boot.Run();
            }
            catch (Exception e)
            {
                SystemCrash(e.ToString());
            }

        }
        protected override void Run()
        {
            try
            { if (GUIenabled) { Display.Refresh(); } }
            catch (Exception e)
            {
                try
                {
                    Display.Refresh();
                    if (Display.canvas != null) { Msg.Main("Error", "System " + e, GUI.Icons.error); } else { Console.WriteLine("System " + e, GUI.Icons.error); }
                }
                catch (Exception ex)
                {
                    SystemCrash(ex.ToString());
                }
            }
        }

        public static void SystemCrash(string e)
        {
            Display.canvas?.Disable();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.CursorTop = Console.WindowHeight / 3;
            Console.CursorLeft = (Console.WindowWidth - OSVERSION.Length - 4) / 2;
            Console.WriteLine("  " + OSVERSION + "  ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.CursorLeft = (Console.WindowWidth - ("Fatal System " + e).Length) / 2;
            Console.WriteLine("Fatal System " + e);
            Console.WriteLine();
            Console.CursorLeft = (Console.WindowWidth - "Press any key to restart computer ".Length) / 2;
            Console.Write("Press any key to restart computer ");
            Console.ReadKey(true);
            Sys.Power.Reboot();
            ACPI.Reboot();
        }
        public static void Shutdown(bool restart = false, bool force = false)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.CursorVisible = false;
            Console.Clear();
            Console.SetCursorPosition(18, Console.WindowHeight / 2 - 1);
            if (force)
            {
                if (restart) { Sys.Power.Reboot(); } else { Sys.Power.Shutdown(); Console.Write("It is now safe to turn off computer."); }
            }
            else
            {
                if (GUIenabled)
                {
                    Display.ShutdownGUI(restart);
                }
                Console.Beep(700, 100);
                Console.Beep(600, 100);
                Console.Beep(500, 100);
                if (restart)
                {
                    Console.Beep(400, 300);
                    if (GUIenabled) { Display.canvas.Disable(); }
                    Thread.Sleep(100);
                    Sys.Power.Reboot();
                }
                else
                {
                    Console.Beep(400, 100);
                    Console.Beep(300, 200);
                    if (GUIenabled) { Display.canvas.Disable(); }
                    Thread.Sleep(100);
                    Sys.Power.Shutdown();
                    Console.Write("It is now safe to turn off computer.");
                }
            }
            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}