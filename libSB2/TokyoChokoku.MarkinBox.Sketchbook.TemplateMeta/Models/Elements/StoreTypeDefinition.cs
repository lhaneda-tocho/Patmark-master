using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public class StoreTypeDefinition : TypeDefinition
	{
		public TypeDefinition ContentType;



		public StoreTypeDefinition (string name, string contentTypeName) : base(name)
		{
			this.ContentType = new TypeDefinition (contentTypeName);
		}
	}
}

