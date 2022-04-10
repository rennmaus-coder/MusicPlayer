#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"


using System;

namespace MusicPlayer.Core.Songs
{
    [Serializable()]
    public class SongInfo
    {
        public string Title { get; set; }
        public string Songwriter { get; set; }
        public string Album { get; set; }
        public string FilePath { get; set; }
        public TimeSpan Duration { get; set; }
        public int Bitrate { get; set; }
        public int Rating { get; set; }
        public Genre Genre { get; set; }
        public FileTypes FileType { get; set; }

        public SongInfo()
        {
        }
    }
}
