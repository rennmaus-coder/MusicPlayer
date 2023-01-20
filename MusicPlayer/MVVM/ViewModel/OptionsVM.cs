#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core;
using MusicPlayer.Core.Profile;
using MusicPlayer.MVVM.ViewModel.Interfaces;
using MusicPlayer.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.MVVM.ViewModel
{
    public class OptionsVM : ObservableObject, IOptionsVM
    {
        private ProfileService profileService;
        
        public List<IProfile> Profiles
        {
            get => MainViewModel.instance.ProfileService.Profiles;
            set
            {
                MainViewModel.instance.ProfileService.Profiles = value;
                RaisePropertyChanged();
            }
        }

        private int _selectedProfileIndex = 0;
        public int SelectedProfileIndex
        {
            get => _selectedProfileIndex;
            set
            {
                _selectedProfileIndex = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand Add { get; set; }
        public RelayCommand Remove { get; set; }
        public RelayCommand Load { get; set; }

        public OptionsVM(ProfileService profileService)
        {
            this.profileService = profileService;
            Add = new RelayCommand(_ => AddProfile());
            Remove = new RelayCommand(_ => RemoveProfile(Profiles[SelectedProfileIndex]));
            Load = new RelayCommand(_ => LoadProfile(Profiles[SelectedProfileIndex]));
        }

        public IProfile AddProfile()
        {
            Profile profile = new Profile();
            profile.Name = "Default";
            profile.Id = Guid.NewGuid();
            if (Profiles.Where(p => p.Name == profile.Name).Count() > 0)
            {
                profile.Name = "Default " + Profiles.Count();
            }
            List<IProfile> profs = Profiles;
            profs.Add(profile);
            Profiles = profs;
            return profile;
        }

        public void RemoveProfile(IProfile profile)
        {
            List<IProfile> profiles = Profiles;
            profiles.Remove(profile);
            Profiles = profiles;
        }

        public void LoadProfile(IProfile profile)
        {
            profileService.ActiveProfile = profile;
        }
    }
}