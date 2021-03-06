﻿using System;
using System.Data.Linq;
using System.Linq;
using FiresecAPI;

namespace SKDDriver.Translators
{
	public class JounalTranslator : IDisposable
	{
		Table<DataAccess.Journal> _Table;
		string Name { get { return "Journal"; } }
		public string NameXml { get { return Name + ".xml"; } }
		public static string ConnectionString { get; set; }
		DataAccess.JournalDataContext Context;

		public JounalTranslator()
		{
			Context = new DataAccess.JournalDataContext(ConnectionString);
			_Table = Context.Journals;
		}

		public OperationResult SaveVideoUID(Guid itemUID, Guid videoUID, Guid cameraUID)
		{
			try
			{
				var tableItem = _Table.FirstOrDefault(x => x.UID == itemUID);
				if (tableItem != null)
				{
					tableItem.VideoUID = videoUID;
					tableItem.CameraUID = cameraUID;
					Context.SubmitChanges();
				}
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}


		public OperationResult SaveCameraUID(Guid itemUID, Guid CameraUID)
		{
			try
			{
				var tableItem = _Table.FirstOrDefault(x => x.UID == itemUID);
				if (tableItem != null)
				{
					tableItem.CameraUID = CameraUID;
					Context.SubmitChanges();
				}
				return new OperationResult();
			}
			catch (Exception e)
			{
				return new OperationResult(e.Message);
			}
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}