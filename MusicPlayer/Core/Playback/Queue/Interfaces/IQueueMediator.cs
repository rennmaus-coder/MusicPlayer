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

namespace MusicPlayer.Core.Playback.Queue.Interfaces
{
    public interface IQueueMediator
    {
        void PlayNext();
        void PlayPrevious();
        void StartAt(int index);
        void Pause();
        void Stop();
        void UpdateProgress(TimeSpan progress);
        void SetProgress(TimeSpan progress);

        event EventHandler<ISong> SongChanged;
        bool IsPlaying { get; set; }
        ISong CurrentSong { get; set; }
    }
}
