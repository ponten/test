using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.IO;
using System.Data.OracleClient;
using CProcess.Enums;

namespace CProcess
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        public static string g_sUserID;
        public string g_sProgram, g_sFunction;
        DataSet dsTemp;
        public string g_sOrderField, g_sOrderDetailField;

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;
            g_sOrderDetailField = TableDefine.gsDef_DtlOrderField;

            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();

            //
            combShow.SelectedIndex = 0;
            combDetailShow.SelectedIndex = 0;
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Filter - Master
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;
            //Filter - Detail
            combDetailFilter.Items.Clear();
            combDetailFilter.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                combDetailFilter.Items.Add(TableDefine.tGridDetailField[i].sCaption);
                combDetailFilterField.Items.Add(TableDefine.tGridDetailField[i].sFieldName);
            }
            if (combDetailFilter.Items.Count > 0)
                combDetailFilter.SelectedIndex = 0;

            Check_Privilege();
        }
        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);

            btnDetailAppend.Enabled = btnAppend.Enabled;
            btnDetailModify.Enabled = btnModify.Enabled;
            btnDetailEnabled.Enabled = btnEnabled.Enabled;
            btnDetailDisabled.Enabled = btnDisabled.Enabled;
            btnDetailDelete.Enabled = btnDelete.Enabled;
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);

            ShowData();
        }

        public void ShowData()
        {
            sSQL = $" SELECT * FROM {TableDefine.gsDef_Table} ";
            if (combShow.SelectedIndex == 0)
            {
                sSQL += " WHERE UPPER(TRIM(ENABLED)) = 'Y' ";
            }
            else if (combShow.SelectedIndex == 1)
            {
                sSQL += " WHERE UPPER(TRIM(ENABLED)) = 'N' ";
            }
            else
            {
                sSQL += " WHERE UPPER(TRIM(ENABLED)) <> 'DROP' ";
            }

            if (combFilter.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(editFilter.Text))
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();

                sSQL += $" AND {sFieldName} LIKE '{editFilter.Text.Trim()}%' ";
            }

            sSQL += $" ORDER BY {g_sOrderField} ";

            var d = ClientUtils.ExecuteSQL(sSQL);

            gvData.DataSource = d;

            gvData.DataMember = d.Tables[0].ToString();

            //欄位Title  
            for (int i = 0; i <= gvData.Columns.Count - 1; i++)
            {
                gvData.Columns[i].Visible = false;
            }

            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridField[i].sFieldName;

                if (gvData.Columns.Contains(sGridField))
                {
                    gvData.Columns[sGridField].HeaderText = TableDefine.tGridField[i].sCaption;
                    gvData.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                    gvData.Columns[sGridField].Visible = true;
                }
            }

            gvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            gvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            gvData.Focus();
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;
                sEnabled = "Y";
            }

            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sSQL = " Update " + TableDefine.gsDef_Table + " "
                 + " set Enabled = '" + sEnabled + "'  "
                 + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                 + "    ,UPDATE_TIME = SYSDATE  "
                 + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);

            ShowData();
        }

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();

            editFilter.Focus();
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = new fData(this);
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            fData f = new fData();
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sformText = btnModify.Text;
                f.dataCurrentRow = gvData.CurrentRow;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            try
            {
                string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();

                //此Stage下有Process時,不可刪除
                sSQL = " Select * from Sajet.sys_process "
                     + " where stage_id = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sErrMsg1 = SajetCommon.SetLanguage("has one or more Processes", 1);
                    string sErrMsg2 = SajetCommon.SetLanguage("Can not delete it", 1);
                    SajetCommon.Show_Message(sData + " " + sErrMsg1 + Environment.NewLine
                                            + sErrMsg2 + "!", 0);
                    return;
                }
                //此Stage下有Terminal,不可刪除
                sSQL = " Select * from Sajet.sys_terminal "
                     + " where stage_id = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sErrMsg1 = SajetCommon.SetLanguage("has one or more Terminals", 1);
                    string sErrMsg2 = SajetCommon.SetLanguage("Can not delete it", 1);
                    SajetCommon.Show_Message(sData + " " + sErrMsg1 + Environment.NewLine
                                            + sErrMsg2 + "!", 0);
                    return;
                }

                //=====                
                string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                sSQL = $@"
UPDATE {TableDefine.gsDef_Table}
   SET Enabled = 'DROP'  
      ,UPDATE_USERID = '{g_sUserID}'
      ,UPDATE_TIME = SYSDATE 
 WHERE {TableDefine.gsDef_KeyField} = '{sID}'
