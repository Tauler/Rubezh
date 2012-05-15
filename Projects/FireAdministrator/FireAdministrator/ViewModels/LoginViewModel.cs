﻿using System.Reflection;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.MessageBox;

namespace FireAdministrator.ViewModels
{
    public class LoginViewModel : DialogContent
    {
        public LoginViewModel()
        {
            Title = "Администратор. Авторизация";
            ConnectCommand = new RelayCommand(OnConnect);
            CancelCommand = new RelayCommand(OnCancel);

            UserName = ServiceFactory.AppSettings.DefaultLogin;
            Password = ServiceFactory.AppSettings.DefaultPassword;
        }

        public bool AutoConnect()
        {
            var userName = ServiceFactory.AppSettings.DefaultLogin;
            var password = ServiceFactory.AppSettings.DefaultPassword;
            if (userName != null && password != null)
            {
                string serverAddress = ServiceFactory.AppSettings.ServiceAddress;

                var result = DoConnect(serverAddress, userName, password);
                return result;
            }
            return false;
        }

        string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        string _password;
		[ObfuscationAttribute(Exclude = true)]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public RelayCommand ConnectCommand { get; private set; }
        void OnConnect()
        {
            var result = DoConnect(ServiceFactory.AppSettings.ServiceAddress, UserName, Password);
            if (result)
                Close(true);
        }

        void OnCancel()
        {
            Close(false);
        }

        bool DoConnect(string serverAddress, string userName, string password)
        {
            var preLoadWindow = new PreLoadWindow()
            {
                PreLoadText = "Соединение с сервером..."
            };
            preLoadWindow.Show();
            string message = FiresecManager.Connect("Администратор", serverAddress, userName, password);
            preLoadWindow.Close();

            if (message == null)
            {
                return true;
            }
            MessageBoxService.Show(message);
            return false;
        }
    }
}