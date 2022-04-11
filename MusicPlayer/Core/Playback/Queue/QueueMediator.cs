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
using MusicPlayer.Core.Songs.Interfaces;
using MusicPlayer.MVVM.ViewModel;
using MusicPlayer.MVVM.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Playback.Queue
{
    public class QueueMediator : IQueueMediator
    {
        public List<ISong> Queue { get; set; }
        public ISong CurrentSong { get; set; }

        public event EventHandler<ISong> OnSongChanged;
        public RelayCommand TogglePlay { get; set; }

        private IMainViewModel mainViewModel;
        private bool isPlaying;
        private List<ISong> PlayedSongs;

        public QueueMediator(IMainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            Queue = new List<ISong>();
        }

        /// <summary>
        /// Starts the playback of the first song in the queue and continues further
        /// </summary>
        /// <returns></returns>
        public async Task PlayNext()
        {
            if (Queue.Count == 0 || isPlaying)
            {
                return;
            }
            CurrentSong = Queue[0];
            isPlaying = true;
            OnSongChanged?.Invoke(this, CurrentSong);
            await CurrentSong.Play(this);
            Queue.RemoveAt(0);
            PlayedSongs.Insert(0, CurrentSong);
            PlayNext();
        }

        public async Task PlayPrevious()
        {
            if (CurrentSong?.Progress.TotalSeconds < 5)
            {
                CurrentSong.SetProgress(new TimeSpan(0, 0, 0));
                await CurrentSong.Play(this);
            }
            else
            {
                Queue.Insert(0, PlayedSongs[0]);
            }
        }

        public void UpdateProgress(TimeSpan progress)
        {
            ((MainViewModel)mainViewModel).Progress = progress;
        }

        public void SetProgress(TimeSpan progress)
        {
            CurrentSong?.SetProgress(progress);
        }
    }
}
