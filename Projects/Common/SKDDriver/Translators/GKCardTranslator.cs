﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKDDriver.DataAccess;
using FiresecAPI.GK;

namespace SKDDriver
{
	public class GKCardTranslator
	{
		DataAccess.SKDDataContext Context;

		public GKCardTranslator(SKDDatabaseService databaseService)
		{
			Context = databaseService.Context;
		}

		public int GetGKNoByCardNo(string gkIPAddress, int cardNo)
		{
			var gkCard = Context.GKCards.FirstOrDefault(x => x.IPAddress == gkIPAddress && x.CardNo == cardNo);
			if (gkCard != null)
			{
				return gkCard.GKNo;
			}
			return -1;
		}

		public int GetCardNoByGKNo(string gkIPAddress, int gkNo)
		{
			var gkCard = Context.GKCards.FirstOrDefault(x => x.IPAddress == gkIPAddress && x.GKNo == gkNo);
			if (gkCard != null)
			{
				return gkCard.CardNo;
			}
			return -1;
		}

		public int GetFreeGKNo(string gkIPAddress, int cardNo, out bool isNew)
		{
			var gkCard = Context.GKCards.FirstOrDefault(x => x.IPAddress == gkIPAddress && x.CardNo == cardNo);
			if (gkCard != null)
			{
				isNew = false;
				return gkCard.GKNo;
			}
			gkCard = Context.GKCards.FirstOrDefault(x => x.IPAddress == gkIPAddress && !x.IsActive);
			if (gkCard != null)
			{
				isNew = false;
				return gkCard.GKNo;
			}
			if (Context.GKCards.Where(x => x.IPAddress == gkIPAddress).Count() > 0)
			{
				isNew = true;
				return Context.GKCards.Where(x => x.IPAddress == gkIPAddress).Max(x => x.GKNo) + 1;
			}
			else
			{
				isNew = true;
				return 1;
			}
		}

		public void AddOrEdit(string gkIPAddress, int gkNo, int cardNo, string employeeName)
		{
			if (string.IsNullOrEmpty(gkIPAddress))
			{
				return;
			}
			var gkCard = Context.GKCards.FirstOrDefault(x => x.IPAddress == gkIPAddress && x.GKNo == gkNo);
			if (gkCard != null)
			{
				gkCard.CardNo = cardNo;
				gkCard.FIO = employeeName;
				gkCard.IsActive = true;
				Context.SubmitChanges();
			}
			else
			{
				gkCard = new GKCard()
				{
					UID = Guid.NewGuid(),
					IPAddress = gkIPAddress,
					GKNo = gkNo,
					CardNo = cardNo,
					FIO = employeeName,
					IsActive = true
				};
				Context.GKCards.InsertOnSubmit(gkCard);
				Context.SubmitChanges();
			}
		}

		public void Remove(string gkIPAddress, int gkNo, int cardNo)
		{
			if (string.IsNullOrEmpty(gkIPAddress))
			{
				return;
			}
			var gkCard = Context.GKCards.FirstOrDefault(x => x.IPAddress == gkIPAddress && x.GKNo == gkNo);
			if (gkCard != null)
			{
				gkCard.CardNo = cardNo;
				gkCard.FIO = "Удален";
				gkCard.IsActive = false;
			}
			Context.SubmitChanges();
		}

		public void Actualize(string gkIPAddress, List<GKUser> users)
		{
			var gkCards = Context.GKCards.Where(x => x.IPAddress == gkIPAddress);
			Context.GKCards.DeleteAllOnSubmit(gkCards);
			foreach (var user in users)
			{
				var gkCard = new GKCard();
				gkCard.UID = Guid.NewGuid();
				gkCard.IPAddress = gkIPAddress;
				gkCard.GKNo = user.GKNo;
				gkCard.CardNo = user.Number;
				gkCard.FIO = user.FIO;
				gkCard.IsActive = user.IsActive;
				Context.GKCards.InsertOnSubmit(gkCard);
			}
			Context.SubmitChanges();
		}
	}
}