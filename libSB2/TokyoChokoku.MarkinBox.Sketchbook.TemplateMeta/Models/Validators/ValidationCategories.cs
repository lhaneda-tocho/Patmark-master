using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;


namespace TokyoChokoku.MarkinBox.Sketchbook.TemplateMeta
{
    public class ValidationCategories : IEnumerable<ValidationCategory>
    {
        private readonly HashSet<ValidationCategory> set;

        public static ValidationCategories Create (params ValidationCategory [] categories)
        {
            return new ValidationCategories (new HashSet<ValidationCategory> (categories.AsEnumerable ()));
        }

        private ValidationCategories (HashSet<ValidationCategory> set)
        {
            this.set = set;
        }

        public bool IsNoValidation {
            get {
                return set.Count () == 0;
            }
        }

        public int Count {
            get {
                return set.Count ();
            }
        }

        public bool OfText {
            get {
                return set.Contains (ValidationCategory.Text);
            }
        }

        public bool OfGeometry {
            get {
                return set.Contains (ValidationCategory.Geometry);
            }
        }

        public bool OfMarking {
            get {
                return set.Contains (ValidationCategory.Marking);
            }
        }


        public IEnumerator<ValidationCategory> GetEnumerator ()
        {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return set.GetEnumerator ();
        }
    }
}

