using System;
using System.Collections.Immutable;
using System.Linq;

namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public sealed class ValidationResult
    {
        public static ValidationResult Empty { get; } = new ValidationResult();
        readonly ImmutableList<Element> errors;


        public static Builder CreateBuilder () {
            return new Builder ();
        }

        public ImmutableList<string> ErrorMessages { 
            get {
                return errors.Select ((obj) => {
                    return obj.Message;
                }).ToImmutableList ();
            }
        }

        public ImmutableList<ValidationError> ErrorCodes {
            get {
                return errors.Select ((obj) => {
                    return obj.ErrorCode;
                }).ToImmutableList ();
            }
        }

        public bool HasError {
            get {
                return errors.Count > 0;
            }
        }


        ValidationResult (ImmutableList <Element> errors)
        {
            this.errors = errors;
        }


        ValidationResult ()
        {
            errors = ImmutableList.Create <Element> ();
        }


        public void FireEvent (ModificationListener listeners, object sender)
        {
            if (listeners == null)
                return;

            listeners (this, sender);
        }

        public void FireEventIf ( bool predicate, ModificationListener listeners, object sender) {
            if (listeners == null || !predicate)
                return;

            listeners (this, sender);
        }

        public ValidationResult Merge (ValidationResult result)
        {
            var builder = ImmutableList.CreateBuilder<Element> ();
            builder.AddRange (errors);
            builder.AddRange (result.errors);
            return new ValidationResult (builder.ToImmutable ());
        }

        public ValidationResult SelectCategory (ValidationCategory category)
        {
            return new ValidationResult (errors.Where ((obj) => {
                    return obj.Category == category;
                }).ToImmutableList ());
        }

        public sealed class Builder {
            readonly
            ImmutableList <Element> .Builder errors = ImmutableList.CreateBuilder <Element> ();

            internal Builder() {}

            public void PutError(ValidationCategory category, ValidationError error, string message) {
                errors.Add (new Element (message, category, error));
            }

            public ValidationResult Create() {
                var errorList =  errors.ToImmutable ();
                return new ValidationResult (errorList);
            }
        }

        sealed class Element
        {
            public string Message { get; }
            public ValidationCategory Category { get; }
            public ValidationError ErrorCode { get; }


            public Element (string message, ValidationCategory category, ValidationError errorCode)
            {
                Message = message;
                Category = category;
                ErrorCode = errorCode;
            }
        }
    }
}

