﻿using System;
using ChinaSKDDriverNativeApi;

namespace ChinaSKDDriverAPI
{
	public class Card
	{
		public int RecordNo { get; set; }
		public string CardNo { get; set; }
		public CardType CardType { get; set; }
		public string Password { get; set; }
		public int DoorsCount { get; set; }
		public int[] Doors { get; set; }
		public int TimeSectionsCount { get; set; }
		public int[] TimeSections { get; set; }
		public int UserTime { get; set; }
		public DateTime ValidStartDateTime { get; set; }
		public DateTime ValidEndDateTime { get; set; }
	}

	public enum CardType
	{
		NET_ACCESSCTLCARD_TYPE_UNKNOWN = -1,
		NET_ACCESSCTLCARD_TYPE_GENERAL,
		NET_ACCESSCTLCARD_TYPE_VIP,
		NET_ACCESSCTLCARD_TYPE_GUEST,
		NET_ACCESSCTLCARD_TYPE_PATROL,
		NET_ACCESSCTLCARD_TYPE_BLACKLIST,
		NET_ACCESSCTLCARD_TYPE_CORCE,
		NET_ACCESSCTLCARD_TYPE_MOTHERCARD = 0xff,
	}
}