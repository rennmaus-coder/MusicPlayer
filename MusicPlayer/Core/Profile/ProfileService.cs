#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Util;
using System.Windows;

namespace MusicPlayer.Core.Profile
{
    public class ProfileService : ObservableObject
    {
        private Profile _activeProfile;
        public Profile ActiveProfile
        {
            get => _activeProfile;
            set
            {
                _activeProfile = value;
                Application.Current.Resources["ActiveProfile"] = value;
                RaisePropertyChanged();
            }
        }
        
        public ProfileService()
        {
            ActiveProfile = new Profile();
            ActiveProfile.ColorSchemaSettings = new ColorSchemas();
            ActiveProfile.ColorSchemaSettings.ColorSchemasList = ColorSchemas.ReadColorSchemas();
            ActiveProfile.ColorSchema = ActiveProfile.ColorSchemaSettings.ColorSchemasList[0];
        }
    }
}
