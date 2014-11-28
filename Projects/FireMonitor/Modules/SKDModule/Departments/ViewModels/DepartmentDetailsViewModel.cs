﻿using System;
using System.Collections.Generic;
using FiresecAPI.SKD;
using FiresecClient.SKDHelpers;
using Infrastructure;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using SKDModule.Events;

namespace SKDModule.ViewModels
{
	public class DepartmentDetailsViewModel : SaveCancelDialogViewModel, IDetailsViewModel<ShortDepartment>
	{
		Guid OrganisationUID { get; set; }
		Department Department { get; set; }
		public EmployeeSelectationViewModel ChiefViewModel { get; private set; }
		public bool IsNew { get; private set; }

		public DepartmentDetailsViewModel() { }
		
		public bool Initialize(Organisation organisation, ShortDepartment shortDepartment, ViewPartViewModel parentViewModel)
		{
			OrganisationUID = organisation.UID;
			if (shortDepartment == null)
			{
				Title = "Создание отдела";
				IsNew = true;
				var parentModel = (parentViewModel as DepartmentsViewModel).SelectedItem.Model;
				Department = new Department()
				{
					Name = "Новый отдел",
					ParentDepartmentUID = parentModel != null ? parentModel.UID : new Guid?(),
					OrganisationUID = OrganisationUID
				};
			}
			else
			{
				Department = DepartmentHelper.GetDetails(shortDepartment.UID);
				Title = string.Format("Свойства отдела: {0}", Department.Name);
			}
			CopyProperties();
			ChiefViewModel = new EmployeeSelectationViewModel(Department.ChiefUID, new EmployeeFilter { DepartmentUIDs = new List<Guid> { Department.UID } });
			return true;
		}

		public void Initialize(Guid organisationUID, Guid? parentDepartmentUID)
		{
			OrganisationUID = organisationUID;
			Title = "Создание отдела";
			Department = new Department()
			{
				Name = "Новый отдел",
				ParentDepartmentUID = parentDepartmentUID,
				OrganisationUID = OrganisationUID
			};
			CopyProperties();
			ChiefViewModel = new EmployeeSelectationViewModel(Department.ChiefUID, new EmployeeFilter { DepartmentUIDs = new List<Guid> { Department.UID } });
		}

		public void CopyProperties()
		{
			Name = Department.Name;
			Description = Department.Description;
			Phone = Department.Phone;
			if (Department.Photo != null)
				PhotoData = Department.Photo.Data;
		}

		string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(() => Name);
				}
			}
		}

		string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				if (_description != value)
				{
					_description = value;
					OnPropertyChanged(() => Description);
				}
			}
		}

		string _phone;
		public string Phone
		{
			get { return _phone; }
			set
			{
				if (_phone != value)
				{
					_phone = value;
					OnPropertyChanged(() => Phone);
				}
			}
		}

		byte[] _photoData;
		public byte[] PhotoData
		{
			get { return _photoData; }
			set
			{
				_photoData = value;
				OnPropertyChanged(()=>PhotoData);
			}
		}

		protected override bool CanSave()
		{
			return true;
		}

		public ShortDepartment Model 
		{ 
			get
			{
				return new ShortDepartment
				{
					UID = Department.UID,
					Description = Department.Description,
					Name = Department.Name,
					ParentDepartmentUID = Department.ParentDepartmentUID,
					ChildDepartmentUIDs = Department.ChildDepartmentUIDs,
					Phone = Department.Phone, 
					OrganisationUID = OrganisationUID
				};
			}
		}

		protected override bool Save()
		{
			Department.Name = Name;
			Department.Description = Description;
			if (Department.Photo == null)
				Department.Photo = new Photo();
			Department.Photo.Data = PhotoData;
			Department.ChiefUID = ChiefViewModel.SelectedEmployeeUID;
			Department.Phone = Phone;
			if (!DetailsValidateHelper.Validate(Model))
				return false;
			var saveResult = DepartmentHelper.Save(Department, IsNew);
			if (saveResult)
			{
				ServiceFactory.Events.GetEvent<ChangeDepartmentChiefEvent>().Publish(Department);
				return true;
			}
			else
				return false;
		}

		bool Validate()
		{
			if (Department.Phone.Length > 50)
			{
				MessageBoxService.Show("Значение поля 'Телефон' не может быть длиннее 50 символов");
				return false;
			}
			return true;
		}
	}
}