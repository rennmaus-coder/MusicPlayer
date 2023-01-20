#region "copyright"

/*
    Copyright © 2022 Christian Palm (christian@palm-family.de)
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion "copyright"

using MusicPlayer.MVVM.ViewModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.Exit += (sender, e) =>
            {
                MainViewModel.instance.ProfileService.Dispose();
            };
        }

        public void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            // if (e.ChangedButton == MouseButton.Left)
                // MainViewModel.instance.SliderIsUpdating = true;
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            // if (e.ChangedButton == MouseButton.Left)
            //    MainViewModel.instance.SliderIsUpdating = false;
        }
    }
}