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
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MusicPlayer.Core.Profile
{
    public class ProfileService : ObservableObject
    {
        [JsonIgnore]
        private IProfile _activeProfile;
        [JsonIgnore]
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
            if (Directory.Exists(Path.Combine(CoreUtil.APPDATA, "Profiles"))) 
            {
                if (File.Exists(Path.Combine(CoreUtil.APPDATA, "Profiles", "profiles.json")))
                {
                    Profiles = JsonConvert.DeserializeObject<List<IProfile>>(Path.Combine(CoreUtil.APPDATA, "Profiles", "profiles.json"));
                    foreach (IProfile profile in Profiles)
                    {
                        if (profile.IsDefault)
                        {
                            ActiveProfile = profile;
                            break;
                        }
                    }
                }
                else
                {
                    Profiles = new List<IProfile>();
                }
            }
            else
            {
                Profiles = new List<IProfile>();
            }
            ActiveProfile = new Profile();
            Profiles.Add(ActiveProfile);
        }
    }
}
