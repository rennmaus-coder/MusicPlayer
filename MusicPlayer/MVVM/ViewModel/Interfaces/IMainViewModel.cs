﻿#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Playback.Queue.Interfaces;
using MusicPlayer.Core.Profile;

namespace MusicPlayer.MVVM.ViewModel.Interfaces
{
    public interface IMainViewModel
    {
        IHomeVM HomeVM { get; }
        IOptionsVM OptionsVM { get; }
        IQueueMediator Queue { get; }
        ProfileService ProfileService { get; }
        string TrackTitle { get; set; }

        void TogglePlay();
        void PlayNext();
        void PlayPrevious();
    }
}
