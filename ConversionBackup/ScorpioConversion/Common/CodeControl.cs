namespace ScorpioConversion {
    public partial class CodeControl {
        private PROGRAM m_Program;
        public void SetProgram(PROGRAM program) {
            m_Program = program;
            ConversionUtil.Bind(CodePath, m_Program, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
            ConversionUtil.Bind(DataPath, m_Program, ConfigKey.DataDirectory, ConfigFile.PathConfig);
            ConversionUtil.Bind(CheckCreate, m_Program, ConfigKey.Create, ConfigFile.PathConfig);
            ConversionUtil.Bind(CheckCompress, m_Program, ConfigKey.Compress, ConfigFile.PathConfig);
            SetProgram_impl();
        }
    }
}
