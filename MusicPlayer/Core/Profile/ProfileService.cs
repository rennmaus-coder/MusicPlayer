#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MusicPlayer.Core.Profile
{
    public class ProfileService : ObservableObject, IDisposable
    {
        public static ProfileService Instance { get; set; }

        private static bool initialized = false;
        
        private IProfile _activeProfile;
        public IProfile ActiveProfile
        {
            get => _activeProfile;
            set
            {
                _activeProfile = value;
                Application.Current.Resources["ActiveProfile"] = value;
                RaisePropertyChanged();
            }
        }
        public List<IProfile> Profiles { get; set; } = new List<IProfile>();
        
        public ProfileService()
        {
            if (initialized) return;
            
            if (File.Exists(Path.Combine(CoreUtil.APPDATA, "profiles.json")))
            {
                Logger.Info("Loading profiles from json");
                List<Profile> temp = JsonConvert.DeserializeObject<List<Profile>>(File.ReadAllText(Path.Combine(CoreUtil.APPDATA, "profiles.json")));
                Profiles.AddRange(temp);
                initialized = true;
                Instance = this;
                foreach (IProfile profile in temp)
                {
                    if (profile.IsDefault)
                    {
                        ActiveProfile = profile;
                        return;
                    }
                }
                if (Profiles.Count > 0)
                {
                    ActiveProfile = Profiles[0];
                    ActiveProfile.IsDefault = true;
                    return;
                }
            }
            Profiles = new List<IProfile>();
            ActiveProfile = new Profile();
            Profiles.Add(ActiveProfile);
        }

        public void Dispose()
        {
            SerializeProfiles();
            Profiles = null;
            ActiveProfile = null;
        }

        public void SerializeProfiles()
        {
            if (!Directory.Exists(CoreUtil.APPDATA)) Directory.CreateDirectory(CoreUtil.APPDATA);
            if (!File.Exists(Path.Combine(CoreUtil.APPDATA, "profiles.json"))) File.Create(Path.Combine(CoreUtil.APPDATA, "profiles.json")).Close();

            Logger.Info("Writing Profiles to disk");
            File.WriteAllText(Path.Combine(CoreUtil.APPDATA, "profiles.json"), JsonConvert.SerializeObject(Profiles, Formatting.Indented));
        }
    }
}
