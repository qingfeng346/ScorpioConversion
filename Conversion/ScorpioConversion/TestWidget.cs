using System;

namespace ScorpioConversion
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class TestWidget : Gtk.Bin
	{
		public TestWidget ()
		{
			this.Build ();
		}

		protected void OnClickButton (object sender, EventArgs e)
		{
			languagewidget1.Visible = !languagewidget1.Visible;
		}
	}
}

