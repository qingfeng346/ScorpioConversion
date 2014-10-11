using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XML_Conversion
{
    public partial class FormLanguage : Form
    {
        public FormLanguage()
        {
            InitializeComponent();
        }
        private void FormLanguage_Load(object sender, EventArgs e)
        {
            Util.Bind(this.textAll, ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
            Util.Bind(this.textTranslation, ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig);
            Util.Bind(this.textLanguage, ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig);
        }
    }
}
