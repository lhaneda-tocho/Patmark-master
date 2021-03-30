using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
	public class ParameterDefinition
	{
		public string Name { get; }
		public ReadOnlyCollection<PropertyDefinition> PropertyDefinitions { get; }


        private ParameterDefinition (string name, List<PropertyDefinition> storeDefs)
		{
			this.Name = name;
			PropertyDefinitions = storeDefs.AsReadOnly();
		}


		public static Builder CreateBuilder(string name) {
			return new Builder (name);
		}








		public sealed class Builder {
			private readonly string name;
			private readonly List<PropertyDefinition> storeDefinitions = new List<PropertyDefinition>();


			public Builder(string name) {
				this.name = name;
			}


			public ParameterDefinition Build () {
				return new ParameterDefinition (name, storeDefinitions);
			}



            public Builder AddText (string propertyName) {
                var storeType = MetaStores.Text;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry,
                        ValidationCategory.Text
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddMode (string propertyName) {
                var storeType = MetaStores.Mode;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddPrmFl (string propertyName) {
                var storeType = MetaStores.PrmFl;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddId (string propertyName) {
                var storeType = MetaStores.Id;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddX (string propertyName) {
                var storeType = MetaStores.X;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddY (string propertyName) {
                var storeType = MetaStores.Y;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddHeight (string propertyName) {
                var storeType = MetaStores.Height;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddPitch (string propertyName) {
                var storeType = MetaStores.Pitch;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddAspect (string propertyName) {
                var storeType = MetaStores.Aspect;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddAngle (string propertyName) {
                var storeType = MetaStores.Angle;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddArcRadius (string propertyName) {
                var storeType = MetaStores.ArcRadius;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddMirrored (string propertyName) {
                var storeType = MetaStores.Mirrored;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddSpeed (string propertyName) {
                var storeType = MetaStores.Speed;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Marking
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


            public Builder AddJogging(string propertyName)
            {
                var storeType = MetaStores.Jogging;

                var propertyDef = new PropertyDefinition(
                    storeType: storeType,
                    name: propertyName,
                    categories: ValidationCategories.Create()
                );

                storeDefinitions.Add(propertyDef);

                return this;
            }


            public Builder AddPause (string propertyName)
            {
                var storeType = MetaStores.Pause;

                var propertyDef = new PropertyDefinition (
                    storeType: storeType,
                    name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Marking
                    ));

                storeDefinitions.Add (propertyDef);

                return this;
            }


			public Builder AddDensity (string propertyName) {
                var storeType = MetaStores.Density;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}

			public Builder AddPower (string propertyName) {
                var storeType = MetaStores.Power;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Marking
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}

            public Builder AddReverse (string propertyName, params ValidationCategory [] categories)
            {
                var storeType = MetaStores.Reverse;

                var propertyDef = new PropertyDefinition (
                    storeType: storeType,
                    name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Marking
                    ));

                storeDefinitions.Add (propertyDef);

                return this;
            }

			public Builder AddHostVersion (string propertyName) {
                var storeType = MetaStores.HostVersion;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}

			public Builder AddZDepth (string propertyName) {
                var storeType = MetaStores.ZDepth;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddBasePoint (string propertyName) {
                var storeType = MetaStores.BasePoint;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}


			public Builder AddDotCount (string propertyName) {
                var storeType = MetaStores.DotCount;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}

			public Builder AddTime (string propertyName) {
                var storeType = MetaStores.Time;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}

			public Builder AddIsBezierCurve (string propertyName, params ValidationCategory [] categories) {
                var storeType = MetaStores.IsBezierCurve;

				var propertyDef = new PropertyDefinition (
					storeType: storeType,
					name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

				storeDefinitions.Add (propertyDef);

				return this;
			}

            public Builder AddFont (string propertyName, params ValidationCategory [] categories)
            {
                var storeType = MetaStores.Font;

                var propertyDef = new PropertyDefinition (
                    storeType: storeType,
                    name: propertyName,
                    categories: ValidationCategories.Create (
                        ValidationCategory.Geometry
                    ));

                storeDefinitions.Add (propertyDef);

                return this;
            }
		}
	}
}

