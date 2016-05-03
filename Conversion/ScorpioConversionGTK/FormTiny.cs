using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Collections.Generic;
namespace ScorpioConversion {
	public partial class FormTiny : Gtk.Window {
		public FormTiny () : base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			Init ();
		}
	}
}


