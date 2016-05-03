using System;

namespace ScorpioConversion
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class FoldWidget : Gtk.Bin
	{
		private string mName = "";
		public FoldWidget ()
		{
			this.Build ();
			mName = this.Toggle.Label;
			UpdateName ();
		}
		public void AddWidget(Gtk.Widget widget) {
			this.ToggleBox.Add (widget);
			this.ToggleBox.ShowAll ();
			this.ToggleBox.Visible = this.Toggle.Active;
		}
		public void SetName(string name) {
			mName = name;
			UpdateName ();
		}
		private void UpdateName() {
			this.Toggle.Label = (Toggle.Active ? "▼" : "▶") + " " + mName;
		}
		protected void OnToggled (object sender, EventArgs e)
		{
			this.ToggleBox.Visible = this.Toggle.Active;
			UpdateName ();
		}
	}
}

