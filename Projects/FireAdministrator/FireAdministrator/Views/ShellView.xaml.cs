﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Common;
using FiresecClient;
using Infrastructure.Common;

namespace FireAdministrator
{
    public partial class ShellView : Window, INotifyPropertyChanged
    {
        public ShellView()
        {
            InitializeComponent();
            DataContext = this;
        }

        void Header_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }

        void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width + e.HorizontalChange > 10)
                this.Width += e.HorizontalChange;
            if (this.Height + e.VerticalChange > 10)
                this.Height += e.VerticalChange;
        }

        void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                ChangeMaximizedState();
        }

        void ChangeMaximizedState()
        {
            if (Application.Current.MainWindow.WindowState == System.Windows.WindowState.Normal)
                Application.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
        }

        void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void OnMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        void OnMaximize(object sender, RoutedEventArgs e)
        {
            ChangeMaximizedState();
        }

        void OnShowHelp(object sender, RoutedEventArgs e)
        {
            var helperPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Firesec\FS_CL_ADMIN.HLP");
            Process.Start(helperPath);
        }

        void OnShowAbout(object sender, RoutedEventArgs e)
        {
        }

        public IViewPart MainContent
        {
            get { return _mainRegionHost.Content as IViewPart; }
            set { _mainRegionHost.DataContext = _mainRegionHost.Content = value; }
        }

        public object Menu
        {
            get { return _menu.Content; }
            set { _menu.DataContext = _menu.Content = value; }
        }

        public object ValidatoinArea
        {
            get { return _validationArea.Content; }
            set { _validationArea.DataContext = _validationArea.Content = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            AlarmPlayerHelper.Dispose();

            if (DevicesModule.DevicesModule.HasChanges || PlansModule.PlansModule.HasChanges ||
                SoundsModule.SoundsModule.HasChanges ||
                FiltersModule.FilterModule.HasChanges || LibraryModule.LibraryModule.HasChanges ||
                InstructionsModule.InstructionsModule.HasChanges || SecurityModule.SecurityModule.HasChanges)
            {
                var result = DialogBox.DialogBox.Show("Сохранить изменения в настройках?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        FiresecManager.SetConfiguration();
                        return;

                    case MessageBoxResult.No:
                        return;

                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
        }

        void Window_Closed(object sender, System.EventArgs e)
        {
            FiresecManager.Disconnect();
            Application.Current.Shutdown();
        }
    }
}