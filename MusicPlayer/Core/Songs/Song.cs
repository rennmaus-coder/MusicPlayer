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
using MusicPlayer.Util;
using NAudio.Wave;
using System;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Songs
{
    public class Song : ISong
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="info">A SongInfo instance</param>
        public Song(SongInfo info) => Info = info;

        #region Private Fields
        
        private SongInfo Info { get; set; }
        
        private Thread PlayTask;
        private CancellationTokenSource PlayCancellationToken;
        private IQueueMediator queue;

        #endregion

        #region Public Properties

        public TimeSpan Progress { get; set; }

        #endregion

        #region Interface Methods
        public SongInfo GetInfo()
        {
            return Info;
        }

        public async Task Play(IQueueMediator mediator)
        {
            queue = mediator;
            if (PlayTask.ThreadState == ThreadState.Running)
            {
                Logger.Warn($"Song {Info.Title} is already playing");
                return;
            }
            PlayTask = new Thread(new ThreadStart(PlayThread));
            PlayTask.Start();
            PlayTask.Join();

            return;
        }

        public void Stop()
        {
            if (PlayTask is null || PlayCancellationToken is null) return;
            if (PlayTask.ThreadState == ThreadState.Running)
            {
                PlayCancellationToken.Cancel();
            }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Continue()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods

        private void PlayThread()
        {
            AudioFileReader reader = new AudioFileReader(Info.FilePath);
            using (WaveOutEvent outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(reader);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(200);
                    Progress = reader.CurrentTime;
                    queue.UpdateProgress(Progress);
                }
            }
        }
        
        #endregion
    }
}
