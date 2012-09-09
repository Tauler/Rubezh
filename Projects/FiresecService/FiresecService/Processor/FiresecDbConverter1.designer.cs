﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiresecService.Processor
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Firesec")]
	public partial class FiresecDbConverterDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertJournal(Journal instance);
    partial void UpdateJournal(Journal instance);
    partial void DeleteJournal(Journal instance);
    #endregion
		
		public FiresecDbConverterDataContext() : 
				base(global::FiresecService.Properties.Settings.Default.FiresecConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public FiresecDbConverterDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FiresecDbConverterDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FiresecDbConverterDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FiresecDbConverterDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Journal> Journal
		{
			get
			{
				return this.GetTable<Journal>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Journal")]
	public partial class Journal : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private System.Nullable<System.DateTime> _DeviceTime;
		
		private System.Nullable<System.DateTime> _SystemTime;
		
		private string _ZoneName;
		
		private string _Description;
		
		private string _DeviceName;
		
		private string _PanelName;
		
		private string _DeviceDatabaseId;
		
		private string _PanelDatabaseId;
		
		private string _UserName;
		
		private System.Nullable<int> _StateType;
		
		private System.Nullable<int> _OldId;
		
		private System.Nullable<int> _DeviceCategory;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDeviceTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnDeviceTimeChanged();
    partial void OnSystemTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnSystemTimeChanged();
    partial void OnZoneNameChanging(string value);
    partial void OnZoneNameChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnDeviceNameChanging(string value);
    partial void OnDeviceNameChanged();
    partial void OnPanelNameChanging(string value);
    partial void OnPanelNameChanged();
    partial void OnDeviceDatabaseIdChanging(string value);
    partial void OnDeviceDatabaseIdChanged();
    partial void OnPanelDatabaseIdChanging(string value);
    partial void OnPanelDatabaseIdChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnStateTypeChanging(System.Nullable<int> value);
    partial void OnStateTypeChanged();
    partial void OnOldIdChanging(System.Nullable<int> value);
    partial void OnOldIdChanged();
    partial void OnDeviceCategoryChanging(System.Nullable<int> value);
    partial void OnDeviceCategoryChanged();
    #endregion
		
		public Journal()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeviceTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> DeviceTime
		{
			get
			{
				return this._DeviceTime;
			}
			set
			{
				if ((this._DeviceTime != value))
				{
					this.OnDeviceTimeChanging(value);
					this.SendPropertyChanging();
					this._DeviceTime = value;
					this.SendPropertyChanged("DeviceTime");
					this.OnDeviceTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SystemTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> SystemTime
		{
			get
			{
				return this._SystemTime;
			}
			set
			{
				if ((this._SystemTime != value))
				{
					this.OnSystemTimeChanging(value);
					this.SendPropertyChanging();
					this._SystemTime = value;
					this.SendPropertyChanged("SystemTime");
					this.OnSystemTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ZoneName", DbType="NVarChar(MAX)")]
		public string ZoneName
		{
			get
			{
				return this._ZoneName;
			}
			set
			{
				if ((this._ZoneName != value))
				{
					this.OnZoneNameChanging(value);
					this.SendPropertyChanging();
					this._ZoneName = value;
					this.SendPropertyChanged("ZoneName");
					this.OnZoneNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(MAX)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeviceName", DbType="NVarChar(MAX)")]
		public string DeviceName
		{
			get
			{
				return this._DeviceName;
			}
			set
			{
				if ((this._DeviceName != value))
				{
					this.OnDeviceNameChanging(value);
					this.SendPropertyChanging();
					this._DeviceName = value;
					this.SendPropertyChanged("DeviceName");
					this.OnDeviceNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PanelName", DbType="NVarChar(MAX)")]
		public string PanelName
		{
			get
			{
				return this._PanelName;
			}
			set
			{
				if ((this._PanelName != value))
				{
					this.OnPanelNameChanging(value);
					this.SendPropertyChanging();
					this._PanelName = value;
					this.SendPropertyChanged("PanelName");
					this.OnPanelNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeviceDatabaseId", DbType="NVarChar(MAX)")]
		public string DeviceDatabaseId
		{
			get
			{
				return this._DeviceDatabaseId;
			}
			set
			{
				if ((this._DeviceDatabaseId != value))
				{
					this.OnDeviceDatabaseIdChanging(value);
					this.SendPropertyChanging();
					this._DeviceDatabaseId = value;
					this.SendPropertyChanged("DeviceDatabaseId");
					this.OnDeviceDatabaseIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PanelDatabaseId", DbType="NVarChar(MAX)")]
		public string PanelDatabaseId
		{
			get
			{
				return this._PanelDatabaseId;
			}
			set
			{
				if ((this._PanelDatabaseId != value))
				{
					this.OnPanelDatabaseIdChanging(value);
					this.SendPropertyChanging();
					this._PanelDatabaseId = value;
					this.SendPropertyChanged("PanelDatabaseId");
					this.OnPanelDatabaseIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(MAX)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StateType", DbType="Int")]
		public System.Nullable<int> StateType
		{
			get
			{
				return this._StateType;
			}
			set
			{
				if ((this._StateType != value))
				{
					this.OnStateTypeChanging(value);
					this.SendPropertyChanging();
					this._StateType = value;
					this.SendPropertyChanged("StateType");
					this.OnStateTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OldId", DbType="Int")]
		public System.Nullable<int> OldId
		{
			get
			{
				return this._OldId;
			}
			set
			{
				if ((this._OldId != value))
				{
					this.OnOldIdChanging(value);
					this.SendPropertyChanging();
					this._OldId = value;
					this.SendPropertyChanged("OldId");
					this.OnOldIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeviceCategory", DbType="Int")]
		public System.Nullable<int> DeviceCategory
		{
			get
			{
				return this._DeviceCategory;
			}
			set
			{
				if ((this._DeviceCategory != value))
				{
					this.OnDeviceCategoryChanging(value);
					this.SendPropertyChanging();
					this._DeviceCategory = value;
					this.SendPropertyChanged("DeviceCategory");
					this.OnDeviceCategoryChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
