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
using System.Runtime.CompilerServices;
using System.Text;

namespace MusicPlayer.Util
{
    public static class Logger
    {
        public static string FileName { get; set; }

        static Logger()
        {
            string LogsDir = Path.Combine(CoreUtil.APPDATA, "Logs");

            if (!Directory.Exists(LogsDir))
            {
                Directory.CreateDirectory(LogsDir);
            }
            else
            {
                foreach (string file in Directory.GetFiles(LogsDir))
                {
                    if (DateTime.Now - File.GetCreationTime(file) > new TimeSpan(40, 0, 0, 0))
                    {
                        File.Delete(file);
                    }
                }
            }

            FileName = Path.Combine(LogsDir, DateTime.Now.ToString("yyy-M-HH-mm-ss") + ".log");

            File.Create(FileName).Close();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(PadBoth($"Music Player", 70, '-'));
            sb.AppendLine(PadBoth($".NET Version {Environment.Version}", 70, '-'));
            sb.AppendLine(PadBoth($"Is 64bit OS {Environment.Is64BitOperatingSystem}", 70, '-'));
            sb.AppendLine(PadBoth($"Is 64bit Process {Environment.Is64BitProcess}", 70, '-'));
            sb.AppendLine(PadBoth($"Platform {Environment.OSVersion.Platform}", 70, '-'));
            sb.AppendLine(PadBoth("", 70, '-'));
            sb.AppendLine($"TIME|PATH|CALLER MEMBER|LINE|LOG|MESSAGE");

            File.WriteAllText(FileName, sb.ToString());
        }

        public static void Info(string message, [CallerMemberName] string Caller = null, [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            path = Path.GetFileName(path);
            string log = $"[{DateTime.Now.ToString("HH:mm:ss")}]|[{path}]|[{Caller}]|[{line}]|[INFO]: {message}\n";

            File.AppendAllText(FileName, log);
        }


        public static void Warn(string message, [CallerMemberName] string Caller = null, [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            path = Path.GetFileName(path);
            string log = $"[{DateTime.Now.ToString("HH:mm:ss")}]|[{path}]|[{Caller}]|[{line}]|[WARN]: {message}\n";

            File.AppendAllText(FileName, log);
        }


        public static void Error(string message, [CallerMemberName] string Caller = null, [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            path = Path.GetFileName(path);
            string log = $"[{DateTime.Now.ToString("HH:mm:ss")}]|[{path}]|[{Caller}]|[{line}]|[ERROR]: {message}\n";

            File.AppendAllText(FileName, log);
        }

        public static void Error(string message, Exception e, [CallerMemberName] string Caller = null, [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            path = Path.GetFileName(path);
            string log = $"[{DateTime.Now.ToString("HH:mm:ss")}]|[{path}]|[{Caller}]|[{line}]|[ERROR]: {message}\n{e.StackTrace}\n";

            File.AppendAllText(FileName, log);
        }

        public static void Error(Exception e, [CallerMemberName] string Caller = null, [CallerFilePath] string path = null, [CallerLineNumber] int line = 0)
        {
            path = Path.GetFileName(path);
            string log = $"[{DateTime.Now.ToString("HH:mm:ss")}]|[{path}]|[{Caller}]|[{line}]|[ERROR]: {e.Message}\n{e.StackTrace}\n";

            File.AppendAllText(FileName, log);
        }
        
        private static string PadBoth(string source, int length, char paddingChar)
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft, paddingChar).PadRight(length, paddingChar);
        }
    }
}
