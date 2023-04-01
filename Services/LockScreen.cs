﻿using Cosmos.System;
using System.Drawing;

namespace NclearOS2
{
    public class LockScreen
    {
        private static bool Menu;
        public static void Update()
        {
            if (KeyboardManager.TryReadKey(out KeyEvent keyEvent))
            {
                if (keyEvent.Key == ConsoleKeyEx.Delete && KeyboardManager.ControlPressed && KeyboardManager.AltPressed) { Cosmos.System.Power.Reboot(); }
                Kernel.userInactivity = false; Kernel.userInactivityTime = -1;
            }
            if (Kernel.Pressed)
            {
                if (Menu)
                {
                    if (MouseManager.X < 111)
                    {
                        if (MouseManager.Y < (int)Kernel.screenY - 70 && MouseManager.Y > (int)Kernel.screenY - 100)
                        {
                            Kernel.ShutdownPC(true);
                        }
                        else if (MouseManager.Y < (int)Kernel.screenY - 40 && MouseManager.Y > (int)Kernel.screenY - 70)
                        {
                            Kernel.ShutdownPC(false);
                        }
                    }
                    Menu = false;
                }
                else
                {
                    if (MouseManager.Y > (int)Kernel.screenY - 40 && MouseManager.X < 40)
                    {
                        Menu = !Menu;
                    }
                    else
                    {
                        Kernel.Lock = false;
                        Menu = false;
                    }
                }
            }
            if (Kernel.safeMode) { Kernel.canvas.Clear(Color.CadetBlue); } else { Kernel.canvas.DrawImage(Resources.wallpaperlock, 0, 0); }
            Kernel.canvas.DrawImageAlpha(Resources.shutdown, 10, (int)Kernel.screenY - 35);
            Kernel.canvas.DrawString(Date.CurrentTime(false), Kernel.font, Kernel.WhitePen, (int)Kernel.screenX / 2 - 30, (int)Kernel.screenY / 3 - 80);
            Kernel.canvas.DrawString(Date.CurrentDate(true, false), Kernel.font, Kernel.WhitePen, (int)Kernel.screenX / 2 - 80, (int)Kernel.screenY / 3 - 60);
            if (Menu)
            {
                Kernel.canvas.DrawImageAlpha(Resources.reboot, 10, (int)Kernel.screenY - 95);
                Kernel.canvas.DrawString("Restart", Kernel.font, Kernel.WhitePen, 40, (int)Kernel.screenY - 90);

                Kernel.canvas.DrawImageAlpha(Resources.shutdown, 10, (int)Kernel.screenY - 65);
                Kernel.canvas.DrawString("Shutdown", Kernel.font, Kernel.WhitePen, 40, (int)Kernel.screenY - 60);
            }
        }
    }
}