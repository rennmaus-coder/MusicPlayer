#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.Util
{
    public static class CoreUtil
    {
        public static DateTime APPLICATIONSTART = DateTime.Now;
        public static string APPLICATIONNAME = "MusicPlayer";
        public static string APPLICATIONVERSION = "0.0.1";
        public static string APPDATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MusicPlayer");
        public static async Task WaitForMouseUp()
        {
            while (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                await Task.Delay(10);
            }
        }
    }
}
