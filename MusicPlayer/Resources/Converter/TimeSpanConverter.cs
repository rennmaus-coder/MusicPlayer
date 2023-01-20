#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using System;
using System.Windows.Data;

namespace MusicPlayer.Resources.Converter
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TimeSpan ts)
            {
                return ts.ToString("mm\\:ss");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string span = value as string;
            if (span != null)
            {
                string[] split = span.Split(':');
                int min = int.Parse(split[0]);
                int sec = int.Parse(split[1]);
                return new TimeSpan(0, min, sec);
            }
            return new TimeSpan(0, 0, 0);
        }
    }
}