";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sID);

                ShowData();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        public static void CopyToHistory(string sID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_HTTable + " "
                        + " Select * from " + TableDefine.gsDef_Table + " "
                        + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            if (sender == btnExport)
                Export.ExportToExcel(gvData);
            else if (sender == btnDetailExport)
                Export.ExportToExcel(gvDetail);
        }

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            sSQL = "";
            DataGridView dvControl = (DataGridView)contextMenuStrip1.SourceControl;
            if (dvControl == gvData)
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;
                string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                sSQL = TableDefine.History_SQL(sFieldID);
            }
            else if (dvControl == gvDetail)
            {
                if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                    return;
                string sFieldID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                sSQL = TableDefine.DetailHistory_SQL(sFieldID);
            }

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            fHistory fHistory = new fHistory();
            fHistory.dgvHistory.DataSource = dsTemp;
            fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();

            for (int i = 0; i <= fHistory.dgvHistory.Columns.Count - 1; i++)
            {
                string sGridField = fHistory.dgvHistory.Columns[i].HeaderText;
                string sField = "";
                for (int j = 0; j <= dvControl.Columns.Count - 1; j++)
                {
                    sField = dvControl.Columns[j].Name;
                    if (sGridField == sField)
                    {
                        fHistory.dgvHistory.Columns[i].HeaderText = dvControl.Columns[j].HeaderText;
                        break;
                    }
                }
            }

            fHistory.ShowDialog();
            fHistory.Dispose();
        }

        private void gvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                ShowDetailData();
            }
        }

        public void ShowDetailData()
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null) return;

            string sStageID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            sSQL = $@"
SELECT
    A.PROCESS_ID,
    A.PROCESS_NAME,
    A.PROCESS_DESC,
    A.PROCESS_DESC2,
    A.PROCESS_CODE,
    B.TYPE_NAME,
    A.WIP_ERP,
    NVL(C.OPTION1, 'N') ""{ColumnNameEnum.PrintWaterProofLabel}"",
    NVL(C.OPTION2, 'N') ""{ColumnNameEnum.UseT4Stove}"",
    NVL(C.OPTION3, 'N') ""{ColumnNameEnum.UseT6Stove}"",
    NVL(C.OPTION4, 'N') ""{ColumnNameEnum.StartingProcessOf10C}"",
    NVL(C.OPTION5, 'N') ""{ColumnNameEnum.WarehousingProcessOf10C}"",
    TRIM(D.MACHINE_TYPE_NAME)
    || ' '
    || TRIM(D.MACHINE_TYPE_DESC) ""{ColumnNameEnum.UsingEquipment}"",
    E.DEPT_NAME ""{ColumnNameEnum.ProductionUnit}""
FROM
    SAJET.SYS_PROCESS          A,
    SAJET.SYS_OPERATE_TYPE     B,
    SAJET.SYS_PROCESS_OPTION   C,
    SAJET.SYS_MACHINE_TYPE     D,
    SAJET.SYS_DEPT             E
