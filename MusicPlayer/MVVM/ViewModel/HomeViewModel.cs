#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Songs;
using MusicPlayer.Util;
using System;
using System.Collections.Generic;

namespace MusicPlayer.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        private List<Song> _songs;
        public List<Song> Songs
        {
            get => _songs;
            private set
            {
                _songs = value;
                RaisePropertyChanged();
            }
        }
    }
}
