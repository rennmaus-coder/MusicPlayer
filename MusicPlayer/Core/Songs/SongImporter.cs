#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"


using MusicPlayer.MVVM.ViewModel;
using MusicPlayer.Util;
using System;
using System.IO;

namespace MusicPlayer.Core.Songs
{
    public static class SongImporter
    {

        public static Song ImportFromPath(string path)
        {
            foreach (Song s in MainViewModel.instance.ProfileService.ActiveProfile.Songs)
            {
                if (s.Info.FilePath == path)
                {
                    Logger.Warn($"File with same path already exists in profile: {path}");
                    return null;
                }
            }
            
            string extension = path.ToLower().Split('.')[path.ToLower().Split('.').Length - 1];
            
            TagLib.File tags = TagLib.File.Create(path);
            Genre genre;
            string title;
            string songwriter;
            string album;
            int bitrate;
            TimeSpan duration;
            FileTypes FileType = extension switch
            {
                "aiff" => FileTypes.AIFF,
                "flac" => FileTypes.FLAC,
                "mp3" => FileTypes.MP3,
                "ogg" => FileTypes.OGG,
                "wav" => FileTypes.WAV,
                "wma" => FileTypes.WMA,
                "m4a" => FileTypes.M4A,
                _ => throw new ArgumentException($"Filetype not supported: {path}"),
            };
            if (tags.PossiblyCorrupt)
            {
                Logger.Warn($"File may be corrupt, please check: {path}\nReasons: {string.Join("\n", tags.CorruptionReasons)}");
            }
            title = tags.Tag.Title;
            songwriter = tags.Tag?.AlbumArtists[0];
            album = tags.Tag.Album;
            duration = tags.Properties.Duration;
            bitrate = tags.Properties.AudioBitrate;
            genre = MatchStringToGenre(tags.Tag.FirstGenre);

            if (string.IsNullOrEmpty(title))
            {
                title = Path.GetFileNameWithoutExtension(path);
            }

            SongInfo songInfo = new SongInfo()
            {
                Album = album,
                Bitrate = bitrate,
                Duration = duration,
                FileType = FileType,
                FilePath = path,
                Genre = genre,
                Rating = 0,
                Songwriter = songwriter,
                Title = title
            };

            return new Song(songInfo);
        }

        public static Song ImportFromInfo(SongInfo info)
        {
            return new Song(info);
        }

        private static Genre MatchStringToGenre(string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return Genre.OTHER;
            }
            genre = genre.ToUpper();

            return genre switch
            {
                "BLUES" => Genre.BLUES,
                "CLASSIC" => Genre.CLASSIC,
                "COUNTRY" => Genre.COUNTRY,
                "FOLK" => Genre.FOLK,
                "HIPHOP" => Genre.HIPHOP,
                "JAZZ" => Genre.JAZZ,
                "METAL" => Genre.METAL,
                "POP" => Genre.POP,
                _ => Genre.OTHER,
            };
        }
    }
}
