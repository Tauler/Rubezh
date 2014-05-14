﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI.GK;
using FiresecAPI.SKD;

namespace SKDDriver
{
	public partial class Watcher
	{
		int LastId = -1;

		void PingJournal()
		{
			var newLastId = GetLastId();
			if (newLastId == -1)
				return;
			if (LastId == -1)
				LastId = newLastId;
			if (newLastId > LastId)
			{
				ReadAndPublish(LastId, newLastId);
				LastId = newLastId;
			}
		}

		int GetLastId()
		{
			var bytes = new List<byte>();
			bytes.Add(2);
			var sendResult = SKDDeviceProcessor.SendBytes(Device, bytes);
			if (IsStopping)
				return -1;
			if (sendResult.HasError || sendResult.Bytes.Count < 4)
			{
				ConnectionChanged(false);
				return -1;
			}
			ConnectionChanged(true);
			var result = BytesHelper.SubstructInt(sendResult.Bytes, 0);
			return result;
		}

		SKDJournalItem ReadJournal(int index)
		{
			LastUpdateTime = DateTime.Now;
			if (IsStopping)
				return null;
			var bytes = new List<byte>();
			bytes.Add(3);
			bytes.AddRange(BitConverter.GetBytes(index).ToList());
			var sendResult = SKDDeviceProcessor.SendBytes(Device, bytes);
			if (sendResult.HasError)
			{
				ConnectionChanged(false);
				return null;
			}
			var journalParser = new JournalParser(Device, sendResult.Bytes);
			return journalParser.JournalItem;
		}

		void ReadAndPublish(int startIndex, int endIndex)
		{
			var journalItems = new List<SKDJournalItem>();
			for (int index = startIndex + 1; index <= endIndex; index++)
			{
				var journalItem = ReadJournal(index);
				if (journalItem != null)
				{
					UpdateDeviceStateOnJournalItem(journalItem);
					journalItems.Add(journalItem);
				}
			}
			if (journalItems.Count > 0)
			{
				AddJournalItems(journalItems);
			}
		}

		bool ReadMissingJournalItems()
		{
			return true;
		}

		void UpdateDeviceStateOnJournalItem(SKDJournalItem journalItem)
		{
			if (journalItem.Device != null)
			{
				if (journalItem.Name == "Неисправность")
				{
					if (!journalItem.Device.State.StateClasses.Contains(XStateClass.Failure))
						journalItem.Device.State.StateClasses.Add(XStateClass.Failure);
					OnDeviceStateChanged(journalItem.Device);
				}
				if (journalItem.Name == "Неисправность устранена")
				{
					journalItem.Device.State.StateClasses.Remove(XStateClass.Failure);
					OnDeviceStateChanged(journalItem.Device);
				}
			}
		}
	}
}