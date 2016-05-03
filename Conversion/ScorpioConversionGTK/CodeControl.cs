using System;

namespace ScorpioConversion {
	[System.ComponentModel.ToolboxItem (true)]
	public partial class CodeControl : Gtk.Bin
	{
		//private PROGRAM m_Program;
		public CodeControl () {
			this.Build ();
		}
//		public void SetProgram(PROGRAM program) {
//			m_Program = program;
//			LanguageName.LabelProp = "===========" + program.ToString() + "===========";
//			ConversionUtil.Bind (CodePath, m_Program, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
//			ConversionUtil.Bind (DataPath, m_Program, ConfigKey.DataDirectory, ConfigFile.PathConfig);
//			ConversionUtil.Bind (CheckCreate, m_Program, ConfigKey.Create, ConfigFile.PathConfig);
//			ConversionUtil.Bind (CheckCompress, m_Program, ConfigKey.Compress, ConfigFile.PathConfig);
//		}
		private void SetProgram_impl() {
			LanguageName.LabelProp = "===========" + m_Program.ToString() + "===========";
			ShowAll ();
		}
	}
}
