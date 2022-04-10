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
using MusicPlayer.MVVM.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Playback.Queue
{
    public class QueueMediator : IQueueMediator
    {
        public List<Song> Queue { get; set; }
        public Song CurrentSong { get; set; }
        public int QueueIndex { get; set; }
        
        private IMainViewModel mainViewModel;

        public QueueMediator(IMainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            Queue = new List<Song>();
            QueueIndex = -1;
        }

        public async Task Start()
        {
            QueueIndex = -1;
            
            foreach (var song in Queue)
            {
                QueueIndex++;
                CurrentSong = song;
                await song.Play(this);
            }
        }

        public void PlayNext()
        {
            CurrentSong.Stop();
        }

        public void PlayPrevious()
        {
            throw new NotImplementedException();
        }

        public void UpdateProgress(TimeSpan progress)
        {
            throw new NotImplementedException();
        }
    }
}
