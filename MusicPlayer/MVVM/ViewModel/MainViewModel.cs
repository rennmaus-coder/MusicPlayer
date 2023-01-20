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
using System.Threading.Tasks;
using System.Windows;

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
        public RelayCommand TogglePlayCommand { get; set; }
        public RelayCommand PlayNextCommand { get; set; }
        public RelayCommand PlayPreviousCommand { get; set; }

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
                if (SliderIsUpdating)
                    return;
                _progress = new TimeSpan(value.Hours, value.Minutes, value.Seconds);
                ProgressValue = new ProgressValue() { Progress = value.TotalSeconds, FromCode = true };
                RaisePropertyChanged();
            }
        }

        private TimeSpan _trackDuration = new TimeSpan(0, 0, 0);
        public TimeSpan TrackDuration
        {
            get => _trackDuration;
            set
            {
                _trackDuration = new TimeSpan(value.Hours, value.Minutes, value.Seconds);

                RaisePropertyChanged();
            }
        }

        private double _progressValue;
        /* public double ProgressValue
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
        } */

        // This Property only gets called from the XAML
        /* public double ProgressXAML
        {
            get => ProgressValue;
            set
            {
                if (!fromCode)
                    UpdateProgressAsync();
                else
                    ProgressValue = value;
            }
        } */

        public ProgressValue ProgressValue
        {
            get => new ProgressValue() { Progress = _progressValue };
            set
            {
                if (!value.FromCode)
                {
                    UpdateProgressAsync();
                    RaisePropertyChanged();
                }
                else
                {
                    if (!SliderIsUpdating)
                    {
                        _progressValue = value.Progress;
                        RaisePropertyChanged();
                    }
                }
            }
        }


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
        public bool SliderIsUpdating { get; set; } = false;

        public MainViewModel()
        {
            instance = this;
            HomeVM = new HomeVM();
            OptionsVM = new OptionsVM(ProfileService);
            ActiveVM = HomeVM;
            
            Queue = new QueueMediator(this);
            Queue.SongChanged += Queue_OnSongChanged;

            HomeCommand = new RelayCommand(o =>
            {
                ChangeTab(ApplicationTab.HOME);
            });

            OptionsCommand = new RelayCommand(o =>
            {
                ChangeTab(ApplicationTab.OPTIONS);
            });

            TogglePlayCommand = new RelayCommand(o =>
            {
                TogglePlay();
            });

            PlayNextCommand = new RelayCommand(o =>
            {
                PlayNext();
            });

            PlayPreviousCommand = new RelayCommand(o =>
            {
                PlayPrevious();
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

        public void TogglePlay()
        {
            if (Queue.IsPlaying)
            {
                Queue.Pause();
            }
            else
            {
                Queue.StartAt(-1);
            }
        }

        public void PlayNext()
        {
            Queue.Stop();
            Queue.PlayNext();
        }

        public void PlayPrevious()
        {
            Queue.Stop();
            Queue.PlayPrevious();
        }

        private void Queue_OnSongChanged(object sender, ISong e)
        {
            TrackDuration = e.Info.Duration;
            TrackTitle = e.Info.Title;
        }

        private async void UpdateProgressAsync()
        {
            if (SliderIsUpdating)
                return;
            SliderIsUpdating = true;
            await CoreUtil.WaitForMouseUp();
            instance.Queue.SetProgress(new TimeSpan(0, 0, (int)Math.Round(ProgressValue.Progress)));
            Progress = new TimeSpan(0, 0, (int)Math.Round(ProgressValue.Progress));
            SliderIsUpdating = false;
        }
    }
}
