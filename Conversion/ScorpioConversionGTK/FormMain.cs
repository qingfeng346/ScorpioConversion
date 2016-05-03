using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using Gtk;
using ScorpioConversion;
namespace ScorpioConversion {
	public partial class FormMain: Gtk.Window {
		private Thread m_Thread;
		public FormMain () : base (Gtk.WindowType.Toplevel) {
			Build ();
			m_Thread = new Thread (ThreadFunc);
			m_Thread.Start ();
		}
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			m_Thread.Abort ();
			Application.Quit ();
			a.RetVal = true;
		}
		void ThreadFunc() {
			while (true) {
				Thread.Sleep (10);
				Application.Invoke ((sender, e) => {
					Tick();
				});
			}
		}
		protected void OnToggleCode (object sender, EventArgs e) {
			CheckBox ();
		}
		protected void OnToggleLanguage (object sender, EventArgs e) {
			CheckBox ();
		}
		protected void OnToggleSpawn (object sender, EventArgs e) {
			CheckBox ();
		}
		private void CheckBox() {
			this.CodeBox.Visible = this.ToggleCode.Active;
			this.LanguageBox.Visible = this.ToggleLanguage.Active;
			this.SpawnBox.Visible = this.ToggleSpawn.Active;
		}
		protected void OnClickTinyPNG (object sender, EventArgs e)
		{
			FormTiny window = new FormTiny ();
			window.Show ();
		}
	}
}