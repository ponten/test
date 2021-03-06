using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Text.RegularExpressions;//導入命名空間(正則表達式)

namespace PartAQL
{
    public partial class fAddItem : Form
    {
        public string sItem, sInputType;
        public fAddItem()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            sItem = editItem.Text;

            if (sInputType == "1")
            {
                if (IsNumeric(sItem))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Item Value in Number"), 0);
                    return;
                }
            }
            DialogResult = DialogResult.Yes;
        }

        private void fAddItem_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        //定義一個函數,作用:判斷strNumber是否為數字,是數字返回True,不是數字返回False
        public bool IsNumeric(string strNumber)
        {
            Regex NumberPattern = new Regex("[^0-9.-]");
            return NumberPattern.IsMatch(strNumber);
        }
    }
}