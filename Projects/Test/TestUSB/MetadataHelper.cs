﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace TestUSB
{
	public static class MetadataHelper
	{
		static Rubezh2010.driverConfig Metadata;

		public static void Initialize()
		{
			using (var fileStream = new FileStream("rubezh2010.xml", FileMode.Open))
			{
				var serializer = new XmlSerializer(typeof(Rubezh2010.driverConfig));
				Metadata = (Rubezh2010.driverConfig)serializer.Deserialize(fileStream);
			}
		}

		public static string GetEventByCode(int eventCode)
		{
			string stringCode = "$" + eventCode.ToString("X2");
			var nativeEvent = Metadata.events.FirstOrDefault(x => x.rawEventCode == stringCode);
			if (nativeEvent != null)
				return nativeEvent.eventMessage;
			return "Неизвестный код события " + eventCode.ToString("x2");
		}
	}
}