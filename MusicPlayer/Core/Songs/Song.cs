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
using Newtonsoft.Json;
using System;
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
        
        private Task PlayTask;
        private CancellationTokenSource PlayCancellationToken;
        private WaveOutEvent activeDevice;
        private AudioFileReader activeReader;
        private IQueueMediator queue;

        #endregion

        #region Public Properties

        [JsonIgnore]
        public TimeSpan Progress { get; set; }
        public SongInfo Info { get; set; }

        #endregion

        #region Interface Methods

        public async Task Play(IQueueMediator mediator)
        {
            queue = mediator;
            if (PlayTask.Status == TaskStatus.Running)
            {
                Logger.Warn($"Song {Info.Title} is already playing");
                return;
            }

            PlayTask = new Task(() => PlayThread(new TimeSpan(0, 0, 0)), PlayCancellationToken.Token, TaskCreationOptions.LongRunning);
            PlayTask.Start();
            PlayTask.Wait();

            return;
        }

        public void Stop()
        {
            if (PlayTask is null || PlayCancellationToken is null || activeDevice is null) return;
            activeDevice.Stop();
            if (PlayTask.Status == TaskStatus.Running)
            {
                PlayCancellationToken.Cancel();
            }
            Progress = new TimeSpan(0, 0, 0);
        }

        /// <summary>
        /// Stops the song, progress stays the same.
        /// </summary>
        public void Pause()
        {
            if (activeDevice is null) return;
            activeDevice.Pause();
        }

        public bool Continue()
        {
            if (activeDevice != null)
            {
                activeDevice.Play();
                return true;
            }
            return false;
        }

        public void SetProgress(TimeSpan progress) {
            if (activeReader is null) return;
            activeReader.CurrentTime = progress;
            Progress = progress;
        }
        #endregion

        #region Private Methods

        [STAThread]
        private void PlayThread(TimeSpan progress)
        {
            activeReader = new AudioFileReader(Info.FilePath);
            using (activeDevice = new WaveOutEvent())
            {
                activeDevice.Init(activeReader);
                activeReader.CurrentTime = progress;
                activeDevice.Play();
                while (activeDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                    Progress = activeReader.CurrentTime;
                    queue.UpdateProgress(Progress);
                }
            }
        }
        
        #endregion
    }
}
