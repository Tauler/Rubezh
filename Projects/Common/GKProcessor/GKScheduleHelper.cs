﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FiresecAPI;
using FiresecAPI.GK;
using FiresecClient;

namespace GKProcessor
{
	public static class GKScheduleHelper
	{
		public static OperationResult<bool> GKRewriteAllSchedules(GKDevice device)
		{
			var progressCallback = GKProcessorManager.StartProgress("Перезапись графиков в " + device.PresentationName, "Стирание графиков", 1, true, GKProgressClientType.Administrator);
			var removeResult = GKRemoveAllSchedules(device);
			if (removeResult.HasError)
				return new OperationResult<bool>(removeResult.Error);
			progressCallback = GKProcessorManager.StartProgress("Запись графиков в " + device.PresentationName, "", GKManager.DeviceConfiguration.Schedules.Count + 1, true, GKProgressClientType.Administrator);
			var emptySchedule = new GKSchedule();
			emptySchedule.Name = "Никогда";
			var setResult = GKSetSchedule(device, emptySchedule);
			if (setResult.HasError)
				return new OperationResult<bool>(setResult.Error);
			GKProcessorManager.DoProgress("Запись пустого графика ", progressCallback);
			int i = 1;
			foreach (var schedule in GKManager.DeviceConfiguration.Schedules)
			{
				setResult = GKSetSchedule(device, schedule);
				if (setResult.HasError)
					return new OperationResult<bool>(setResult.Error);
				GKProcessorManager.DoProgress("Запись графика " + i, progressCallback);
				i++;
			}
			GKProcessorManager.StopProgress(progressCallback);
			return new OperationResult<bool>();
		}

		public static OperationResult<bool> GKRemoveAllSchedules(GKDevice device)
		{
			for (int no = 1; no <= 255; no++)
			{
				var bytes = new List<byte>();
				bytes.Add(0);
				bytes.Add((byte)no);
				var nameBytes = BytesHelper.StringDescriptionToBytes("");
				bytes.AddRange(nameBytes);
				for (int i = 0; i < 255 - 32; i++)
				{
					bytes.Add(0);
				}

				var sendResult = SendManager.Send(device, (ushort)(bytes.Count), 28, 0, bytes);
				if (sendResult.HasError) // Если не охранный ГК, то графики пытаются писаться очень долго!
					return new OperationResult<bool> {Error = sendResult.Error, HasError = true};
			}
			return new OperationResult<bool>();
		}

		public static OperationResult<bool> GKSetSchedule(GKDevice device, GKSchedule schedule)
		{
			var count = 0;
			if (schedule.ScheduleType == GKScheduleType.Access)
				foreach (var dayScheduleUID in schedule.DayScheduleUIDs)
				{
					var daySchedule = GKManager.DeviceConfiguration.DaySchedules.FirstOrDefault(x => x.UID == dayScheduleUID);
					if (daySchedule != null)
					{
						count += daySchedule.DayScheduleParts.Count;
					}
				}
			else
			{
				count = schedule.Calendar.SelectedDays.FindAll(x => x > schedule.StartDateTime).Count;
			}
			var secondsPeriod = schedule.DayScheduleUIDs.Count * 60 * 60 * 24;
			if (schedule.SchedulePeriodType == GKSchedulePeriodType.Custom)
				secondsPeriod = schedule.HoursPeriod * 60 * 60;
			if (schedule.SchedulePeriodType == GKSchedulePeriodType.NonPeriodic || schedule.ScheduleType == GKScheduleType.Holiday || schedule.ScheduleType == GKScheduleType.WorkHoliday)
				secondsPeriod = 0;

			var bytes = new List<byte>();
			bytes.Add((byte)schedule.No);
			var nameBytes = BytesHelper.StringDescriptionToBytes(schedule.Name);
			bytes.AddRange(nameBytes);
			bytes.Add((byte)schedule.HolidayScheduleNo);
			bytes.AddRange(BytesHelper.ShortToBytes((ushort)(count * 2)));
			bytes.AddRange(BytesHelper.IntToBytes(secondsPeriod));
			bytes.Add((byte)schedule.WorkHolidayScheduleNo);
			bytes.Add(0);
			bytes.Add(0);
			bytes.Add(0);
			bytes.Add(0);
			bytes.Add(0);
			bytes.Add(0);
			bytes.Add(0);

			var startDateTime = schedule.StartDateTime;
			if (schedule.SchedulePeriodType == GKSchedulePeriodType.Weekly && schedule.ScheduleType == GKScheduleType.Access)
			{
				if (startDateTime.DayOfWeek == DayOfWeek.Monday)
					startDateTime = startDateTime.AddDays(0);
				if (startDateTime.DayOfWeek == DayOfWeek.Tuesday)
					startDateTime = startDateTime.AddDays(-1);
				if (startDateTime.DayOfWeek == DayOfWeek.Wednesday)
					startDateTime = startDateTime.AddDays(-2);
				if (startDateTime.DayOfWeek == DayOfWeek.Thursday)
					startDateTime = startDateTime.AddDays(-3);
				if (startDateTime.DayOfWeek == DayOfWeek.Friday)
					startDateTime = startDateTime.AddDays(-4);
				if (startDateTime.DayOfWeek == DayOfWeek.Saturday)
					startDateTime = startDateTime.AddDays(-5);
				if (startDateTime.DayOfWeek == DayOfWeek.Sunday)
					startDateTime = startDateTime.AddDays(-6);
			}
			var timeSpan = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day) - new DateTime(2000, 1, 1);
			var scheduleStartSeconds = timeSpan.TotalSeconds;

