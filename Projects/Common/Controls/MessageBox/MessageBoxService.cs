﻿using System;
using System.Windows;
using Infrastructure.Common;

namespace Controls.MessageBox
{
    public static class MessageBoxService
    {
        static MessageBoxService()
        {
            var resourceService = new ResourceService();
            resourceService.AddResource(new ResourceDescription(typeof(MessageBoxViewModel).Assembly, "MessageBox/DataTemplates/Dictionary.xaml"));
        }

        public static MessageBoxResult Show(string message)
        {
            return ShowWindow(message, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowQuestion(string message)
        {
            return ShowWindow(message, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }

        public static MessageBoxResult ShowWarning(string message)
        {
            return ShowWindow(message, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static MessageBoxResult ShowException(Exception e)
        {
            string message = e.Message.ToString();
            var stackTraces = e.StackTrace.Split('\n');
            if (stackTraces.Length > 0)
                message += "\n" + stackTraces[0];
            if (stackTraces.Length > 1)
                message += "\n" + stackTraces[1];
            return ShowWindow(message, MessageBoxButton.OK, MessageBoxImage.Error, true);
        }

        static MessageBoxResult ShowWindow(string message, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, bool isException = false)
        {
            var messageBoxViewModel = new MessageBoxViewModel(message, messageBoxButton, messageBoxImage, isException);
            UserDialogService.ShowModalWindow(messageBoxViewModel);
            return messageBoxViewModel.Result;
        }
    }
}