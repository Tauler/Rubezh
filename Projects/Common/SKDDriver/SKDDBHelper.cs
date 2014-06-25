﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Common;
using FiresecAPI.GK;
using FiresecAPI.SKD;
using JournalItem = FiresecAPI.SKD.JournalItem;

namespace SKDDriver
{
	public static class SKDDBHelper
	{
		public static object locker = new object();
		public static object databaseLocker = new object();
		public static bool IsAbort { get; set; }
		public static event Action<List<JournalItem>> ArchivePortionReady;

		public static void Add(JournalItem journalItem)
		{
			lock (databaseLocker)
			{
				SKDDatabaseService.JournalItemTranslator.Save(new List<JournalItem> { journalItem });
			}
		}

		public static void AddMany(List<JournalItem> journalItems)
		{
			lock (databaseLocker)
			{
				SKDDatabaseService.JournalItemTranslator.Save(journalItems);
			}
		}

		public static JournalItem AddMessage(EventNameEnum name, string userName)
		{
			var result = new JournalItem();
			result.Name = name;
			result.UserName = userName;
			return result;
		}

		public static List<JournalItem> BeginGetSKDFilteredArchive(SKDArchiveFilter archiveFilter, bool isReport)
		{
			var journalItems = new List<JournalItem>();
			var result = new List<JournalItem>();

			try
			{
				lock (locker)
				{
					var connectionString = global::SKDDriver.Properties.Settings.Default.SKUDConnectionString;
					using (var dataContext = new SqlConnection(connectionString))
					{
						var query = BuildQuery(archiveFilter);
						var sqlCommand = new SqlCommand(query, dataContext);
						dataContext.Open();
						var reader = sqlCommand.ExecuteReader();
						while (reader.Read())
						{
							if (IsAbort && !isReport)
								break;
							try
							{
								var journalItem = ReadOneJournalItem(reader);
								result.Add(journalItem);
								if (!isReport)
								{
									journalItems.Add(journalItem);
									if (journalItems.Count >= archiveFilter.PageSize)
										PublishNewItemsPortion(journalItems);
								}
							}
							catch (Exception e)
							{
								Logger.Error(e, "SKD DatabaseHelper.OnGetFilteredArchive");
							}
						}
						if (!isReport)
							PublishNewItemsPortion(journalItems);
					}
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception e)
			{
				Logger.Error(e, "SKDDBHelper.Select");
			}
			return result;
		}

		static void PublishNewItemsPortion(List<JournalItem> journalItems)
		{
			if (ArchivePortionReady != null)
				ArchivePortionReady(journalItems.ToList());
			UpdateNamesDescriptions(journalItems);
			journalItems.Clear();
		}

		static string BuildQuery(SKDArchiveFilter archiveFilter)
		{
			string dateTimeTypeString;
			if (archiveFilter.UseDeviceDateTime)
				dateTimeTypeString = "DeviceDate";
			else
				dateTimeTypeString = "SysemDate";

			var query =
				"SELECT * FROM Journal WHERE " +
				"\n " + dateTimeTypeString + " > '" + archiveFilter.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
				"\n AND " + dateTimeTypeString + " < '" + archiveFilter.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";

			//if (archiveFilter.JournalItemTypes.Count > 0)
			//{
			//	query += "\n AND (";
			//	int index = 0;
			//	foreach (var journalItemType in archiveFilter.JournalItemTypes)
			//	{
			//		if (index > 0)
			//			query += "\n OR ";
			//		index++;
			//		query += "JournalItemType = '" + ((int)journalItemType).ToString() + "'";
			//	}
			//	query += ")";
			//}

			//if (archiveFilter.StateClasses.Count > 0)
			//{
			//	query += "\n AND (";
			//	int index = 0;
			//	foreach (var stateClass in archiveFilter.StateClasses)
			//	{
			//		if (index > 0)
			//			query += "\n OR ";
			//		index++;
			//		query += "StateClass = '" + ((int)stateClass).ToString() + "'";
			//	}
			//	query += ")";
			//}

			if (archiveFilter.EventNames.Count > 0)
			{
				query += "\n and (";
				int index = 0;
				foreach (var eventName in archiveFilter.EventNames)
				{
					if (index > 0)
						query += "\n OR ";
					index++;
					query += "Name = '" + eventName + "'";
				}
				query += ")";
			}

			if (archiveFilter.Descriptions.Count > 0)
			{
				query += "\n AND (";
				int index = 0;
				foreach (var description in archiveFilter.Descriptions)
				{
					if (index > 0)
						query += "\n OR ";
					index++;
					query += "Description = '" + description + "'";
				}
				query += ")";
			}

			//if (archiveFilter.SubsystemTypes.Count > 0)
			//{
			//	query += "\n AND (";
			//	int index = 0;
			//	foreach (var subsystem in archiveFilter.SubsystemTypes)
			//	{
			//		if (index > 0)
			//			query += "\n OR ";
			//		index++;
			//		if (subsystem == SKDSubsystemType.System)
			//			query += "Subsystem = 0";
			//		else
			//			query += "Subsystem = 1";
			//	}
			//	query += ")";
			//}

			//var objectUIDs = new List<Guid>();
			//objectUIDs.AddRange(archiveFilter.DeviceUIDs);
			//if (objectUIDs.Count > 0)
			//{
			//	int index = 0;
			//	query += "\n AND (";
			//	foreach (var objectUID in objectUIDs)
			//	{
			//		if (index > 0)
			//			query += "\n OR ";
			//		index++;
			//		query += "ObjectUID = '" + objectUID + "'";
			//	}
			//	query += ")";
			//}

			query += "\n ORDER BY " + dateTimeTypeString + " DESC ,DeviceNo DESC";
			return query;
		}

		static JournalItem ReadOneJournalItem(SqlDataReader reader)
		{
			var journalItem = new JournalItem();
			//if (!reader.IsDBNull(reader.GetOrdinal("JournalItemType")))
			//	journalItem.JournalItemType = (SKDJournalItemType)reader.GetByte(reader.GetOrdinal("JournalItemType"));

			if (!reader.IsDBNull(reader.GetOrdinal("SysemDate")))
				journalItem.SystemDateTime = reader.GetDateTime(reader.GetOrdinal("SysemDate"));

			if (!reader.IsDBNull(reader.GetOrdinal("DeviceDate")))
				journalItem.DeviceDateTime = reader.GetDateTime(reader.GetOrdinal("DeviceDate"));

			//if (!reader.IsDBNull(reader.GetOrdinal("DeviceUID")))
			//	journalItem.DeviceUID = reader.GetGuid(reader.GetOrdinal("DeviceUID"));

			if (!reader.IsDBNull(reader.GetOrdinal("Name")))
				journalItem.Name = (EventNameEnum)reader.GetOrdinal("NameNo");

			if (!reader.IsDBNull(reader.GetOrdinal("Description")))
				journalItem.Description = (EventDescription)reader.GetOrdinal("DescriptionNo");

			//if (!reader.IsDBNull(reader.GetOrdinal("UserName")))
			//	journalItem.UserName = reader.GetString(reader.GetOrdinal("UserName"));

			return journalItem;
		}

		static void UpdateNamesDescriptions(List<JournalItem> journalItems)
		{
			//var commands = new List<string>();
			//foreach (var item in journalItems)
			//{
			//	var name = item.Name;
			//	if (!EventNames.Any(x => name == x))
			//	{
			//		EventNames.Add(name);
			//		commands.Add(@"Insert Into EventNames (EventName) Values ('" + name + "')");
			//	}

			//	var description = item.Description;
			//	if (!EventDescriptions.Any(x => description == x))
			//	{
			//		EventDescriptions.Add(description);
			//		commands.Add(@"Insert Into EventDescriptions (EventDescription) Values ('" + description + "')");
			//	}
			//}
			//ExecuteNonQuery(commands);
		}
	}
}