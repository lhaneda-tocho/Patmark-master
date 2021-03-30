using System;
using UIKit;

using TokyoChokoku.MarkinBox.Sketchbook.Properties.Stores;
using TokyoChokoku.MarkinBox.Sketchbook.Connections;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	partial class CalendarSettingsShiftController : UIViewController
	{
		public CalendarSettingsShiftController (IntPtr handle) : base (handle)
		{
		}

        public CalendarShiftStore ShiftStore { get; set; }

        private Connection<char> CodeConnection;
        private Connection<byte> StartingHourConnection;
        private Connection<byte> StartingMinuteConnection;
        private Connection<byte> EndingHourConnection;
        private Connection<byte> EndingMinuteConnection;

        override
        public void ViewDidLoad()
        {
            this.NavigationItem.Title = Title;

            // コード
            //SetUpConnection(
            //    ShiftStore.CodeStore,
            //    char.TryParse,
            //    CodeTextField,
            //    "[a-z|A-Z|1-9]+"
            //);
            CodeConnection = new Connection<char>(
                ShiftStore.CodeStore,
                char.TryParse,
                (result, sender) => { },
                (result, sender) => { }
            );
            CodeTextField.Text = new System.Text.RegularExpressions.Regex("[a-z|A-Z|1-9]+").Match(
                CodeConnection.Content.ToString()
            ).Value;
            CodeTextField.EditingDidEnd += (object sender, EventArgs e) =>
            {
                CodeConnection.TrySet(CodeTextField.Text);
            };

            // 開始時刻(時)
            StartingHourConnection = new Connection<byte>(
                ShiftStore.StartingHourStore,
                byte.TryParse,
                (result, sender) => { },
                (result, sender) => { }
            );
            StartHourTextField.Text = StartingHourConnection.Content.ToString();
            StartHourTextField.EditingDidEnd += (object sender, EventArgs e) =>
            {
                StartingHourConnection.TrySet(StartHourTextField.Text);
            };

            // 開始時刻(分)
            StartingMinuteConnection = new Connection<byte>(
                ShiftStore.StartingMinuteStore,
                byte.TryParse,
                (result, sender) => { },
                (result, sender) => { }
            );
            StartMinuteTextField.Text = StartingMinuteConnection.Content.ToString();
            StartMinuteTextField.EditingDidEnd += (object sender, EventArgs e) =>
            {
                StartingMinuteConnection.TrySet(StartMinuteTextField.Text);
            };

            // 終了時刻(時)
            EndingHourConnection = new Connection<byte>(
                ShiftStore.EndingHourStore,
                byte.TryParse,
                (result, sender) => { },
                (result, sender) => { }
            );
            EndHourTextField.Text = EndingHourConnection.Content.ToString();
            EndHourTextField.EditingDidEnd += (object sender, EventArgs e) =>
            {
                EndingHourConnection.TrySet(EndHourTextField.Text);
            };

            // 終了時刻(分)
            EndingMinuteConnection = new Connection<byte>(
                ShiftStore.EndingMinuteStore,
                byte.TryParse,
                (result, sender) => { },
                (result, sender) => { }
            );
            EndMinuteTextField.Text = EndingMinuteConnection.Content.ToString();
            EndMinuteTextField.EditingDidEnd += (object sender, EventArgs e) =>
            {
                EndingMinuteConnection.TrySet(EndMinuteTextField.Text);
            };

            //
            ClearButton.TouchDown += (object sender, EventArgs e) =>
            {
                Clear();
            };

            //
            //OKButton.TouchDown += (object sender, EventArgs e) =>
            //{
            //    Commit();
            //};
            OKButton.Hidden = true;
        }


        //private Connection<T> SetUpConnection<T>(Store<T> store, StringAnalizer<T> analizer, UITextField textField, string regex)
        //{
        //    var con = new Connection<T>(
        //        store,
        //        analizer,
        //        (result, sender) => { },
        //        (result, sender) => { }
        //    );
        //    textField.Text = new System.Text.RegularExpressions.Regex(regex).Match(
        //        con.Content.ToString()
        //    ).Value;
        //    textField.EditingDidEnd += (object sender, EventArgs e) =>
        //    {
        //        con.TrySet(textField.Text);
        //    };
        //    return con;
        //}

        private void Clear()
        {
            ForceSetString(CodeTextField, this.CodeConnection, "\0");
            ForceSetString(StartHourTextField, this.StartingHourConnection, "0");
            ForceSetString(StartMinuteTextField, this.StartingMinuteConnection, "0");
            ForceSetString(EndHourTextField, this.EndingHourConnection, "0");
            ForceSetString(EndMinuteTextField, this.EndingMinuteConnection, "0");
        }

        private void ForceSetString<T>(UITextField textField, Connection<T> con, string val)
        {
            if (con.TrySet(val))
            {
                textField.Text = val;
            }
        }

        //private bool Commit()
        //{
        //    return (
        //        CodeConnection.TrySet(CodeTextField.Text) &&
        //        StartingHourConnection.TrySet(StartHourTextField.Text) &&
        //        StartingMinuteConnection.TrySet(StartMinuteTextField.Text) &&
        //        EndingHourConnection.TrySet(EndHourTextField.Text) &&
        //        EndingMinuteConnection.TrySet(EndMinuteTextField.Text)
        //    );
        //}


        override
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisconnectAll();
                base.Dispose();
            }
        }

        private void DisconnectAll()
        {
            if (CodeConnection != null)
            {
                CodeConnection.Disconnect();
            }
            if (StartingHourConnection != null)
            {
                StartingHourConnection.Disconnect();
            }
            if (StartingMinuteConnection != null)
            {
                StartingMinuteConnection.Disconnect();
            }
            if (EndingHourConnection != null)
            {
                EndingHourConnection.Disconnect();
            }
            if (EndingMinuteConnection != null)
            {
                EndingMinuteConnection.Disconnect();
            }
        }
	}
}
