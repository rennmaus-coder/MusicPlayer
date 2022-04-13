#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core;
using MusicPlayer.Core.Songs;
using MusicPlayer.Core.Songs.Interfaces;
using MusicPlayer.MVVM.ViewModel.Interfaces;
using MusicPlayer.Util;
using System.Collections.Generic;

namespace MusicPlayer.MVVM.ViewModel
{
    public class HomeVM : ObservableObject, IHomeVM
    {
        private List<Song> _songs = MainViewModel.instance.ProfileService.ActiveProfile.Songs;
        public List<Song> Songs
        {
            get => _songs;
            private set
            {
                _songs = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ImportSongs { get; set; }

        public HomeVM()
        {            
            ImportSongs = new RelayCommand(_ =>
            {
                List<Song> temp = Songs;
                foreach (string file in CoreUtil.FileDialog())
                {
                    Song song = SongImporter.ImportFromPath(file);
                    if (song != null)
                    {
                        temp.Add(song);
                        MainViewModel.instance.ProfileService.ActiveProfile.Songs.Add(song);
                    }
                }
                Songs = temp;
            });
        }

        public List<ISong> GetSongs()
        {
            List<ISong> result = new List<ISong>();
            result.AddRange(Songs);
            return result;
        }
    }
}
