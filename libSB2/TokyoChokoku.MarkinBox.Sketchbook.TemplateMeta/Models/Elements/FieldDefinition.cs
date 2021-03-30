using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public abstract class FieldDefinition
	{
		static ReadOnlyCollection<FieldDefinition> Empty { get; } = new List <FieldDefinition> ().AsReadOnly ();

        public string Name { get; }
		public ParameterDefinition Parameter { get; }

        virtual
		public ReadOnlyCollection<FieldDefinition> Changable {
			get {
				return Empty;
			}
		}


        public FieldDefinition (string name)
		{
			this.Name = name;
			this.Parameter = BuildParameter (
				ParameterDefinition.CreateBuilder(name + "Parameter")
			);
		}


		protected abstract ParameterDefinition BuildParameter (ParameterDefinition.Builder builder);
	}
}

