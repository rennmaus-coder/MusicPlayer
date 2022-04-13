#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core;
using MusicPlayer.Core.Playback.Queue;
using MusicPlayer.Core.Playback.Queue.Interfaces;
using MusicPlayer.Core.Profile;
using MusicPlayer.Core.Songs.Interfaces;
using MusicPlayer.MVVM.ViewModel.Interfaces;
using MusicPlayer.Util;
using System;

namespace MusicPlayer.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        public IHomeVM HomeVM { get; }
        public IOptionsVM OptionsVM { get; set; }
        public IQueueMediator Queue { get; }
        public ProfileService ProfileService { get; } = ProfileService.Instance;
        public RelayCommand HomeCommand { get; set; }
        public RelayCommand OptionsCommand { get; set; }
        public RelayCommand TogglePlay { get; set; }
        public RelayCommand PlayNext { get; set; }
        public RelayCommand PlayPrevious { get; set; }

        private object _activeVM;
        public object ActiveVM
        {
            get => _activeVM;
            private set
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

        public static MainViewModel instance;

        public MainViewModel()
        {
            instance = this;
            HomeVM = new HomeVM();
            OptionsVM = new OptionsVM();
            ActiveVM = HomeVM;
            
            Queue = new QueueMediator(this);
            Queue.OnSongChanged += Queue_OnSongChanged;

            HomeCommand = new RelayCommand(o =>
            {
                ChangeTab(ApplicationTab.HOME);
            });

            OptionsCommand = new RelayCommand(o =>
            {
                ChangeTab(ApplicationTab.OPTIONS);
            });
        }

        public void ChangeTab(ApplicationTab tab)
        {
            switch (tab)
            {
                case ApplicationTab.HOME:
                    ActiveVM = HomeVM;
                    break;
                case ApplicationTab.OPTIONS:
                    ActiveVM = OptionsVM;
                    break;
            }
        }

        private void Queue_OnSongChanged(object sender, ISong e)
        {
            TrackDuration = e.Info.Duration;
            TrackTitle = e.Info.Title;
        }

        private async void UpdateProgressAsync()
        {
            if (_progressValueUpdating)
                return;
            _progressValueUpdating = true;
            await CoreUtil.WaitForMouseUp();
            instance.Queue.SetProgress(new TimeSpan(0, 0, (int)Math.Round(ProgressValue)));
            Progress = new TimeSpan(0, 0, (int)Math.Round(ProgressValue));
            _progressValueUpdating = false;
        }
    }
}