			if (schedule.ScheduleType == GKScheduleType.Access)
				for (int dayNo = 0; dayNo < schedule.DayScheduleUIDs.Count; dayNo++)
				{
					var dayScheduleUID = schedule.DayScheduleUIDs[dayNo];
					var daySchedule = GKManager.DeviceConfiguration.DaySchedules.FirstOrDefault(x => x.UID == dayScheduleUID);
					if (daySchedule != null)
					{
						foreach (var daySchedulePart in daySchedule.DayScheduleParts)
						{
							bytes.AddRange(BytesHelper.IntToBytes((int)(scheduleStartSeconds + 24 * 60 * 60 * dayNo + daySchedulePart.StartMilliseconds / 1000)));
							bytes.AddRange(BytesHelper.IntToBytes((int)(scheduleStartSeconds + 24 * 60 * 60 * dayNo + daySchedulePart.EndMilliseconds / 1000)));

							var startSeconds = 24 * 60 * 60 * dayNo + daySchedulePart.StartMilliseconds / 1000;
							var endSeconds = 24 * 60 * 60 * dayNo + daySchedulePart.EndMilliseconds / 1000;

							Trace.WriteLine("Time interval " + startSeconds + " " + endSeconds);
						}
					}
				}
			else
			{
				foreach (var day in schedule.Calendar.SelectedDays.FindAll(x => x >= schedule.StartDateTime))
				{
					bytes.AddRange(BytesHelper.IntToBytes((int)((day - new DateTime(2000, 1, 1)).TotalSeconds)));
					bytes.AddRange(BytesHelper.IntToBytes((int)((day - new DateTime(2000, 1, 1) + TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1)).TotalSeconds)));
				}
			}

			var packs = new List<List<byte>>();
			for (int packNo = 0; packNo <= bytes.Count / 256; packNo++)
			{
				int packLenght = Math.Min(256, bytes.Count - packNo * 256);
				var packBytes = bytes.Skip(packNo * 256).Take(packLenght).ToList();

				if (packBytes.Count > 0)
				{
					var resultBytes = new List<byte>();
					resultBytes.Add((byte)(packNo));
					resultBytes.AddRange(packBytes);
					packs.Add(resultBytes);
				}
			}

			foreach (var pack in packs)
			{
				var sendResult = SendManager.Send(device, (ushort)(pack.Count), 28, 0, pack);
				if (sendResult.HasError)
				{
					return new OperationResult<bool>(sendResult.Error);
				}
			}

			return new OperationResult<bool>() { Result = true };
		}
	}
}
