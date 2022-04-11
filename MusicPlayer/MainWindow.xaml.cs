#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.Core.Profile;
using MusicPlayer.MVVM.ViewModel;
using MusicPlayer.Util;
using System;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            new ProfileService();
            InitializeComponent();
        }
    }
}