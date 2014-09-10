﻿using System;
using FiresecAPI.SKD;
using FiresecClient.SKDHelpers;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class DayIntervalDetailsViewModel : SaveCancelDialogViewModel, IDetailsViewModel<DayInterval>
	{
		FiresecAPI.SKD.Organisation Organisation;
		public DayInterval Model { get; private set; }

        public void Initialize(Organisation organisation, DayInterval model, ViewPartViewModel parentViewModel)
        {
            Organisation = organisation;
            if (model == null)
            {
                Title = "Новый дневной график";
                model = new DayInterval()
                {
                    Name = "Дневной график",
                    OrganisationUID = organisation.UID,
                };
                model.DayIntervalParts.Add(new DayIntervalPart() { BeginTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(18, 0, 0), DayIntervalUID = model.UID });
            }
            else
                Title = "Редактирование дневного графика";
            Model = model;
            Name = Model.Name;
            Description = Model.Description;
            ConstantSlideTime = Model.SlideTime;
        }

		string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(() => Name);
			}
		}

		string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				OnPropertyChanged(() => Description);
			}
		}

		TimeSpan _constantSlideTime;
		public TimeSpan ConstantSlideTime
		{
			get { return _constantSlideTime; }
			set
			{
				_constantSlideTime = value;
				OnPropertyChanged(() => ConstantSlideTime);
			}
		}

		protected override bool CanSave()
		{
			return !string.IsNullOrEmpty(Name) && Name != "Всегда" && Name != "Никогда";
		}
		protected override bool Save()
		{
			Model.Name = Name;
			Model.Description = Description;
			Model.SlideTime = ConstantSlideTime;
			return DayIntervalHelper.Save(Model);
		}
    }
}