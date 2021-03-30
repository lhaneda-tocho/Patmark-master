using System;
using System.Drawing;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
	public enum MessageBoxResult
	{
		None = 0,
		OK,
		Cancel,
		Yes,
		No
	}

	public enum MessageBoxButtons
	{
		OK = 0,
		OKCancel,
		YesNo,
		YesNoCancel
	}

	/// <summary>
	/// メッセージ ボックスを表示します。メッセージ ボックスには、テキスト、ボタンを格納できます。
	/// </summary>
	public static class MessageBox
	{
		/// <summary>
		/// Message box with text, caption and buttons. 
		/// </summary>
		public static MessageBoxResult Show(string text, string caption, MessageBoxButtons buttons)
		{
			MessageBoxResult result = MessageBoxResult.Cancel;
			bool IsDisplayed = false;
			nint buttonClicked = -1;
			MessageBoxButtons button = buttons;
			UIAlertView alert = null;

			var cancelButton = "Cancel";
			var otherButtons = new string[]{};

			switch (button)
			{
			case MessageBoxButtons.OK:
				cancelButton = string.Empty;
				otherButtons = new [] { "OK" };
				break;

			case MessageBoxButtons.OKCancel:
				otherButtons = new [] { "OK" };
				break;

			case MessageBoxButtons.YesNo:
				cancelButton = string.Empty;
				otherButtons = new [] { "Yes", "No" };
				break;

			case MessageBoxButtons.YesNoCancel:
				otherButtons = new [] { "Yes", "No" };
				break;
			}

			if (cancelButton != string.Empty)
			{
				alert = new UIAlertView(caption, text, null, cancelButton, otherButtons);
			}
			else
			{
				alert = new UIAlertView(caption, text, null, null, otherButtons);
			}

			alert.Canceled += (sender, e) => {
				buttonClicked = 0;
				IsDisplayed = false;
			};

			alert.Clicked += (sender, e) => {
				buttonClicked = e.ButtonIndex;
				IsDisplayed = false;
			};

			alert.Dismissed += (sender, e) => {
				if (IsDisplayed)
				{
					buttonClicked = e.ButtonIndex;
					IsDisplayed = false;
				}
			};

			alert.Show();

			IsDisplayed = true; // このフラグがfalseになるとループから抜ける

			while (IsDisplayed)
			{
				NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow(0.2));
			}

			switch (button)
			{
			case MessageBoxButtons.OK:
				result = MessageBoxResult.OK;
				break;

			case MessageBoxButtons.OKCancel:
				if (buttonClicked == 1)
					result = MessageBoxResult.OK;
				break;

			case MessageBoxButtons.YesNo:
				if (buttonClicked == 0)
					result = MessageBoxResult.Yes;
				else
					result = MessageBoxResult.No;
				break;

			case MessageBoxButtons.YesNoCancel:
				if (buttonClicked == 1)
					result = MessageBoxResult.Yes;
				else if (buttonClicked == 2)
					result = MessageBoxResult.No;
				break;
			}

			return result;
		}

		/// <summary>
		/// Message box with text.
		/// </summary>
		public static MessageBoxResult Show(string text)
		{
			return Show(text, string.Empty, MessageBoxButtons.OK);
		}

		/// <summary>
		/// Message box with text and caption.
		/// </summary>
		public static MessageBoxResult Show(string text, string caption)
		{
			return Show(text, caption, MessageBoxButtons.OK);
		}
	}
}