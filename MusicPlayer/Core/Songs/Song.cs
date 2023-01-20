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
using System.Threading.Tasks;

namespace MusicPlayer.Core.Songs
{
    public class Song : ISong
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="info">A SongInfo instance</param>
        public Song(SongInfo info)
        {
            Info = info;
            Progress = TimeSpan.Zero;
        }

        #region Private Fields
        
        private WaveOutEvent activeDevice;
        private AudioFileReader activeReader;
        private IQueueMediator queue;

        #endregion

        #region Public Properties
        
        [JsonIgnore]        
        public TimeSpan Progress { get; set; }
        public SongInfo Info { get; set; }

        [JsonIgnore]
        public bool WasCancelled { get; set; } = false;

        #endregion

        #region Interface Methods

        public async Task Play(IQueueMediator mediator)
        {
            queue = mediator;
            
            if (activeDevice is null || activeDevice.PlaybackState != PlaybackState.Playing)
            {
                try
                {
                    activeReader = new AudioFileReader(Info.FilePath);
                    using (activeDevice = new WaveOutEvent())
                    {
                        activeDevice.Init(activeReader);
                        activeReader.CurrentTime = Progress;
                        queue.IsPlaying = true;
                        activeDevice.Play();
                        
                        while (activeDevice.PlaybackState == PlaybackState.Playing)
                        {
                            Progress = activeReader.CurrentTime;
                            queue.UpdateProgress(Progress);
                            await Task.Delay(100);
                        }
                        if (activeDevice.PlaybackState == PlaybackState.Stopped) 
                            WasCancelled = true;
                        else
                            WasCancelled = false;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            return;
        }

        public void Stop()
        {
            if (activeDevice is null) return;
            activeDevice.Stop();
            WasCancelled = true;
        }

        public void Pause()
        {
            if (activeDevice is null) return;
            activeDevice.Pause();
        }

        public void SetProgress(TimeSpan progress) {
            if (activeReader is null) return;
            activeReader.CurrentTime = progress;
            Progress = progress;
        }
        #endregion
    }
}
