﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FiresecAPI;
using LinqKit;

namespace SKDDriver
{
	public class PositionTranslator : WithShortTranslator<DataAccess.Position, Position, PositionFilter, ShortPosition>
	{
		public PositionTranslator(DataAccess.SKDDataContext context, PhotoTranslator photoTranslator)
			: base(context)
		{
			PhotoTranslator = photoTranslator;
		}

		PhotoTranslator PhotoTranslator;

		protected override OperationResult CanSave(Position item)
		{
			bool sameName = Table.Any(x => x.Name == item.Name && 
				x.OrganisationUID == item.OrganisationUID && 
				x.UID != item.UID &&
				!x.IsDeleted);
			if (sameName)
				return new OperationResult("Попытка добавления должности с совпадающим именем");
			return base.CanSave(item);
		}

		protected override OperationResult CanDelete(Guid uid)
		{
			if (Context.Employees.Any(x => x.PositionUID == uid && !x.IsDeleted))
				return new OperationResult("Не могу удалить должность, пока она указана у действующих сотрудников");
			return base.CanDelete(uid);
		}

		protected override Position Translate(DataAccess.Position tableItem)
		{
			var result = base.Translate(tableItem);
			result.Name = tableItem.Name;
			result.Description = tableItem.Description;
			result.Photo = GetResult(PhotoTranslator.GetSingle(tableItem.PhotoUID));
			return result;
		}

		protected override void TranslateBack(DataAccess.Position tableItem, Position apiItem)
		{
			base.TranslateBack(tableItem, apiItem);
			tableItem.Name = apiItem.Name;
			tableItem.Description = apiItem.Description;
			if(apiItem.Photo != null)
				tableItem.PhotoUID = apiItem.Photo.UID;
		}

		protected override ShortPosition TranslateToShort(DataAccess.Position tableItem)
		{
			var shortPosition = new ShortPosition
			{
				UID = tableItem.UID,
				Name = tableItem.Name,
				Description = tableItem.Description,
				OrganisationUID = tableItem.OrganisationUID
			};
			return shortPosition;
		}

		public ShortPosition GetSingleShort(Guid? uid)
		{
			if (uid == null)
				return null;
			var tableItem = Table.Where(x => x.UID.Equals(uid.Value)).FirstOrDefault();
			if (tableItem == null)
				return null;
			return TranslateToShort(tableItem);
		}

		protected override Expression<Func<DataAccess.Position, bool>> IsInFilter(PositionFilter filter)
		{
			var result = PredicateBuilder.True<DataAccess.Position>();
			result = result.And(base.IsInFilter(filter));
			return result;
		}

		public override OperationResult Save(Position apiItem)
		{
			var photoSaveResult = PhotoTranslator.Save(new List<Photo> { apiItem.Photo });
			if (photoSaveResult.HasError)
				return photoSaveResult;
			return base.Save(apiItem);
		}
	}
}