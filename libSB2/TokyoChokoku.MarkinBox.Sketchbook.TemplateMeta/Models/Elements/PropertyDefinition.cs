using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public class PropertyDefinition
	{
		public StoreTypeDefinition StoreType { get; }


		public string Name { get; }


		public string StoreTypeName {
			get { return StoreType.Name; }
		}

		public string ContentTypeName {
			get { return StoreType.ContentType.Name; }
		}

        public ValidationCategories Categories { get; }


        public PropertyDefinition (StoreTypeDefinition storeType, string name, ValidationCategories categories)
		{
			StoreType = storeType;
			Name = name;
            Categories = categories;
		}
	}
}

