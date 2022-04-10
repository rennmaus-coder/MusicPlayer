#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"


using MusicPlayer.Util;
using System;
using System.IO;

namespace MusicPlayer.Core.Songs
{
    internal class SongImporter
    {
        public SongImporter()
        {
        }

        public Song ImportFromPath(string path)
        {
            string extension = path.ToLower().Split('.')[path.ToLower().Split('.').Length - 1];
            
            TagLib.File tags = TagLib.File.Create(path);
            FileTypes FileType;
            Genre genre;
            string title;
            string songwriter;
            string album;
            int bitrate;
            TimeSpan duration;

            switch (extension)
            {
                case "aac": 
                    FileType = FileTypes.AAC;
                    break;
                case "alac":
                    FileType = FileTypes.ALAC;
                    break;
                case "flac":
                    FileType = FileTypes.FLAC;
                    break;
                case "mp3":
                    FileType = FileTypes.MP3;
                    break;
                case "ogg":
                    FileType = FileTypes.OGG;
                    break;
                case "wav":
                    FileType = FileTypes.WAV;
                    break;
                case "wma":
                    FileType = FileTypes.WMA;
                    break;
                default:
                    throw new ArgumentException($"Filetype not supported: {path}");

            }

            if (tags.PossiblyCorrupt)
            {
                Logger.Warn($"File may be corrupt, please check: {path}\nReasons: {string.Join("\n", tags.CorruptionReasons)}");
            }
            title = tags.Tag.Title;
            songwriter = tags.Tag.FirstPerformer;
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

        public Song ImportFromInfo(SongInfo info)
        {
            return new Song(info);
        }

        private Genre MatchStringToGenre(string genre)
        {
            genre = genre.ToUpper();
            
            switch (genre) 
            {
                case "BLUES":
                    return Genre.BLUES;
                case "CLASSIC":
                    return Genre.CLASSIC;
                case "COUNTRY":
                    return Genre.COUNTRY;
                case "FOLK":
                    return Genre.FOLK;
                case "HIPHOP":
                    return Genre.HIPHOP;
                case "JAZZ":
                    return Genre.JAZZ;
                case "METAL":
                    return Genre.METAL;
                case "POP":
                    return Genre.POP;
                default:
                    return Genre.OTHER;
            }
                
        }
    }
}
