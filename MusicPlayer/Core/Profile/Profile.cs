#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Songs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MusicPlayer.Core.Profile
{
    public class Profile : IProfile
    {
        [JsonIgnore]
        private static ColorSchemas _colorSchemas;

        [JsonIgnore]
        public ColorSchema ColorSchema { get; set; }
        public List<Song> Songs { get; set; } = new List<Song>();
        public string Name { get; set; } = "Profile";
        public string ColorSchemaName { get; set; } = "Dark";
        public bool IsDefault { get; set; } = true;
        public Guid Id { get; set; }

        public Profile()
        {
            List<ColorSchema> schemas = ColorSchemas.ReadColorSchemas();
            _colorSchemas = new ColorSchemas() { ColorSchemasList = schemas };
            ColorSchema = schemas.Find(x => x.Name == ColorSchemaName);
            ColorSchemaName = ColorSchema.Name;
        }

        public ColorSchemas GetColorSchemas()
        {
            if (_colorSchemas == null)
            {
                _colorSchemas = new ColorSchemas();
            }

            return _colorSchemas;
        }

        public void SetColorSchema(ColorSchema schema)
        {
            ColorSchema = schema;
            ColorSchemaName = schema.Name;
        }
    }
}
