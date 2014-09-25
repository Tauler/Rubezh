﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FiresecAPI.GK;
using System.Linq;

namespace FiresecAPI.Automation
{
	[DataContract]
	public class Variable
	{
		public Variable()
		{
			Uid = Guid.NewGuid();
			DefaultExplicitValue = new ExplicitValue();
			ExplicitValue = new ExplicitValue();
			DefaultExplicitValues = new List<ExplicitValue>();
			ExplicitValues = new List<ExplicitValue>();
		}

		public Variable(string name) : base()
		{
			Name = name;
		}

		[DataMember]
		public Guid Uid { get; set; }

		[DataMember]
		public bool IsList { get; set; }

		[DataMember]
		public ExplicitType ExplicitType { get; set; }

		[DataMember]
		public ObjectType ObjectType { get; set; }

		[DataMember]
		public EnumType EnumType { get; set; }
		
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public bool IsGlobal { get; set; }

		[DataMember]
		public ExplicitValue DefaultExplicitValue { get; set; }

		[DataMember]
		public ExplicitValue ExplicitValue { get; set; }

		[DataMember]
		public List<ExplicitValue> ExplicitValues { get; set; }

		[DataMember]
		public List<ExplicitValue> DefaultExplicitValues { get; set; }

		public void ResetValue()
		{
			PropertyCopy.Copy<ExplicitValue, ExplicitValue>(DefaultExplicitValue, ExplicitValue);
			ExplicitValues = new List<ExplicitValue>();
			foreach (var defaultExplicitValue in DefaultExplicitValues)
			{
				var newExplicitValue = new ExplicitValue();
				PropertyCopy.Copy<ExplicitValue, ExplicitValue>(defaultExplicitValue, newExplicitValue);
				ExplicitValues.Add(newExplicitValue);
			}
		}

		public void CopyValue(Argument argument, Procedure callingProcedure, List<Variable> globalVariables)
		{
			ExplicitValues = new List<ExplicitValue>();
			if (argument.VariableScope == VariableScope.ExplicitValue)
			{
				PropertyCopy.Copy<ExplicitValue, ExplicitValue>(argument.ExplicitValue, ExplicitValue);				
				foreach (var explicitValue in argument.ExplicitValues)
				{
					var newExplicitValue = new ExplicitValue();
					PropertyCopy.Copy<ExplicitValue, ExplicitValue>(explicitValue, newExplicitValue);
					ExplicitValues.Add(newExplicitValue);
				}
			}
			else
			{
				var variable = new Variable();
				if (argument.VariableScope == VariableScope.LocalVariable)
				{
					if (callingProcedure == null)
						return;
					var allLocalVariables = new List<Variable>(callingProcedure.Variables);
					allLocalVariables.AddRange(new List<Variable>(callingProcedure.Arguments));
					variable = allLocalVariables.FirstOrDefault(x => x.Uid == argument.VariableUid);
				}
				if (argument.VariableScope == VariableScope.GlobalVariable) //TODO
				{
					if (globalVariables == null)
						return;
					variable = globalVariables.FirstOrDefault(x => x.Uid == argument.VariableUid);
				}

				PropertyCopy.Copy<ExplicitValue, ExplicitValue>(variable.ExplicitValue, ExplicitValue);
				foreach (var explicitValue in variable.ExplicitValues)
				{					
					var newExplicitValue = new ExplicitValue();
					PropertyCopy.Copy<ExplicitValue, ExplicitValue>(explicitValue, newExplicitValue);
					ExplicitValues.Add(newExplicitValue);
				}
			}
		}
	}
}