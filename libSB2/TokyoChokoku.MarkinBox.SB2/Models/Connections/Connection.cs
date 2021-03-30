using System;
using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Validators;


namespace TokyoChokoku.MarkinBox.Sketchbook.Connections
{
	public class Connection<Type>
	{
		private Store<Type> 				store;
		private StringAnalizer<Type>       	analizer;
		private ModificationListener	errorListener;
		private ModificationListener	modifiedListener;

        public StringAnalizer<Type> Analizer { get; }

		public Type Content {
			get {
				return store.Content;
			}

			set {
				store.Content = value;
			}
		}


		public Connection (
			Store<Type> store,
			StringAnalizer<Type> analizer,
			ModificationListener errorListener,
			ModificationListener modifiedListener) {

			this.store = store;
			this.analizer = analizer;
			this.errorListener = errorListener;
			this.modifiedListener = modifiedListener;

			store.OnFailure += errorListener;
			store.OnModified += modifiedListener;
		}


		public bool TrySet(string str) {
			Type value;
			if (analizer (str, out value)) {
				store.Content = value;
				return true;
			} else {
				return false;
			}
		}


		public void Connect () {
			if (store != null) {
				store.OnFailure += errorListener;
				store.OnModified += modifiedListener;
			} else {
				throw new InvalidOperationException ();
			}
		}

		public void Disconnect () {
			if (store != null) {
				store.OnFailure -= errorListener;
				store.OnModified -= modifiedListener;
			}
		}

		public void Dispose () {
			if (store != null) {
				store.OnFailure -= errorListener;
				store.OnModified -= modifiedListener;
			}
		}
	}
}

