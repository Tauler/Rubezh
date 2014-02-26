﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using FiresecAPI;
using System.Collections.ObjectModel;
using Infrastructure.Common;
using GKImitator.Processor;
using XFiresecAPI;

namespace GKImitator.ViewModels
{
	public class ReaderViewModel : BaseViewModel
	{
		public SKDDevice Device { get; private set; }
		SKDImitatorProcessor SKDImitatorProcessor;

		public ReaderViewModel(SKDDevice device, SKDImitatorProcessor skdImitatorProcessor)
		{
			Device = device;
			SKDImitatorProcessor = skdImitatorProcessor;

			SKDEvents = new ObservableCollection<ReaderEventViewModel>();
			foreach (var skdEvent in SKDEventsHelper.SKDEvents)
			{
				if (skdEvent.DriverType == SKDDriverType.Reader)
					SKDEvents.Add(new ReaderEventViewModel(this, skdEvent));
			}
		}

		int _cardSeries;
		public int CardSeries
		{
			get { return _cardSeries; }
			set
			{
				_cardSeries = value;
				OnPropertyChanged("CardSeries");
			}
		}

		int _cardNo;
		public int CardNo
		{
			get { return _cardNo; }
			set
			{
				_cardNo = value;
				OnPropertyChanged("CardNo");
			}
		}

		public ObservableCollection<ReaderEventViewModel> SKDEvents { get; private set; }

		public void NewEvent(SKDEvent skdEvent)
		{
			var address = 0;
			Int32.TryParse(Device.Address, out address);
			SKDImitatorProcessor.LastJournalNo++;
			var imitatorJournalItem = new SKDImitatorJournalItem()
			{
				No = SKDImitatorProcessor.LastJournalNo,
				Source = 1,
				Address = address,
				Code = skdEvent.No,
				CardSeries = CardSeries,
				CardNo = CardNo
			};
			SKDImitatorProcessor.JournalItems.Add(imitatorJournalItem);
		}
	}
}