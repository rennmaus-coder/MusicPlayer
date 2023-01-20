#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Playback.Queue.Interfaces;
using MusicPlayer.Core.Songs.Interfaces;
using MusicPlayer.MVVM.ViewModel;
using MusicPlayer.MVVM.ViewModel.Interfaces;
using System;
using System.Collections.Generic;

namespace MusicPlayer.Core.Playback.Queue
{
    public class QueueMediator : IQueueMediator
    {
        public List<ISong> Queue { get; set; }
        public ISong CurrentSong { get; set; }
        public bool IsPlaying { get; set; }

        public event EventHandler<ISong> SongChanged;
        public RelayCommand TogglePlay { get; set; }

        private IMainViewModel mainViewModel;
        private int currentIndex = 0;

        public QueueMediator(IMainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            Queue = mainViewModel.HomeVM.GetSongs();
        }

        /// <summary>
        /// Starts the playback of the first song in the queue and continues further
        /// </summary>
        /// <returns></returns>
        public void PlayNext()
        {
            CurrentSong?.Stop();
            StartAt(Queue.IndexOf(CurrentSong) + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">If set to -1 it will continue from the last position</param>
        public async void StartAt(int index)
        {
            if (Queue.Count == 0)
                return;

            if (index >= Queue.Count)
                return;

            if (CurrentSong != null)
            {
                CurrentSong.Stop();
            }
            IsPlaying = true;

            if (index >= 0)
            {
                currentIndex = index;
            }

            for (; currentIndex < Queue.Count; currentIndex++)
            {
                CurrentSong = Queue[currentIndex];
                SongChanged?.Invoke(this, CurrentSong);
                await CurrentSong.Play(this);
                if (CurrentSong.WasCancelled)
                {
                    IsPlaying = false;
                    return;
                }
            }
            IsPlaying = false;
        }

        public void PlayPrevious()
        {
            if (CurrentSong?.Progress.TotalSeconds < 5 || currentIndex >= Queue.Count)
            {
                CurrentSong.Stop();
                StartAt(Queue.IndexOf(CurrentSong) - 2);
            }
            else
            {
                SetProgress(new TimeSpan(0, 0, 0));
            }
        }

        public void Stop()
        {
            CurrentSong.Stop();
            IsPlaying = false;
        }

        public void Pause() 
        {
            CurrentSong.Pause();
            IsPlaying = false;
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
