﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:2.0.50727.3053
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace Firesec.Models.DeviceCustomFunctions
{


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
	public class requests
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("request", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public requestsRequest[] request;
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class requestsRequest
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("param", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public requestsParam[] param;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int id;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int state;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int paramCount;
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class requestsParam
	{
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public int value;
	}
}
