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
namespace Firesec.Models.Metadata
{


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
	public partial class config {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("class", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public classType[] @class;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("drv", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public drvType[] drv;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string clsid;
	}
	
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class classType {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("parent", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public parentType[] parent;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("param", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public paramType[] param;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string clsid;
	}
	
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class parentType {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string clsid;
	}
	
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class stateType {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string id;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string code;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string nameSource;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string @class;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string affectChildren;

		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string AffectedParent;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string manualReset;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string primaryState;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string CanResetOnPanel;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string type;
	}
	
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class paramInfoType {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string type;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string @default;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string editType;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string nameSource;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string caption;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string helpIndex;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string hint;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string paramID;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string min;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string max;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string tslen;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string hidden;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string showOnlyInState;
	}
	
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class propInfoType {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("param", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public paramType[] param;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string type;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string @default;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string editType;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string caption;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string helpIndex;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string hint;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string paramID;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string min;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string max;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string tslen;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string hidden;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string showOnlyInState;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string blockName;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string shiftInMemory;
	}
	
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class paramType {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string type;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string value;
	}
	
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class drvType {
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("propInfo", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public propInfoType[] propInfo;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("paramInfo", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public paramInfoType[] paramInfo;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("state", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public stateType[] state;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string id;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string clsid;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string options;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string maxZoneCardinality;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string minZoneCardinality;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string addrGroup;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string parentInAddr;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string shortName;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string acr_enabled;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string acr_from;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string acr_to;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ar_enabled;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ar_from;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ar_to;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ar_no_addr;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string cat;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string caseCnt;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string childAddrMask;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string validChars;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string addrMask;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string dev_icon;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string id_alias;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string lim_parent;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string altIntf;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string addrPrefix;
		
		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string res_addr;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string child_id;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string child_count;
	}
}