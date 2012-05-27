﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Common.Windows.ViewModels
{
	public class ShellViewModel : ApplicationViewModel
	{
		public ShellViewModel()
		{
			AllowHelp = true;
			AllowMaximize = true;
			AllowMinimize = true;
			ContentFotter = null;
			ContentHeader = null;
			MainContent = null;
		}

		private BaseViewModel _toolbar;
		public BaseViewModel Toolbar
		{
			get { return _toolbar; }
			set
			{
				_toolbar = value;
				OnPropertyChanged("Toolbar");
			}
		}
		private BaseViewModel _contentHeader;
		public BaseViewModel ContentHeader
		{
			get { return _contentHeader; }
			set
			{
				_contentHeader = value;
				OnPropertyChanged("ContentHeader");
			}
		}
		private BaseViewModel _contentFotter;
		public BaseViewModel ContentFotter
		{
			get { return _contentFotter; }
			set
			{
				_contentFotter = value;
				OnPropertyChanged("ContentFotter");
			}
		}
		private BaseViewModel _mainContent;
		public BaseViewModel MainContent
		{
			get { return _mainContent; }
			set
			{
				_mainContent = value;
				OnPropertyChanged("MainContent");
			}
		}
	}
}
