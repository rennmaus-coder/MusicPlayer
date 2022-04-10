#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Songs.Interfaces;
using System;

namespace MusicPlayer.Core.Songs
{
    [Serializable()]
    public class Song : ISong
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="title">The title of the song (required)</param>
        /// <param name="songwriter">The sonfwriter of the song, string.empty if unkown</param>
        /// <param name="album">The album of the song, string.empty if unkown</param>
        /// <param name="duration">The duration of the song in seconds, -1 if unkown</param>
        public Song(SongInfo info)
        {
            Info = info;
        }
        
        private SongInfo Info { get; set; }
        
        public SongInfo GetInfo()
        {
            return Info;
        }
    }
}
