#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Playback.Queue;
using MusicPlayer.Core.Playback.Queue.Interfaces;
using MusicPlayer.Core.Songs.Interfaces;
using MusicPlayer.MVVM.ViewModel.Interfaces;
using MusicPlayer.Util;
using System;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        public HomeViewModel HomeViewModel { get; }
        public IQueueMediator Queue { get; }

        private object _activeVM;
        public object ActiveVM
        {
            get => _activeVM;
            set
            {
                _activeVM = value;
                RaisePropertyChanged();
            }
        }

        private TimeSpan _progress = new TimeSpan(0, 0, 0);
        public TimeSpan Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                RaisePropertyChanged();
            }
        }

        private TimeSpan _trackDuration = new TimeSpan(0, 0, 100);
        public TimeSpan TrackDuration
        {
            get => _trackDuration;
            set
            {
                _trackDuration = value;
                RaisePropertyChanged();
            }
        }

        private double _progressValue;
        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    UpdateProgressAsync();
                    RaisePropertyChanged();
                }
            }
        }

        private bool _progressValueUpdating;

        private string _trackTitle;
        public string TrackTitle
        {
            get => _trackTitle;
            set
            {
                _trackTitle = value;
                RaisePropertyChanged();
            }
        }

        private static MainViewModel instance;

        public MainViewModel()
        {
            HomeViewModel = new HomeViewModel();
            ActiveVM = HomeViewModel;
            Queue = new QueueMediator(this);
            instance = this;
            Queue.OnSongChanged += Queue_OnSongChanged;
        }

        private void Queue_OnSongChanged(object sender, ISong e)
        {
            RaiseAllPropertiesChanged();
        }

        private async void UpdateProgressAsync()
        {
            if (_progressValueUpdating)
                return;
            _progressValueUpdating = true;
            await CoreUtil.WaitForMouseUp();
            instance.Queue.SetProgress(new TimeSpan(0, 0, (int)Math.Round(ProgressValue)));
            Progress = new TimeSpan(0, 0, (int)Math.Round(ProgressValue));
            Logger.Info("Progress set to " + (int)Math.Round(ProgressValue));
            _progressValueUpdating = false;
        }
    }
}
