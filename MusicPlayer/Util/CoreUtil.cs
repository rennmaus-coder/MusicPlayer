#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MusicPlayer.Util
{
    public static class CoreUtil
    {
        public static DateTime APPLICATIONSTART = DateTime.Now;
        public static string APPLICATIONNAME = "MusicPlayer";
        public static string APPLICATIONVERSION = "0.0.1";
        public static string APPDATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APPLICATIONNAME);
        public static async Task WaitForMouseUp()
        {
            while (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                await Task.Delay(10);
            }
        }

        public static List<string> FileDialog()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Audio Files(*.wav;*.mp3;*.ogg;*.flac;*.wma;*.aiff;*.m4a)|*.wav;*.mp3;*.ogg;*.flac;*.wma;*.aiff;*.m4a";
                dialog.Title = "Select Songs";
                dialog.Multiselect = true;
                DialogResult res = dialog.ShowDialog();
                if (res == DialogResult.OK)
                {
                    return dialog.FileNames.ToList();
                }
                return new List<string>();
            }
        }
    }
}