WHERE
    A.STAGE_ID = :STAGE_ID
    AND A.OPERATE_ID = B.OPERATE_ID (+)
    AND A.PROCESS_ID = C.PROCESS_ID (+)
    AND C.OPTION6 = TO_CHAR(D.MACHINE_TYPE_ID(+))
    AND C.OPTION7 = TO_CHAR(E.DEPT_ID(+))
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", sStageID },
            };

            if (combDetailShow.SelectedIndex == 0)
            {
                sSQL += " AND UPPER(TRIM(a.ENABLED)) = 'Y' ";
            }
            else if (combDetailShow.SelectedIndex == 1)
            {
                sSQL += " AND UPPER(TRIM(a.ENABLED)) = 'N' ";
            }
            else
            {
                sSQL += " AND UPPER(TRIM(a.ENABLED)) <> 'DROP' ";
            }

            if (combDetailFilter.SelectedIndex > -1 && string.IsNullOrWhiteSpace(editDetailFilter.Text))
            {
                string sFieldName = combDetailFilterField.Items[combDetailFilter.SelectedIndex].ToString();
                sSQL += $" AND {sFieldName} LIKE :PARAM_1 || '%' ";

                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAM_1", editDetailFilter.Text.Trim() });
            }

            sSQL += " order by process_name ";

            var d = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            gvDetail.DataSource = d;
            gvDetail.DataMember = d.Tables[0].ToString();

            foreach (DataGridViewColumn column in gvDetail.Columns)
            {
                column.Visible = column.Name.ToUpper() != "PROCESS_ID";

                column.HeaderText = SajetCommon.SetLanguage(column.Name);

                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            gvDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            gvDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            gvDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            ShowDetailData();

            gvDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            gvData.Focus();
        }

        private void combDetailShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDetailDelete.Visible = (combDetailShow.SelectedIndex == 1);
            btnDetailDisabled.Visible = (combDetailShow.SelectedIndex == 0);
            btnDetailEnabled.Visible = (combDetailShow.SelectedIndex == 1);

            ShowDetailData();

            gvDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void editDetailFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowDetailData();

            gvDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            editDetailFilter.Focus();
        }

        public static void CopyToDetailHistory(string sID)
        {
            string sSQL = $@"
INSERT INTO {TableDefine.gsDef_DtlHTTable}
    SELECT
        *
    FROM
        {TableDefine.gsDef_DtlTable}
    WHERE
        {TableDefine.gsDef_DtlKeyField} = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);
        }

        private void btnDetailAppend_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 ||
                gvData.CurrentRow == null)
            {
                return;
            }

            using (fDetailData f = new fDetailData
            {
                g_sUpdateType = "APPEND",
                g_sformText = btnAppend.Text,
                g_sStageID = gvData.CurrentRow.Cells["STAGE_ID"].Value.ToString(),
                g_sStageName = gvData.CurrentRow.Cells["STAGE_NAME"].Value.ToString()
            })
            {
                var dr = f.ShowDialog();

                ShowDetailData();

                gvDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
        }

        private void btnDetailModify_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 ||
                gvDetail.CurrentRow == null)
            {
                return;
            }

            int row_index = gvDetail.CurrentRow.Index;

            using (fDetailData f = new fDetailData
            {
                g_sUpdateType = "MODIFY",
                g_sformText = btnModify.Text,
                dataCurrentRow = gvDetail.CurrentRow,
                g_sStageID = gvData.CurrentRow.Cells["STAGE_ID"].Value.ToString(),
                g_sStageName = gvData.CurrentRow.Cells["STAGE_NAME"].Value.ToString()
            })
            {
                var dr = f.ShowDialog();

                ShowDetailData();

                gvDetail.CurrentCell = gvDetail.Rows[row_index].Cells[gvDetail.FirstDisplayedCell.ColumnIndex];
            }
        }

        private void btnDetailDisabled_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                return;

            string sID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
            string sType = "";
            string sEnabled = "";
            if (sender == btnDetailDisabled)
            {
                sType = btnDetailDisabled.Text;
                sEnabled = "N";
            }
            else if (sender == btnDetailEnabled)
            {
                sType = btnDetailEnabled.Text;
                sEnabled = "Y";
            }

            string sData = gvDetail.Columns[TableDefine.gsDef_DtlKeyData].HeaderText + " : " + gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyData].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sSQL = " Update " + TableDefine.gsDef_DtlTable + " "
                 + " set Enabled = '" + sEnabled + "'  "
                 + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                 + "    ,UPDATE_TIME = SYSDATE  "
                 + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToDetailHistory(sID);

            ShowDetailData();

            gvDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void btnDetailDelete_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                return;

            try
            {
                string sID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                string sData = gvDetail.Columns[TableDefine.gsDef_DtlKeyData].HeaderText + " : " + gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyData].Value.ToString();

                //此Process下有Terminal,不可刪除
                sSQL = $@"
SELECT
    *
  FROM SAJET.SYS_TERMINAL
 WHERE PROCESS_ID = '{sID}'
";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sErrMsg1 = SajetCommon.SetLanguage("has one or more Terminals", 1);
                    string sErrMsg2 = SajetCommon.SetLanguage("Can not delete it", 1);
                    SajetCommon.Show_Message(sData + " " + sErrMsg1 + Environment.NewLine
                                            + sErrMsg2 + "!", 0);
                    return;
                }

                //=====                
                string sMsg = btnDetailDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;
                sSQL = $@"
UPDATE {TableDefine.gsDef_DtlTable}
   SET Enabled = 'DROP'  
      ,UPDATE_USERID = '{g_sUserID}'
      ,UPDATE_TIME = SYSDATE 
 WHERE {TableDefine.gsDef_DtlKeyField} = '{sID}'
";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToDetailHistory(sID);

                sSQL = $" DELETE SAJET.SYS_RC_PROCESS_SHEET WHERE PROCESS_ID = '{sID}' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                ShowDetailData();

                gvDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }
    }
}