﻿//------------------------------------------------------------------------------
// <auto-generated>
//	 Этот код создан программой.
//	 Исполняемая версия:4.0.30319.1026
//
//	 Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//	 повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SKDDriver.Properties {
	
	
	[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
		
		private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
		
		public static Settings Default {
			get {
				return defaultInstance;
			}
		}
		
		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
		[global::System.Configuration.DefaultSettingValueAttribute("Data Source=.\\sqlexpress;Initial Catalog=master;Integrated Security=True;Language" +
			"=\'English\'")]
		public string ConnectionString {
			get {
				return ((string)(this["ConnectionString"]));
			}
		}
		
		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
		[global::System.Configuration.DefaultSettingValueAttribute("Data Source=.\\sqlexpress;Initial Catalog=SKD;Integrated Security=True;Language=\'E" +
			"nglish\'")]
		public string SKDConnectionString {
			get {
				return ((string)(this["SKDConnectionString"]));
			}
		}
		
		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
		[global::System.Configuration.DefaultSettingValueAttribute("Data Source=.\\sqlexpress;Initial Catalog=passjournal;Integrated Security=True")]
		public string passjournalConnectionString {
			get {
				return ((string)(this["passjournalConnectionString"]));
			}
		}
		
		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
		[global::System.Configuration.DefaultSettingValueAttribute("Data Source=02-KBP-NIO-0524\\firesecinstance;Initial Catalog=PassJournal_0;Integra" +
			"ted Security=True")]
		public string PassJournal_0ConnectionString {
			get {
				return ((string)(this["PassJournal_0ConnectionString"]));
			}
		}
		
		[global::System.Configuration.ApplicationScopedSettingAttribute()]
		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		[global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
		[global::System.Configuration.DefaultSettingValueAttribute("Data Source=.\\firesecinstance;Initial Catalog=Journal_1;Integrated Security=True")]
		public string Journal_1ConnectionString {
			get {
				return ((string)(this["Journal_1ConnectionString"]));
			}
		}
	}
}
