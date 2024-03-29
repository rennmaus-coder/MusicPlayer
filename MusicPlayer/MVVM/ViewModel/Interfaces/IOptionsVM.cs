﻿#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Profile;
using System.Collections.Generic;

namespace MusicPlayer.MVVM.ViewModel.Interfaces
{
    public interface IOptionsVM
    {
        List<IProfile> Profiles { get; set; }

        IProfile AddProfile();
        void RemoveProfile(IProfile profile);
        void LoadProfile(IProfile profile);
    }
}
