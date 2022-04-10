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
using System.Windows.Media;

namespace MusicPlayer.Core.Profile
{
    public class ColorSchemas
    {
        public ColorSchemas()
        {
            ColorSchemasList = new List<ColorSchema>();
        }

        public List<ColorSchema> ColorSchemasList { get; set; }

        public static List<ColorSchema> ReadColorSchemas()
        {
            List<JsonColor> schemas;
            List<ColorSchema> schemes = new List<ColorSchema>();
            var schemafile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Core", "Profile", "ColorSchemas.json");
            if (File.Exists(schemafile))
            {
                try
                {
                    schemas = JsonConvert.DeserializeObject<List<JsonColor>>(File.ReadAllText(schemafile));
                    foreach (JsonColor color in schemas)
                    {
                        ColorSchema colorSchema = new ColorSchema
                        {
                            BackgroundColor = (Color)ColorConverter.ConvertFromString(color.BackgroundColor),
                            BorderColor = (Color)ColorConverter.ConvertFromString(color.BorderColor),
                            ButtonBackgroundColor = (Color)ColorConverter.ConvertFromString(color.ButtonBackgroundColor),
                            ButtonBackgroundSelectedColor = (Color)ColorConverter.ConvertFromString(color.ButtonBackgroundSelectedColor),
                            ButtonForegroundColor = (Color)ColorConverter.ConvertFromString(color.ButtonForegroundColor),
                            ButtonForegroundDisabledColor = (Color)ColorConverter.ConvertFromString(color.ButtonForegroundDisabledColor),
                            SecondaryBackgroundColor = (Color)ColorConverter.ConvertFromString(color.SecondaryBackgroundColor),
                            PrimaryColor = (Color)ColorConverter.ConvertFromString(color.PrimaryColor),
                            Name = color.Name
                        };
                        schemes.Add(colorSchema);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    return new List<ColorSchema>();
                }
            }
            else
            {
                Logger.Error("Color schema json not found!");
                return new List<ColorSchema>();
            }

            return schemes;
        }
    }

    public class ColorSchema : ObservableObject
    {
        private Color buttonForegroundDisabledColor;
        private Color buttonForegroundColor;
        private Color buttonBackgroundSelectedColor;
        private Color buttonBackgroundColor;
        private Color secondaryBackgroundColor;
        private Color backgroundColor;
        private Color borderColor;
        private Color primaryColor;

        public string Name { get; set; }

        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Color SecondaryBackgroundColor
        {
            get
            {
                return secondaryBackgroundColor;
            }
            set
            {
                if (secondaryBackgroundColor != value)
                {
                    secondaryBackgroundColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Color ButtonBackgroundColor
        {
            get
            {
                return buttonBackgroundColor;
            }
            set
            {
                if (buttonBackgroundColor != value)
                {
                    buttonBackgroundColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Color ButtonBackgroundSelectedColor
        {
            get
            {
                return buttonBackgroundSelectedColor;
            }
            set
            {
                if (buttonBackgroundSelectedColor != value)
                {
                    buttonBackgroundSelectedColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Color ButtonForegroundColor
        {
            get
            {
                return buttonForegroundColor;
            }
            set
            {
                if (buttonForegroundColor != value)
                {
                    buttonForegroundColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Color ButtonForegroundDisabledColor
        {
            get
            {
                return buttonForegroundDisabledColor;
            }
            set
            {
                if (buttonForegroundDisabledColor != value)
                {
                    buttonForegroundDisabledColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Color PrimaryColor
        {
            get
            {
                return primaryColor;
            }
            set
            {
                if (primaryColor != value)
                {
                    primaryColor = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ColorSchema()
        {
        }
    }

    internal class JsonColor
    {
        public string Name;
        public string ButtonForegroundDisabledColor;
        public string ButtonForegroundColor;
        public string ButtonBackgroundSelectedColor;
        public string ButtonBackgroundColor;
        public string SecondaryBackgroundColor;
        public string BackgroundColor;
        public string BorderColor;
        public string PrimaryColor;
    }
}