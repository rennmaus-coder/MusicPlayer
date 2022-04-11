#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Core.Profile
{
    public interface IProfile
    {
        ColorSchema ColorSchema { get; set; }
        List<Song> Songs { get; set; }
        bool IsDefault { get; set; }

        ColorSchemas GetColorSchemas();
        void SetColorSchema(ColorSchema colorSchema);
    }
}
