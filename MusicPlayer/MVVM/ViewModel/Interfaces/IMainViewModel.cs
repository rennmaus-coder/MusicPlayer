#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Playback.Queue.Interfaces;
using MusicPlayer.Core.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.MVVM.ViewModel.Interfaces
{
    public interface IMainViewModel
    {
        HomeViewModel HomeViewModel { get; }
        IQueueMediator Queue { get; }
        string TrackTitle { get; set; }
    }
}
