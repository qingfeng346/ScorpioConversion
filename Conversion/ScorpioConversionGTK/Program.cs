using System;
using Gtk;

namespace ScorpioConversion
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			FormMain win = new FormMain ();
			win.Show ();
			Application.Run ();
		}
	}
}
