using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;            
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Data.OracleClient;
using SajetClass;
using SajetFilter;
namespace IQCbyLot
{
    public partial class fMain : Form
    {
        public string g_sExeName;
        string g_sFileName = Path.GetFileNameWithoutExtension(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileName);
        string g_sProgram, g_sFunction;
        string g_sUserID;
        string sSQL;
        DataSet dsTemp;
        string g_sIniFile = Application.StartupPath + "\\Sajet.ini";
        int g_iPrivilege;
        string g_sSampleLevel = "正常,加嚴,減量,免驗";
        public string S1;
        public string S2;
        string VendorList;
        string VendorId;
        string tMessage;
        int sNowSamplingLevel = 0;
        string sNowSamplingID = string.Empty, sNowTypeID = string.Empty, sNowSamplingType = string.Empty;
        int sMsgCount = 0;
        bool sMsgKeypass = false;
        bool g_bCheckTool = false;
        bool g_bChangAQL = false;
        public struct TSampleType
        {
            public String sSamplingID;
            public String sSamplingType;
            public String sSamplingLevelID;
            public String sSamplingLevelName;
            public int iSampleQty;
            public int iCriticalRej;
            public int iMajorRej;
            public int iMinorRej;
            public String sSamplingUnit;
        }

        //        public static TSampleType G_rSampleType = new TSampleType();

        public struct TIQCLot
        {
            public string sLotNo;
            public bool bLotInput;
            public string sVendorID;
            public string sVendorNane;
            public string sPartID;
            public string sVendorCode;
            public string sPartNo;
            public string sPartVer;
            public string sPartSpec;
            public string sStartTime;
            public string sTargetTime;
            public int iLotSize;
            public string sHoldFlag;
            public string sPO;
            public string sUrgentType;
            public string[] lstSampleLevel;
            public bool bRoHSOn;
            public bool bRoHSFinish;
           
            public bool bTypeFinish;
            public string sSelTypeName;
            public string sSelTypeID;
            public int iSelTypeSeq;
            public string sRECID;
            public string sPARTTYPE;
            public string sSampleRuleID;
            public string sQCResult;
            public string sQCResultCode;
            public string sDateCode;
            public string sVer;
            public string sSpecA; 
            public string sSpecB;
            public string sMName;
            public string sUrgentHint;
            public string sDelayHold;
            public bool bCriticalPart;
            public string sRDFlag;
            public string sPartPic1;
            public string sPartPic2;
            public string sPartPic3;
            public string sPartPic4;
            public string sPartPic5;
            public string sPartPic6;
        }

        public struct TDefect
        {
            public String sDefectID;
            public string sDefectCode;
            public string sDefectLevel;
            public string sDefectDesc;
        }

        public TIQCLot G_rIQC = new TIQCLot();

        public fMain()
        {
            InitializeComponent();
        }

        private void checkprivilege()
        {
            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
            sbtnPass.Enabled = (g_iPrivilege >= 1);
            sbtnWaive.Enabled = sbtnPass.Enabled;
            sbtnReject.Enabled = sbtnPass.Enabled;
            sbtnSorting.Enabled = sbtnPass.Enabled;
            btnRDAdmit.Enabled = sbtnPass.Enabled;
            sbtnHold.Visible = (ClientUtils.GetPrivilege(g_sUserID, "Hold Admin", g_sProgram) >= 1);
            g_bChangAQL = (ClientUtils.GetPrivilege(g_sUserID, "Change AQL", g_sProgram) >= 1);
        }

        private void clearData()
        {
            dgvItemType.Rows.Clear();
            dgvSampleType.Rows.Clear();
            dgvDefect.Rows.Clear();
            dgRT.Rows.Clear();

            editItemTypeName.Text = string.Empty;
            //changeSampleTypeToolStripMenuItem.Enabled = true;
            changeSampleTypeToolStripMenuItem.Enabled = g_bChangAQL;
           // dgvDefect.ContextMenuStrip = cmsdefect;
            G_rIQC.sLotNo = "N/A";
            G_rIQC.sPartID = "0";
            G_rIQC.sPartNo = "";
            G_rIQC.sPartSpec = "";
            G_rIQC.sVendorID = "0";
            G_rIQC.sVendorNane = "";
            G_rIQC.sPO = "";
            G_rIQC.sHoldFlag = "0";
            G_rIQC.sStartTime = "";
            G_rIQC.sTargetTime = "";
            G_rIQC.iLotSize = 0;
            G_rIQC.bLotInput = false;
            G_rIQC.sSelTypeID = "0";
            G_rIQC.iSelTypeSeq = -1;
            G_rIQC.sQCResult = "N/A";
            G_rIQC.sQCResultCode = "N/A";
            G_rIQC.sPARTTYPE = "N/A";
            G_rIQC.sUrgentType = "N";
            G_rIQC.bCriticalPart = false;
            G_rIQC.sSpecA = "";
            G_rIQC.sSpecB = "";
            G_rIQC.sMName = "";
            G_rIQC.sUrgentType = "";
            G_rIQC.sRDFlag = "N";
            lablPartNo.Text = G_rIQC.sPartNo + " - " + G_rIQC.sPartVer;
            lablSpec1.Text = G_rIQC.sPartSpec;
            lablVendor.Text = G_rIQC.sVendorNane;
            lablLotSize.Text = G_rIQC.iLotSize.ToString();
            lablPONo.Text = G_rIQC.sPO;
            lablStartTime.Text = G_rIQC.sStartTime;
            lablEndTime.Text = G_rIQC.sTargetTime;
            labResult.Text = G_rIQC.sQCResult;
            lablWareHouse.Text = "N/A";
            lablSpecA.Text = G_rIQC.sSpecA;
            lablSpecB.Text = G_rIQC.sSpecB;
            lablModelName.Text = G_rIQC.sMName;
            lablUrgentType.Text = G_rIQC.sUrgentType;
            lablCreateEmp.Text = "";
            lablCriticalPartType.Text = "";
            lablCriticalPartType.Visible = false;
            btnRDAdmit.Enabled = (G_rIQC.sRDFlag=="Y");
          //  panelCriticalPart.Visible = false;
            //ROHS ITEM
            dgvRoHSItem.Rows.Clear();
            PanelRoHS.Enabled = true;
            btnRoHSNG.Visible = PanelRoHS.Enabled;
            btnRoHSOK.Visible = PanelRoHS.Enabled;
            lablRoHSResult.Visible = (!PanelRoHS.Enabled);
            btnRollbackRoHs.Visible = lablRoHSResult.Visible;
            editRoHSMemo.Text = string.Empty;
            G_rIQC.bRoHSFinish = false;
            G_rIQC.bRoHSOn = false;
            lablRoHSOn.Visible = false;
            btWaiveResult.Visible = false;
            panelImg.BackgroundImage = null;
            ckbUrgent.Checked = false;
            G_rIQC.sSpecA = ""; 
            G_rIQC.sSpecB = "";
            G_rIQC.sMName = "";
            dgvSN.Rows.Clear();
        }

        public bool getIQCLotData(string sLotNo) // 取得Lot No基本資料
        {
            G_rIQC.bLotInput = false;

            string sSQL2 = "";
            DataSet dsTemp2 = new DataSet();

            string sUrgent_Type = "";
            string sCalendar_Day = "";
            int sUrgent_Db_Day = 0;
            int sUrgent_Day = 0;
            int sUrgent_Day_Now = 0;
            int sEnd_Time;
            int sDate_Now;
            int sDif_Day;

            bool flag = true;

           


            sSQL = "SELECT A.LOT_NO,B.PART_NO, A.VERSION, B.SPEC1, B.SPEC2, C.VENDOR_NAME,C.VENDOR_CODE, A.LOT_SIZE " //20110916增加B.SPEC2欄位
                + "      ,A.PO_NO, TO_CHAR(A.START_TIME, 'YYYY/MM/DD HH24:MI:SS') START_TIME "
                //+ "      ,A.PO_NO, TO_CHAR(A.START_TIME + " + sUrgent_Day + ", 'YYYY/MM/DD') END_TIME "
                + "      ,A.PASS_QTY, A.FAIL_QTY, A.RECEIVE_QTY, A.PART_ID, A.VENDOR_ID "
                + "      ,A.HOLD_FLAG, SAJET.Inspection_Result(A.QC_RESULT) QC_RESULT, A.QC_RESULT QC_RESULT_CODE, A.DATECODE, A.MATERIAL_VER "
                + "      ,A.URGENT_TYPE "
                + "      ,E.WAREHOUSE_NAME "
                + "      ,F.MODEL_NAME "
                + "      ,A.INSP_LOT, A.URGENT_HINT " //2012/02/15增加INSP_LOT、URGENT_HINT兩個欄位
                + "      ,A.DELAY_HOLD "
                + "      ,NVL(A.RD_FLAG,'N') RD_FLAG "
                + "  FROM SAJET.G_IQC_LOT A "
                + "      ,SAJET.SYS_PART B "
                + "      ,SAJET.SYS_VENDOR C "
                + "      ,SAJET.G_ERP_RT_ITEM D "
                + "      ,SAJET.SYS_WAREHOUSE E "
                + "      ,SAJET.SYS_MODEL F "
                + " WHERE A.LOT_NO = :LOT_NO "
                + "   AND A.PART_ID = B.PART_ID "
                + "   AND A.VENDOR_ID = C.VENDOR_ID(+) "
                + "   AND A.RT_ID = D.RT_ID "
                + "   AND A.RT_SEQ = D.RT_SEQ "
                + "   AND D.LOCATION = E.WAREHOUSE_ID(+) "
                + "   AND B.MODEL_ID = F.MODEL_ID(+) "
                + "   AND ROWNUM = 1 ";

            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    G_rIQC.bLotInput = true;
                    G_rIQC.sLotNo = ds.Tables[0].Rows[0]["LOT_NO"].ToString();
                    G_rIQC.sPartID = ds.Tables[0].Rows[0]["PART_ID"].ToString();
                    G_rIQC.sPartSpec = ds.Tables[0].Rows[0]["SPEC1"].ToString();
                    G_rIQC.sPartNo = ds.Tables[0].Rows[0]["PART_NO"].ToString();
                    G_rIQC.sPartVer = ds.Tables[0].Rows[0]["VERSION"].ToString();
                    G_rIQC.sVendorID = ds.Tables[0].Rows[0]["VENDOR_ID"].ToString();
                    G_rIQC.sVendorNane = ds.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
                    G_rIQC.sVendorCode = ds.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
                    G_rIQC.iLotSize = Convert.ToInt32(ds.Tables[0].Rows[0]["LOT_SIZE"].ToString());
                    G_rIQC.sPO = ds.Tables[0].Rows[0]["PO_NO"].ToString();
                    G_rIQC.sStartTime = ds.Tables[0].Rows[0]["START_TIME"].ToString();
                    //G_rIQC.sEndTime = ds.Tables[0].Rows[0]["TARGET_TIME"].ToString();

                    G_rIQC.sHoldFlag = ds.Tables[0].Rows[0]["HOLD_FLAG"].ToString();
                    G_rIQC.sQCResultCode = ds.Tables[0].Rows[0]["QC_RESULT_CODE"].ToString();                 //檢驗結果代碼
                    G_rIQC.sQCResult = SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["QC_RESULT"].ToString()); //檢驗結果
                    G_rIQC.sDateCode = ds.Tables[0].Rows[0]["DATECODE"].ToString();
                    G_rIQC.sVer = ds.Tables[0].Rows[0]["MATERIAL_VER"].ToString();
                    G_rIQC.sUrgentType = ds.Tables[0].Rows[0]["URGENT_TYPE"].ToString();
                    lablWareHouse.Text = ds.Tables[0].Rows[0]["WAREHOUSE_NAME"].ToString();
                    //-----------MAX-20110916增加SPEC1、SPEC2與MODLE NAME欄位值顯示----------
                    G_rIQC.sSpecA = ds.Tables[0].Rows[0]["SPEC1"].ToString();
                    G_rIQC.sSpecB = ds.Tables[0].Rows[0]["SPEC2"].ToString();
                    G_rIQC.sMName = ds.Tables[0].Rows[0]["MODEL_NAME"].ToString();
                    //-----------Sharon 20120215增加兩個欄位以判斷是否要驗與要控管急件-------
                    G_rIQC.sUrgentHint = ds.Tables[0].Rows[0]["URGENT_HINT"].ToString();
                    G_rIQC.sDelayHold = ds.Tables[0].Rows[0]["DELAY_HOLD"].ToString();
                    G_rIQC.sRDFlag = ds.Tables[0].Rows[0]["RD_FLAG"].ToString();

                }
                //如不需控管急件中間日期與燈號就先隱藏
                if (G_rIQC.sUrgentHint == "Y")
                {
                    CheckLotUrgent(G_rIQC.sUrgentType);
                    label4.Visible = true;
                    lablEndTime.Visible = true;
                    ckbUrgent.Visible = true;
                    panelImg.Visible = true;
                }
                else
                {
                    label4.Visible = false;
                    lablEndTime.Visible = false;
                    ckbUrgent.Visible = false;
                    panelImg.Visible = false;
                }
                rbtnExceptionNo.Checked = true;
                if (G_rIQC.sDelayHold == "Y")
                    rbtnExceptionYes.Checked = true;
                return G_rIQC.bLotInput;
            }
            finally
            {
                ds.Dispose();
            }
        }

        private bool CheckLotUrgent(string sUrgentType)
        {
            /*
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TENDDATE", "" };
            Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_GET_STD_ENDTIME", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult, 0);
                return false;
            }
             */
            
            panelImg.Size = new Size(35, 30);
            panelImg.BackgroundImageLayout = ImageLayout.Stretch;
            string sFileName = "";
            sSQL = "SELECT TO_CHAR(TARGET_TIME,'YYYY/MM/DD HH24:MI:SS') TARGET_TIME "
                  +"      ,TRUNC((SYSDATE-NVL(TARGET_TIME,SYSDATE))*24*60*60) DELAY_STATUS "
                  +"  FROM SAJET.G_IQC_LOT  "
                  + " WHERE LOT_NO =:LOT_NO AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            string sEndDate = dsTemp.Tables[0].Rows[0]["TARGET_TIME"].ToString();
            int iFlag = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["DELAY_STATUS"].ToString());
            if (iFlag >= 0) // 超過檢驗完成日期，紅色標示
            {
                sFileName = "NG.png";
            }
            else
            {
                if (sUrgentType == "N")
                {
                    sFileName = "OK.png";//未超過檢驗完成日期，非急單，綠色標示
                }
                else
                {
                    sFileName = "WARNING.png";// 未超過檢驗完成日期，急單，黃色標示
                }
            }
            if (File.Exists(Application.StartupPath + "\\" + g_sExeName + "\\" + sFileName))
                panelImg.BackgroundImage = Image.FromFile(Application.StartupPath + "\\" + g_sExeName + "\\" + sFileName);

            G_rIQC.sTargetTime = sEndDate;
            lablEndTime.Text = sEndDate;
            return true;
        }
        private void getVendorId(string g_sVendorCode,ref string g_sVendorId)
        {
            string sSQL = "SELECT VENDOR_CODE,VENDOR_ID FROM SAJET.SYS_VENDOR "
                        + "WHERE VENDOR_CODE = '" + g_sVendorCode +"' ";
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);
            g_sVendorId = ds.Tables[0].Rows[0][1].ToString();
        }
        private void getIQCLot() //LOT NO 下拉選單
        {
            combLotNo.SelectedIndex = -1;
            combItemSeq.SelectedIndex = -1;
            combLot.SelectedIndex = -1;
            combLotNo.Items.Clear();
            combItemSeq.Items.Clear();
            combItemSeq1.Items.Clear();
            combLot.Items.Clear();

            ////透過sys_base出控管是否隱藏那些供應商單據不顯示
            //string sExceptionVendor = SajetCommon.GetSysBaseData(g_sProgram, "Exception Vendor", ref tMessage);
            //string[] ListInfo = sExceptionVendor.Split(',');
            //for (int i = 0; i < ListInfo.Length; i++)
            //{
            //    if (i == 0)
            //    {
                    
            //        getVendorId(ListInfo[i].ToString(),ref VendorId);
            //        VendorList = "'" + VendorId + "'";
            //    }
            //    else
            //    { VendorList = "," + "'" + VendorId + "'"; }                                
            //}
            //string g_sSQL = " AND VENDOR_ID NOT IN (" + VendorList + ") ";
            if (btERP.Visible)
            {
                sSQL = "Select distinct LOT_NO "
                    + " From SAJET.G_IQC_LOT "
                    + " Where FLAG_ERP < 2 "
                    + " AND INSP_LOT = 'Y' "; //2012/02/15 增加判斷是否要驗，不需驗就不顯示
                //if (ListInfo.Length > 0)
                //    sSQL = sSQL + g_sSQL;
                    sSQL = sSQL + " Order BY LOT_NO ";
            }
            else
            {
                sSQL = "Select distinct LOT_NO "
                    + " From SAJET.G_IQC_LOT "
                    + " Where QC_RESULT = 'N/A' "
                    + " AND INSP_LOT = 'Y' ";
                //if (ListInfo.Length > 0)
                //    sSQL = sSQL + g_sSQL;
                    sSQL = sSQL + " Order BY LOT_NO ";
            }

            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            foreach (DataRow row in dsTemp.Tables[0].Rows)
            {
                combLot.Items.Add(row["LOT_NO"].ToString());
                string sNo = Convert.ToString(row[0].ToString());

                if (sNo.IndexOf("-") > 0)
                {
                    string[] noList = sNo.Split('-');
                    sNo = noList[0];
                }

                if (combLotNo.Items.IndexOf(sNo) == -1)
                    combLotNo.Items.Add(sNo);
            }
            combLotNo.Focus();
        }

        private void GetDefaultSampleType() //沒有針對供應商設抽驗計畫，會從系統找一組預設的
        {
            object[][] Params = new object[7][];

            for (int i = 0; i <= dgvItemType.Rows.Count - 1; i++)
            {
                string sTypeID = dgvItemType.Rows[i].Cells["TYPE_ID"].Value.ToString();
                int iSampleLevel = Convert.ToInt32(dgvItemType.Rows[i].Cells["SAMPLE_LEVEL_ID"].Value.ToString());
                string sSampleID = dgvItemType.Rows[i].Cells["SAMPLING_ID"].Value.ToString();
                string sSampleType = dgvItemType.Rows[i].Cells["SAMPLE_TYPE"].Value.ToString();
                sNowSamplingLevel = iSampleLevel;
                sNowSamplingID = sSampleID;

                if (dgvItemType.Rows[i].Cells["EXIST"].Value.ToString() == "N" || sSampleType == "N/A") //沒有在IQC的表或沒有抽驗計畫
                {
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTESTTYPEID", sTypeID }; //測試大項ID
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", g_sUserID };
                    Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSAMPLINGPLAN", "" };
                    Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSAMPLINGLEVEL", "" };
                    Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSAMPLINGID", "" };
                    Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_GET_DEF_SAMPLETYPE_RULE", Params);
                    string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

                    if (sResult != "OK")
                    {
                        SajetCommon.Show_Message(sResult, 0);
                        return;
                    }
                    
                    
                    iSampleLevel = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["TSAMPLINGLEVEL"].ToString());
                    sSampleType = dsTemp.Tables[0].Rows[0]["TSAMPLINGPLAN"].ToString();
                    sSampleID = dsTemp.Tables[0].Rows[0]["TSAMPLINGID"].ToString();
                    sNowSamplingLevel = iSampleLevel;
                    sNowSamplingID = sSampleID;
                    sNowTypeID = sTypeID;
                    sNowSamplingType = sSampleType;
                    dgvItemType.Rows[i].Cells["SAMPLE_TYPE"].Value = dsTemp.Tables[0].Rows[0]["TSAMPLINGPLAN"].ToString();
                    dgvItemType.Rows[i].Cells["SAMPLING_ID"].Value = dsTemp.Tables[0].Rows[0]["TSAMPLINGID"].ToString();
                    dgvItemType.Rows[i].Cells["SAMPLE_LEVEL"].Value = G_rIQC.lstSampleLevel[iSampleLevel];
                    dgvItemType.Rows[i].Cells["SAMPLE_LEVEL_ID"].Value = iSampleLevel.ToString();
                    TSampleType rSampleType = getSamplingPlanRange(sSampleID, iSampleLevel.ToString(), G_rIQC.iLotSize);
                    dgvItemType.Rows[i].Cells["SAMPLE_SIZE"].Value = rSampleType.iSampleQty.ToString();
                }
            }
        }

        private bool GetPriorSamplingLevel(string sTypeID,ref int sPriorSampleLevel,ref string sPriorSampleID,ref string sPriorSampleType )
        {
            string gsSQL = " SELECT SAMPLING_PLAN_ID,SAMPLING_LEVEL FROM SAJET.G_IQC_SAMPLING_PLAN "
                         + " WHERE PART_ID = '" + G_rIQC.sPartID + "' "
                         + "   AND VENDOR_ID = '" + G_rIQC.sVendorID + "' "
                         + "   AND ITEM_TYPE_ID = '" + sTypeID + "' "
                         + "   AND ROWNUM = 1 ";
            DataSet ds = ClientUtils.ExecuteSQL(gsSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sPriorSampleID = ds.Tables[0].Rows[0]["SAMPLING_PLAN_ID"].ToString();
                sPriorSampleLevel = int.Parse(ds.Tables[0].Rows[0]["SAMPLING_LEVEL"].ToString());
                gsSQL = " SELECT SAMPLING_TYPE FROM SAJET.SYS_QC_SAMPLING_PLAN "
                      + " WHERE SAMPLING_ID = '" + sPriorSampleID + "' ";
                ds = ClientUtils.ExecuteSQL(gsSQL);
                sPriorSampleType = ds.Tables[0].Rows[0]["SAMPLING_TYPE"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        public TSampleType getSamplingPlanRange(string sSampleID, string sSampleLevel, int iLotSize) //根據批量抓出抽驗計畫的Detail
        {
            TSampleType rSampleType = new TSampleType();
            rSampleType.sSamplingID = sSampleID;
            rSampleType.iCriticalRej = 0;
            rSampleType.iMajorRej = 0;
            rSampleType.iMinorRej = 0;
            rSampleType.iSampleQty = 0;
            rSampleType.sSamplingType = "N/A";
            rSampleType.sSamplingLevelID = sSampleLevel;
            rSampleType.sSamplingLevelName = G_rIQC.lstSampleLevel[Convert.ToInt32(sSampleLevel)];
            rSampleType.sSamplingUnit = "Qty";
            sSQL = "SELECT SAMPLING_TYPE "
                  + "  FROM SAJET.SYS_QC_SAMPLING_PLAN "
                  + " WHERE SAMPLING_ID =:SAMPLING_ID "
                  + "   AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_ID", sSampleID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                rSampleType.sSamplingType = dsTemp.Tables[0].Rows[0]["SAMPLING_TYPE"].ToString();
            }

            sSQL = "SELECT A.SAMPLING_ID,A.SAMPLING_TYPE "
                + "       ,NVL(B.SAMPLE_SIZE,0) SAMPLE_SIZE ,NVL(B.CRITICAL_REJECT_QTY,0) CRITICAL_REJECT_QTY "
                + "       ,NVL(B.MAJOR_REJECT_QTY,0) MAJOR_REJECT_QTY ,NVL(B.MINOR_REJECT_QTY,0) MINOR_REJECT_QTY "
                + "       ,B.SAMPLING_LEVEL  "
                + "       ,NVL(B.SAMPLING_UNIT,'0') SAMPLING_UNIT "
                + " FROM SAJET.SYS_QC_SAMPLING_PLAN A  "
                + "     ,SAJET.SYS_QC_SAMPLING_PLAN_DETAIL B "
                + " WHERE A.SAMPLING_ID = :SAMPLING_ID "
                + "   AND A.SAMPLING_ID = B.SAMPLING_ID "
                + "   AND B.SAMPLING_LEVEL =:SAMPLING_LEVEL "
                + "   AND B.MIN_LOT_SIZE <= :LOT_SIZE1 "
                + "   AND B.MAX_LOT_SIZE >= :LOT_SIZE2 ";
            Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_ID", sSampleID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_LEVEL", sSampleLevel };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.Int32, "LOT_SIZE1", iLotSize };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "LOT_SIZE2", iLotSize };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                rSampleType.iSampleQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString());
                rSampleType.sSamplingUnit = "Qty";
                if (dsTemp.Tables[0].Rows[0]["SAMPLING_UNIT"].ToString() == "1")//使用%當作單位
                {
                    rSampleType.sSamplingUnit = "%";
                    Double dSampleSize = Convert.ToDouble(dsTemp.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString()); ;
                    Double iSampleSize = Math.Round(G_rIQC.iLotSize * (dSampleSize / 100));
                    rSampleType.iSampleQty = (int)iSampleSize;
                    rSampleType.sSamplingUnit = dsTemp.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString() + "%";
                }
                if (rSampleType.iSampleQty > iLotSize)
                {
                    rSampleType.iSampleQty = iLotSize;
                }
                rSampleType.iCriticalRej = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["CRITICAL_REJECT_QTY"].ToString());
                rSampleType.iMajorRej = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MAJOR_REJECT_QTY"].ToString());
                rSampleType.iMinorRej = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MINOR_REJECT_QTY"].ToString());          
            }
            return rSampleType;
        }

        private void GetSYSTestType()
        {
            //找出此料號+供應商定義的測試大項 (未有檢驗結果)
            dgvItemType.Rows.Clear();
            sSQL = "SELECT A.RECID,A.SAMPLING_RULE_ID "
                 + "  FROM SAJET.SYS_PART_IQC_VENDOR_RULE  A "
                 + " WHERE A.PART_ID =:PART_ID "
                 + "   AND A.VENDOR_ID =:VENDOR_ID "
                 + "   AND A.ENABLED='Y' ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", G_rIQC.sVendorID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = "SELECT A.RECID,A.SAMPLING_RULE_ID "
                     + "  FROM SAJET.SYS_PART_IQC_VENDOR_RULE  A "
                     + " WHERE A.PART_ID =:PART_ID "
                     + "   AND A.DEFAULT_FLAG ='Y' "
                     + "   AND A.ENABLED='Y' ";       //有設Default
                Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                //if (dsTemp.Tables[0].Rows.Count == 0)
                //    return;

                if (dsTemp.Tables[0].Rows.Count == 0) // 依照階別帶出測試大項
                {
                    sSQL = " SELECT B.PART_TYPE,B.SAMPLING_RULE_ID "
                         + " FROM   SAJET.SYS_PART A,SAJET.SYS_IQC_PARTTYPE_RULE B "
                         + " WHERE  A.MATERIAL_TYPE=B.PART_TYPE "
                         + " AND    A.PART_ID=:PART_ID "
                         + " AND    A.ENABLED='Y' ";
                    Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    if (dsTemp.Tables[0].Rows.Count == 0)
                        return;

                    G_rIQC.sPARTTYPE = dsTemp.Tables[0].Rows[0]["PART_TYPE"].ToString();
                    G_rIQC.sSampleRuleID = dsTemp.Tables[0].Rows[0]["SAMPLING_RULE_ID"].ToString(); //抽驗規則 

                    sSQL = " SELECT B.ITEM_TYPE_ID,D.ITEM_TYPE_CODE,D.ITEM_TYPE_NAME,C.SAMPLING_TYPE,B.SAMPLING_ID,"
                         + " 0 SAMPLE_LEVEL_ID,'N/A' QC_RESULT ,0 SAMPLE_SIZE,0 CHECK_QTY,0 PASS_QTY, 0 FAIL_QTY,'N' EXIST "
                         + " FROM   SAJET.SYS_PART A,SAJET.SYS_IQC_PARTTYPE_TESTTYPE B,SAJET.SYS_QC_SAMPLING_PLAN C,SAJET.SYS_TEST_ITEM_TYPE D"
                         + " WHERE  A.MATERIAL_TYPE=B.PART_TYPE AND B.SAMPLING_ID=C.SAMPLING_ID AND B.ITEM_TYPE_ID = D.ITEM_TYPE_ID "
                         + " AND    A.PART_ID ='" + G_rIQC.sPartID + "'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    G_rIQC.bTypeFinish = false;
                    dgvItemType.Rows.Clear();

                    for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow dr = dsTemp.Tables[0].Rows[i];

                        dgvItemType.Rows.Add();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["TYPE_CODE"].Value = dr["ITEM_TYPE_CODE"].ToString();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["TYPE_NAME"].Value = dr["ITEM_TYPE_NAME"].ToString();
//                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_TYPE"].Value = dr["SAMPLING_TYPE"].ToString();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_TYPE"].Value = "N/A";
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_LEVEL"].Value = G_rIQC.lstSampleLevel[0];
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_SIZE"].Value = dr["SAMPLE_SIZE"].ToString();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["PASS_QTY"].Value = dr["PASS_QTY"].ToString();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["FAIL_QTY"].Value = dr["FAIL_QTY"].ToString();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Value = dr["QC_RESULT"].ToString();
                        //Unvisible
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["TYPE_ID"].Value = dr["ITEM_TYPE_ID"].ToString();
                        //dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLING_ID"].Value = dr["SAMPLING_ID"].ToString();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLING_ID"].Value = "0";
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_LEVEL_ID"].Value = dr["SAMPLE_LEVEL_ID"].ToString();
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["EXIST"].Value = dr["EXIST"].ToString();  //是否已存在IQC資料表
                        dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SEQ"].Value = i.ToString(); //表格內的順序
                    }
                    G_rIQC.bTypeFinish = true;

                    return;
                }
            }

            G_rIQC.sRECID = dsTemp.Tables[0].Rows[0]["RECID"].ToString();
            G_rIQC.sSampleRuleID = dsTemp.Tables[0].Rows[0]["SAMPLING_RULE_ID"].ToString(); //抽驗規則

            sSQL = " SELECT B.ITEM_TYPE_ID,NVL(D.ITEM_TYPE_CODE,B.ITEM_TYPE_ID) ITEM_TYPE_CODE ,D.ITEM_TYPE_NAME "
                + "        ,NVL(C.SAMPLING_TYPE,'N/A') SAMPLING_TYPE ,NVL(C.SAMPLING_ID,'0') SAMPLING_ID,0 SAMPLE_LEVEL_ID  "
                + "        ,'N/A' QC_RESULT ,0 SAMPLE_SIZE,0 CHECK_QTY,0 PASS_QTY, 0 FAIL_QTY "
                + "        ,'N' EXIST "
                + "   FROM SAJET.SYS_PART_IQC_TESTTYPE B "
                + "       ,SAJET.SYS_QC_SAMPLING_PLAN  C "
                + "       ,SAJET.SYS_TEST_ITEM_TYPE D "
                + "  WHERE B.RECID = :RECID "
                + "    AND B.SAMPLING_ID = C.SAMPLING_ID(+) "
                + "    AND B.ITEM_TYPE_ID = D.ITEM_TYPE_ID(+) "
                + "  ORDER BY D.ITEM_TYPE_CODE ";

            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", G_rIQC.sRECID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            G_rIQC.bTypeFinish = false;
            dgvItemType.Rows.Clear();

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];

                dgvItemType.Rows.Add();
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["TYPE_CODE"].Value = dr["ITEM_TYPE_CODE"].ToString();
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["TYPE_NAME"].Value = dr["ITEM_TYPE_NAME"].ToString();
                //dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_TYPE"].Value = dr["SAMPLING_TYPE"].ToString();
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_TYPE"].Value = "N/A";
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_LEVEL"].Value = G_rIQC.lstSampleLevel[0];
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_SIZE"].Value = dr["SAMPLE_SIZE"].ToString();
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["PASS_QTY"].Value = dr["PASS_QTY"].ToString();
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["FAIL_QTY"].Value = dr["FAIL_QTY"].ToString();
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Value = dr["QC_RESULT"].ToString();
                //Unvisible
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["TYPE_ID"].Value = dr["ITEM_TYPE_ID"].ToString(); 
                //dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLING_ID"].Value = dr["SAMPLING_ID"].ToString(); 
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLING_ID"].Value = "0";
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SAMPLE_LEVEL_ID"].Value = dr["SAMPLE_LEVEL_ID"].ToString();
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["EXIST"].Value = dr["EXIST"].ToString();  //是否已存在IQC資料表
                dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["SEQ"].Value = i.ToString(); //表格內的順序
            }
            G_rIQC.bTypeFinish = true;
        }

        private void GetIQCTestType() //已有檢驗結果
        {
            sSQL = "SELECT A.ITEM_TYPE_ID,NVL(D.ITEM_TYPE_CODE,A.ITEM_TYPE_ID) ITEM_TYPE_CODE ,D.ITEM_TYPE_NAME  "
                  + "      ,NVL(C.SAMPLING_TYPE,'N/A') SAMPLE_TYPE,A.SAMPLING_PLAN_ID SAMPLE_ID,A.SAMPLING_LEVEL SAMPLE_LEVEL_ID "
                  + "      ,DECODE(A.QC_RESULT,'0','OK','1','NG','N/A') QC_RESULT "
                  + "      ,A.SAMPLING_SIZE SAMPLE_SIZE ,A.PASS_QTY + A.FAIL_QTY CHECK_QTY ,A.PASS_QTY,A.FAIL_QTY "
                  + "      ,'Y' EXIST "
                  + "  FROM SAJET.G_IQC_TEST_TYPE A "
                  + "  LEFT JOIN SAJET.SYS_QC_SAMPLING_PLAN C ON A.SAMPLING_PLAN_ID = C.SAMPLING_ID "
                  + "  LEFT JOIN SAJET.SYS_TEST_ITEM_TYPE D ON A.ITEM_TYPE_ID = D.ITEM_TYPE_ID "
                  + " WHERE A.LOT_NO =:LOT_NO ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                string sTypeID = dr["ITEM_TYPE_ID"].ToString();
                int iIndex = -1;

                for (int j = 0; j <= dgvItemType.Rows.Count - 1; j++)
                {
                    if (sTypeID == dgvItemType.Rows[j].Cells["TYPE_ID"].Value.ToString())
                    {
                        iIndex = j;
                        break;
                    }
                }
                /*mark by rita 2011/08/15
                if (iIndex == -1)
                {
                    dgvItemType.Rows.Add();
                    iIndex = dgvItemType.Rows.Count - 1;
                }
                 */ 

                if (iIndex > -1)
                {
                    dgvItemType.Rows[iIndex].Cells["TYPE_CODE"].Value = dr["ITEM_TYPE_CODE"].ToString();
                    dgvItemType.Rows[iIndex].Cells["TYPE_NAME"].Value = dr["ITEM_TYPE_NAME"].ToString();
                    dgvItemType.Rows[iIndex].Cells["SAMPLE_TYPE"].Value = dr["SAMPLE_TYPE"].ToString();
                    dgvItemType.Rows[iIndex].Cells["SAMPLE_LEVEL"].Value = G_rIQC.lstSampleLevel[Convert.ToInt32(dr["SAMPLE_LEVEL_ID"].ToString())];
                    dgvItemType.Rows[iIndex].Cells["SAMPLE_SIZE"].Value = dr["SAMPLE_SIZE"].ToString();
                    dgvItemType.Rows[iIndex].Cells["PASS_QTY"].Value = dr["PASS_QTY"].ToString();
                    dgvItemType.Rows[iIndex].Cells["FAIL_QTY"].Value = dr["FAIL_QTY"].ToString();
                    dgvItemType.Rows[iIndex].Cells["QC_RESULT"].Value = dr["QC_RESULT"].ToString();
                    //Unvisible
                    dgvItemType.Rows[iIndex].Cells["TYPE_ID"].Value = dr["ITEM_TYPE_ID"].ToString();
                    dgvItemType.Rows[iIndex].Cells["SAMPLING_ID"].Value = dr["SAMPLE_ID"].ToString();
                    dgvItemType.Rows[iIndex].Cells["SAMPLE_LEVEL_ID"].Value = dr["SAMPLE_LEVEL_ID"].ToString();
                    dgvItemType.Rows[iIndex].Cells["EXIST"].Value = dr["EXIST"].ToString();  //是否已存在IQC資料表
                    dgvItemType.Rows[iIndex].Cells["SEQ"].Value = iIndex.ToString(); //表格內的順序
                    if (dr["QC_RESULT"].ToString() == "OK")
                    {
                        dgvItemType.Rows[iIndex].Cells["QC_RESULT"].Style.BackColor = Color.Green;
                        dgvItemType.Rows[iIndex].Cells["QC_RESULT"].Style.ForeColor = Color.White;
                    }
                    if (dr["QC_RESULT"].ToString() == "NG")
                    {
                        dgvItemType.Rows[iIndex].Cells["QC_RESULT"].Style.BackColor = Color.Red;
                        dgvItemType.Rows[iIndex].Cells["QC_RESULT"].Style.ForeColor = Color.White;
                    }
                }
            }
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            //tabControl1.TabPages.Remove(tabControl1.TabPages[2]);
            //tabControl1.TabPages.Add(tabControl1.TabPages[2]);
            ckbUrgent.Checked = false;

            G_rIQC.lstSampleLevel = g_sSampleLevel.Split(',');
            SajetCommon.SetLanguageControl(this);
            //ClientUtils.SetLanguage(this, ClientUtils.fCurrentProject);
            g_sExeName = ClientUtils.fCurrentProject;
            fInspHistory.g_sExeName = g_sExeName;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sUserID = ClientUtils.UserPara1;
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            checkprivilege();  //檢查Pass Reject Waive按鈕權限
            clearData();
            DateInsp.Value = DateTime.Now; //檢驗日期
            string sMessage = string.Empty;
            //用來判斷Pass Reject Waive等按鈕是否顯示
            string sParamValue = SajetCommon.GetSysBaseData(g_sProgram, "IQC Result", ref sMessage); //8:上傳ERP按鈕

            if (!string.IsNullOrEmpty(sParamValue))
            {
                string S = sParamValue;
                
                /*
                string sSQL = "Select PARAM_VALUE "
                            + "  From SAJET.SYS_BASE "
                            + " WHERE PROGRAM='IQC' "
                            + "   AND PARAM_NAME = 'IQC Result' ";
                DataSet ds = ClientUtils.ExecuteSQL(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
             
                    string S = ds.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
                 * */
                sbtnPass.Visible = (S.IndexOf("0") >= 0);
                sbtnReject.Visible = (S.IndexOf("1") >= 0);
                sbtnWaive.Visible = (S.IndexOf("2") >= 0);
                sbtnByPass.Visible = (S.IndexOf("4") >= 0);
                sbtnPartialWaive.Visible = (S.IndexOf("6") >= 0);
                sbtnSorting.Visible = (S.IndexOf("3") >= 0);
                // sbtnHold.Visible = (S.IndexOf("5") >= 0);
                sbtnSpecialWaive.Visible = (S.IndexOf("7") >= 0);
                btERP.Visible = (S.IndexOf("8") >= 0);
            }
            else
            {
                sbtnPass.Visible = true;
                sbtnReject.Visible = true;
                sbtnWaive.Visible = true;
                sbtnByPass.Visible = false;
                sbtnPartialWaive.Visible = false;
                sbtnSorting.Visible = false;
                sbtnHold.Visible = false;
                sbtnSpecialWaive.Visible = false;
                btERP.Visible = false;
            }

            getIQCLot();    //Lot No下拉選單
            sbtnPass.BackColor = Color.Green;
            sbtnReject.BackColor = Color.Red;
            sbtnWaive.BackColor = Color.Yellow;
            sbtnByPass.BackColor = Color.Blue;
            sbtnSpecialWaive.BackColor = Color.Orange;
            sbtnSorting.BackColor = Color.Purple;
            btnRDAdmit.BackColor = Color.SteelBlue;

            sbtnPass.ForeColor = Color.White;
            sbtnReject.ForeColor = Color.White;
            sbtnWaive.ForeColor = Color.Black;
            sbtnByPass.ForeColor = Color.White;
            sbtnSpecialWaive.ForeColor = Color.White;
            sbtnSorting.ForeColor = Color.White;
            btnRDAdmit.ForeColor = Color.White;

            //用來判斷Start/Stop RoHS按鈕是否顯示

            string sParamValue2 = SajetCommon.GetSysBaseData(g_sProgram, "IQC RoHS", ref sMessage);
            if (!string.IsNullOrEmpty(sParamValue2))
            {
                string S = sParamValue2;
                btnStartRoHS.Visible = (S.ToString() == "Y");
                btnStopRoHS.Visible = btnStartRoHS.Visible;
            }
            else
            {
                btnStartRoHS.Visible = false;
                btnStopRoHS.Visible = false;
            }
            string sFilePath = SajetCommon.GetSysBaseData(g_sProgram, "Component Photo Path", ref sMessage);
            tsbtnPhoto.ToolTipText = sFilePath;
            sFilePath = SajetCommon.GetSysBaseData(g_sProgram, "Abnormal Report", ref sMessage);
            tsbtnMCI.ToolTipText = sFilePath;
            sFilePath = SajetCommon.GetSysBaseData(g_sProgram, "Component IQC SOP", ref sMessage);
            tsbtnInspSOP.ToolTipText = sFilePath;
            tsbtnQualityReport.ToolTipText = SajetCommon.GetSysBaseData(g_sProgram, "Quality Report", ref sMessage);
            string sCheckTooling = SajetCommon.GetSysBaseData(g_sProgram, "CHECK TOOLING", ref sMessage);
            g_bCheckTool = (sCheckTooling == "Y");

            string strApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(strApplicationPath + "\\IQCToolingdll.dll"))//在本地端發現DLL檔案則不另外組字串，否則從資料庫中搜尋程式在哪!
            {
                tsbtnTooling.Visible = false;
            }
            tpRoHS.Parent = null;
            tpSnHistory.Parent = null;
            tabPageSize.Parent = null;
            tpComponentPic.Parent = null;
        }

        private bool LotInspRoHS()
        {
            object[][] Params = new object[9][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", G_rIQC.sPartID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVENDORID", G_rIQC.sVendorID };
            Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TROHSON", "" };
            Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TROHSFLAG", "" };
            Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TCHECKROHS", "" };
            Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TPASSLOT", "" };
            Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TROHSLOT", "" };
            Params[8] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_ROHS_GET_INFO", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
            string sRoHSOn = dsTemp.Tables[0].Rows[0]["TROHSON"].ToString();
            string sCheckRoHS = dsTemp.Tables[0].Rows[0]["TCHECKROHS"].ToString();

            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult,0);
                return false;
            }

            if (sRoHSOn == "0")
                return false;
            return (sCheckRoHS == "Y");
        }

        private void combLotNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            G_rIQC.bLotInput = false;
            G_rIQC.bTypeFinish = false;
            timer1.Enabled = false;
            clearData();
            combItemSeq.Items.Clear();
            combItemSeq1.Items.Clear();
            combItemSeq.Text = "";

            if (combLotNo.Items.IndexOf(combLotNo.Text) < 0)
                return;
            string sSQL = "Select LOT_NO, QC_RESULT "
                 + " From SAJET.G_IQC_LOT "
                 + " Where QC_RESULT = 'N/A' "
                 + " and LOT_NO Like '" + combLotNo.Text + "-%' "
                 + " Order BY LOT_NO ";
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string sNo = Convert.ToString(row[0].ToString());
                string[] noList = sNo.Split('-');
                combItemSeq.Items.Add(noList[1]);
                combItemSeq1.Items.Add(noList[1]);
            }

            if (combItemSeq.Items.Count > 0)
            {
                combItemSeq.SelectedIndex = 0;
            }
        }

        private void GetRoHSCtl()
        {
            G_rIQC.bRoHSOn = false;
            sSQL = "SELECT STATUS FROM SAJET.G_IQC_ROHS_CTL "
                + " WHERE PART_ID =:PART_ID "
                + "   AND VENDOR_ID =:VENDOR_ID "
                + "   AND ROWNUM = 1 ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", G_rIQC.sVendorID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
           
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                G_rIQC.bRoHSOn = (dsTemp.Tables[0].Rows[0]["STATUS"].ToString() == "1");
            }
        }

        private void GetRoHSItem()
        {
            dgvRoHSItem.Rows.Clear();
            editRoHSMemo.Text = string.Empty;
            sSQL = "SELECT POSITION,PB,CD,HG,CR,BR,CL,MEMO "
                + "  FROM SAJET.G_IQC_ROHS_ITEM "
                + " WHERE LOT_NO =:LOT_NO "
                + " ORDER BY POSITION ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_no", G_rIQC.sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvRoHSItem.Rows.Add();
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["POSITION"].Value = dr["POSITION"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["PB"].Value = dr["PB"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["CD"].Value = dr["CD"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["HG"].Value = dr["HG"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["CR"].Value = dr["CR"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["BR"].Value = dr["BR"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["CL"].Value = dr["CL"];
                dgvRoHSItem.Rows[dgvRoHSItem.Rows.Count - 1].Cells["MEMO"].Value = dr["MEMO"];
            }

            string sQCResult = "N/A";

            sSQL = "SELECT DECODE(NVL(QC_RESULT,'N/A'),'0','OK','1','NG',NVL(QC_RESULT,'N/A')) QC_RESULT,MEMO "
                 + "  FROM SAJET.G_IQC_ROHS_RESULT "
                 + " WHERE LOT_NO =:LOT_NO "
                 + "   AND ROWNUM = 1 ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_no", G_rIQC.sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sQCResult = dsTemp.Tables[0].Rows[0]["QC_RESULT"].ToString();
                editRoHSMemo.Text = dsTemp.Tables[0].Rows[0]["MEMO"].ToString();
            }

            G_rIQC.bRoHSFinish = (sQCResult != "N/A");
            lablRoHSResult.Text = sQCResult;          
            
        //    editRoHSResult.Text =SajetCommon.SetLanguage("RoHS Result",1)+" : "+  sQCResult;
        }

        private void combItemSeq_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                G_rIQC.bLotInput = false;
                clearData();

                if (combItemSeq.Items.IndexOf(combItemSeq.Text) < 0)
                    return;
                if (!getIQCLotData(combLotNo.Text + "-" + combItemSeq.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Lot No Error", 1) + " - " + combLotNo.Text + "-" + combItemSeq.Text, 0);
                    combLotNo.SelectAll();
                    return;
                }

                GetSYSTestType();
                GetIQCTestType();
                GetDefaultSampleType();
                GetRoHSCtl();
                GetRoHSItem();

                btnStartRoHS.Enabled = (!G_rIQC.bRoHSOn);
                btnStopRoHS.Enabled = G_rIQC.bRoHSOn;
                PanelRoHS.Enabled = (!G_rIQC.bRoHSFinish);
                btnRoHSOK.Visible = PanelRoHS.Enabled;
                btnRoHSNG.Visible = PanelRoHS.Enabled;
                lablRoHSResult.Visible = (G_rIQC.bRoHSFinish);
                lablRoHSResult.BackColor = Color.Green;
                lablRoHSResult.ForeColor = Color.White;
                btnRollbackRoHs.Visible = lablRoHSResult.Visible;

                if (lablRoHSResult.Text == "NG")
                {
                    lablRoHSResult.BackColor = Color.Red;
                    lablRoHSResult.ForeColor = Color.Yellow;
                }

                lablRoHSOn.Visible = LotInspRoHS();
            //    editRoHSResult.Visible = (G_rIQC.bRoHSFinish);

                SetSelectRow(dgvItemType, "", "TYPE_ID");
                sbtnPass.Enabled = (G_rIQC.sHoldFlag == "0");
                sbtnReject.Enabled = sbtnPass.Enabled;
                sbtnWaive.Enabled = sbtnPass.Enabled;
                sbtnByPass.Enabled = sbtnPass.Enabled;
                sbtnSpecialWaive.Enabled = sbtnPass.Enabled;
//                PanelRoHS.Enabled = sbtnPass.Enabled;
//                PanelRoHSResult.Enabled = sbtnPass.Enabled;
                inspectItemToolStripMenuItem.Enabled = sbtnPass.Enabled;
                sbtnHold.Enabled = true;
                sbtnHold.Text = SajetCommon.SetLanguage("Hold", 1);

                if (G_rIQC.sHoldFlag == "1")
                {
                    sbtnHold.Text = SajetCommon.SetLanguage("Unhold", 1);
                    //sbtnHold.Enabled = (g_iPrivilege == 2);
                }

                G_rIQC.bLotInput = true;
            }
            finally
            {
                lablPartNo.Text = G_rIQC.sPartNo + " - " + G_rIQC.sPartVer;
                lablSpec1.Text = G_rIQC.sPartSpec;
                lablVendor.Text = G_rIQC.sVendorNane;
                lablLotSize.Text = G_rIQC.iLotSize.ToString();
                lablPONo.Text = G_rIQC.sPO;
                lablStartTime.Text = G_rIQC.sStartTime;
                lablEndTime.Text = G_rIQC.sTargetTime;
                //-------MAX-20110916---------
                lablSpecA.Text = G_rIQC.sSpecA; 
                lablSpecB.Text = G_rIQC.sSpecB;
                lablModelName.Text = G_rIQC.sMName;
                
                //--------------------------
                lablUrgentType.Text = G_rIQC.sUrgentType;
                btnRDAdmit.Enabled = (G_rIQC.sRDFlag == "Y");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clearData();
            getIQCLot();
            panelImg.BackgroundImage = null;
            ckbUrgent.Checked = false;
        }

        private bool CheckInspItemFinish()
        {
            //有測試值的項目檢查輸入量測值的個數是否達到最少檢驗數

            int iInspQty = 0;

            sSQL = "SELECT NVL(MIN_INSP_QTY,0) MIN_INSP_QTY "
                 + "  FROM SAJET.SYS_TEST_ITEM_TYPE "
                 + " WHERE ITEM_TYPE_ID =:ITEM_TYPE_ID "
                 + "   AND ROWNUM= 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", G_rIQC.sSelTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
                iInspQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MIN_INSP_QTY"].ToString());

            int iSampleSize = Convert.ToInt32(dgvSampleType.Rows[0].Cells["TSAMPLE_SIZE"].Value.ToString());

            if (iInspQty > iSampleSize)
                iInspQty = iSampleSize;

            sSQL = " SELECT A.*,NVL(B.QTY,0) INSP_QTY "
                 + "   FROM "
                 + " ( SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME  " //A :找出此料號大項內有值的小項
                 + "   FROM SAJET.SYS_PART_IQC_TESTITEM A "
                 + "       ,SAJET.SYS_TEST_ITEM B "
                 + "  WHERE A.RECID = :RECID "
                 + "    AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                 + "    AND A.ITEM_ID = B.ITEM_ID "
                 + "    AND B.HAS_VALUE ='Y' ) A  "
                 + " ,(SELECT A.ITEM_ID,COUNT(*) QTY " //B :找出此大項下每個有值的小項已輸入量測值的個數
                 + "  FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                 + " WHERE A.LOT_NO =:LOT_NO "
                 + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                 + "   AND NVL(A.VALUE ,'N/A') <>'N/A' "
                 + " GROUP BY A.ITEM_ID ) B "
                 + " WHERE A.ITEM_ID = B.ITEM_ID(+) " //以 A Table為主
                 + " ORDER BY A.ITEM_CODE ";

            Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", G_rIQC.sRECID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", G_rIQC.sSelTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            string sMessage = string.Empty;

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                int iInspedQty = Convert.ToInt32(dr["INSP_QTY"].ToString());
                if (iInspedQty < iInspQty )
                {
                    sMessage = sMessage + dr["ITEM_CODE"].ToString() + " - " + dr["ITEM_NAME"].ToString() + " ( " + iInspedQty.ToString()+ " ) " + Environment.NewLine;
                }
            }

            if (!string.IsNullOrEmpty(sMessage))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Inspection Item") + " : " + Environment.NewLine + Environment.NewLine
                                        +sMessage + Environment.NewLine
                                        +SajetCommon.SetLanguage("Inspected Qty less than Min Insp Qty")+" ( "+iInspQty.ToString()+" ) ", 0);
                return false;
            }

            //無測試值的項目檢查良品數與不良品數是否有輸入

            sSQL = " SELECT A.*,NVL(B.ITEM_ID,'-1') INSP_ITEM_ID "
                 + "   FROM "
                 + " ( SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME  " //A :找出此料號大項內無測試值的小項
                 + "   FROM SAJET.SYS_PART_IQC_TESTITEM A "
                 + "       ,SAJET.SYS_TEST_ITEM B "
                 + "  WHERE A.RECID = :RECID "
                 + "    AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                 + "    AND A.ITEM_ID = B.ITEM_ID "
                 + "    AND B.HAS_VALUE ='N' ) A  "
                 + " ,(SELECT A.ITEM_ID " //B :找出此大項下無值的小項是否有輸入pass,fail數
                 + "  FROM SAJET.G_IQC_TEST_ITEM A "
                 + " WHERE A.LOT_NO =:LOT_NO "
                 + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID ) B "
                 + " WHERE A.ITEM_ID = B.ITEM_ID(+) " //以 A Table為主
                 + " ORDER BY A.ITEM_CODE ";
            Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", G_rIQC.sRECID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", G_rIQC.sSelTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                if (dr["INSP_ITEM_ID"].ToString()=="-1")
                {
                    sMessage = sMessage + dr["ITEM_CODE"].ToString() + " - " + dr["ITEM_NAME"].ToString() + Environment.NewLine;
                }
            }

            if (!string.IsNullOrEmpty(sMessage))
            {
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Inspection Item") + " : " + Environment.NewLine + Environment.NewLine
                                        + sMessage + Environment.NewLine
                                        + SajetCommon.SetLanguage("PASS QTY and FAIL QTY is Null")+Environment.NewLine+Environment.NewLine
                                        + SajetCommon.SetLanguage("Sure to Complete this Item ?"),2) != DialogResult.Yes)
                {
                    return false;
                }
            }
            return true;
        }

        private void dgvItemType_Click(object sender, EventArgs e)
        {

        }

        private void dgvItemType_SelectionChanged(object sender, EventArgs e)
        {
            editItemTypeName.Text = string.Empty;
            G_rIQC.sSelTypeID = "0";
            G_rIQC.iSelTypeSeq = -1;
            //changeSampleTypeToolStripMenuItem.Enabled = true;
            changeSampleTypeToolStripMenuItem.Enabled = g_bChangAQL;
            dgvDefect.ContextMenuStrip = cmsdefect;
            dgvSampleType.Rows.Clear();
            dgvDefect.Rows.Clear();

            if (dgvItemType.Rows.Count == 0 || dgvItemType.CurrentRow == null || dgvItemType.SelectedCells.Count == 0)
            {
                return;
            }

            if (!G_rIQC.bTypeFinish)
                return;
            //lablTypeName.Text = dgvItemType.CurrentRow.Cells["TYPE_CODE"].Value.ToString() + " - " + dgvItemType.CurrentRow.Cells["TYPE_NAME"].Value.ToString(); 

            editItemTypeName.Text = dgvItemType.CurrentRow.Cells["TYPE_CODE"].Value.ToString() + " - " + dgvItemType.CurrentRow.Cells["TYPE_NAME"].Value.ToString();
            string sSamplingID = dgvItemType.CurrentRow.Cells["SAMPLING_ID"].Value.ToString();
            string sSampleLevelID = dgvItemType.CurrentRow.Cells["SAMPLE_LEVEL_ID"].Value.ToString();
            string sQCResult = dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString();
            TSampleType rSampleType = getSamplingPlanRange(sSamplingID, sSampleLevelID, G_rIQC.iLotSize);
            dgvSampleType.Rows.Clear();
            dgvSampleType.Rows.Add();
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_TYPE"].Value = rSampleType.sSamplingType;
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_LEVEL"].Value = rSampleType.sSamplingLevelName;
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_SIZE"].Value = rSampleType.iSampleQty.ToString();
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TCRITICAL"].Value = rSampleType.iCriticalRej.ToString();
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TMAJOR"].Value = rSampleType.iMajorRej.ToString();
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TMINOR"].Value = rSampleType.iMinorRej.ToString();
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_LEVEL_ID"].Value = rSampleType.sSamplingLevelID;
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_UNIT"].Value = rSampleType.sSamplingUnit;
            dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_ID"].Value = rSampleType.sSamplingID;
            G_rIQC.sSelTypeName = dgvItemType.CurrentRow.Cells["TYPE_NAME"].Value.ToString();
            G_rIQC.sSelTypeID = dgvItemType.CurrentRow.Cells["TYPE_ID"].Value.ToString();
            G_rIQC.iSelTypeSeq = Convert.ToInt32(dgvItemType.CurrentRow.Cells["SEQ"].Value.ToString());
            importTestResultToolStripMenuItem.Enabled = (sQCResult == "N/A");

            if (importTestResultToolStripMenuItem.Enabled)
                importTestResultToolStripMenuItem.Enabled = (G_rIQC.sHoldFlag == "0");//Hold狀態時,不允許做任何動作
            changeSampleTypeToolStripMenuItem.Enabled = importTestResultToolStripMenuItem.Enabled;
            if (changeSampleTypeToolStripMenuItem.Enabled)
                changeSampleTypeToolStripMenuItem.Enabled = g_bChangAQL;
            dgvDefect.ContextMenuStrip = null;

            if (importTestResultToolStripMenuItem.Enabled)
                dgvDefect.ContextMenuStrip = cmsdefect;
            GetTestTypeDefect(G_rIQC.sLotNo, G_rIQC.sSelTypeID);
            GetTestTypeMemo(G_rIQC.sLotNo, G_rIQC.sSelTypeID);
        }

        private void GetTestTypeMemo(string sLotNo, string sTypeID)
        {
            editTestTypeMemo.Text = "";            
            sSQL = "SELECT A.MEMO"
                + "  FROM SAJET.G_IQC_TEST_TYPE A "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                editTestTypeMemo.Text = dsTemp.Tables[0].Rows[0]["MEMO"].ToString();
            }
        }

        private void GetTestTypeDefect(string sLotNo, string sTypeID)
        {
            dgvDefect.Rows.Clear();
            sSQL = "SELECT A.DEFECT_ID,B.DEFECT_CODE,B.DEFECT_DESC,A.DEFECT_QTY,A.DEFECT_MEMO "
                + "  FROM SAJET.G_IQC_TEST_TYPE_DEFECT A "
                + "      ,SAJET.SYS_DEFECT B"
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND A.DEFECT_ID = B.DEFECT_ID(+) "
                + " ORDER BY B.DEFECT_CODE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvDefect.Rows.Add();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_CODE"].Value = dr["DEFECT_CODE"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_DESC"].Value = dr["DEFECT_DESC"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_QTY"].Value = dr["DEFECT_QTY"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_MEMO"].Value = dr["DEFECT_MEMO"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_ID"].Value = dr["DEFECT_ID"].ToString();
            }
        }

        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;
                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //第一個有顯示的欄位(focus到隱藏欄位會錯誤)
                        sShowField = GridData.Columns[i].Name;
                        break;
                    }
                }
                for (int i = 0; i <= GridData.Rows.Count - 1; i++)
                {
                    if (sPrimaryKey == GridData.Rows[i].Cells[sField].Value.ToString())
                    {
                        iIndex = i;
                        break;
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void UpdateTestTypeSamplePlan(string sTypeID, TSampleType rSampleType) //變更抽驗計畫儲存
        {
            object[][] Params = new object[7][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", sTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLELEVEL", rSampleType.sSamplingLevelID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLEID", rSampleType.sSamplingID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLESIZE", rSampleType.iSampleQty.ToString() };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", ClientUtils.UserPara1 };
            Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_IQC_UPDATE_SAMPLETYPE", Params);
        }

        private void btnAQL_Click(object sender, EventArgs e) //變更抽驗計畫
        {
            if (G_rIQC.sSelTypeID == "0")
            {
                return;
            }

            fAQL formAQL = new fAQL();
            formAQL.g_sItemTypeID = G_rIQC.sSelTypeID;
            formAQL.g_iLotSize = G_rIQC.iLotSize;
            formAQL.g_sItemTypeName = G_rIQC.sSelTypeName;
          
            string sSampleType = dgvSampleType.Rows[0].Cells["TSAMPLE_TYPE"].Value.ToString();
            string sSampleLevelID = dgvSampleType.Rows[0].Cells["TSAMPLE_LEVEL_ID"].Value.ToString();
            formAQL.initial(G_rIQC.sLotNo, G_rIQC.lstSampleLevel, sSampleType, sSampleLevelID);

            if (formAQL.ShowDialog() != DialogResult.OK) return;

            string sSamplingID = formAQL.g_sSampleID;
            sSampleLevelID = formAQL.g_sSampleLeveID;
            TSampleType rSampleType = getSamplingPlanRange(sSamplingID, sSampleLevelID, G_rIQC.iLotSize);

            //變更顯示內容
            /*
            dgvSampleType.Rows.Clear();
            sSQL = "SELECT A.SAMPLING_PLAN_ID ,A.SAMPLING_LEVEL "
                 + "  FROM SAJET.G_IQC_TEST_TYPE A "
                 + " WHERE A.LOT_NO =:LOT_NO "
                 + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                 + "   AND ROWNUM = 1 ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", G_rIQC.sSelTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sSamplingID = dsTemp.Tables[0].Rows[0]["SAMPLING_PLAN_ID"].ToString();
                sSampleLevelID = dsTemp.Tables[0].Rows[0]["SAMPLING_LEVEL"].ToString();
                TSampleType rSampleType = getSamplingPlanRange(sSamplingID, sSampleLevelID, G_rIQC.iLotSize);
                dgvSampleType.Rows.Add();
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_TYPE"].Value = rSampleType.sSamplingType;
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_LEVEL"].Value = rSampleType.sSamplingLevelName;
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_SIZE"].Value = rSampleType.iSampleQty.ToString();
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TCRITICAL"].Value = rSampleType.iCriticalRej.ToString();
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TMAJOR"].Value = rSampleType.iMajorRej.ToString();
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TMINOR"].Value = rSampleType.iMinorRej.ToString();
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_LEVEL_ID"].Value = rSampleType.sSamplingLevelID;
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_UNIT"].Value = rSampleType.sSamplingUnit;
                dgvSampleType.Rows[dgvSampleType.Rows.Count - 1].Cells["TSAMPLE_ID"].Value = rSampleType.sSamplingID;
                dgvItemType.Rows[G_rIQC.iSelTypeSeq].Cells["SAMPLE_TYPE"].Value = rSampleType.sSamplingType;
                dgvItemType.Rows[G_rIQC.iSelTypeSeq].Cells["SAMPLE_LEVEL"].Value = rSampleType.sSamplingLevelName;
                dgvItemType.Rows[G_rIQC.iSelTypeSeq].Cells["SAMPLE_SIZE"].Value = rSampleType.iSampleQty.ToString();
                dgvItemType.Rows[G_rIQC.iSelTypeSeq].Cells["SAMPLING_ID"].Value = rSampleType.sSamplingID;
                dgvItemType.Rows[G_rIQC.iSelTypeSeq].Cells["SAMPLE_LEVEL_ID"].Value = rSampleType.sSamplingLevelID;
            }
             */

            bool bChangeALL = false;
           
            for (int i = 0; i <= dgvItemType.Rows.Count - 1; i++)
            {
                string sTypeID = dgvItemType.Rows[i].Cells["TYPE_ID"].Value.ToString();
                string sQCResult = dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString();
                if (dgvItemType.Rows[i].Cells["SAMPLE_TYPE"].Value.ToString() == sSampleType && sQCResult == "N/A" && G_rIQC.sSelTypeID != sTypeID)
                {
                     if (SajetCommon.Show_Message("Change ALL Test Type ?", 2) == DialogResult.Yes)
                     {
                         bChangeALL = true;
                        
                     }
                     break;
                }
            }
            string sTypeCode="";
            if (bChangeALL)
            {
                for (int i = 0; i <= dgvItemType.Rows.Count - 1; i++)
                {
                    string sTypeID = dgvItemType.Rows[i].Cells["TYPE_ID"].Value.ToString();
                    string sQCResult = dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString();
                    string sTypeName = dgvItemType.Rows[i].Cells["TYPE_NAME"].Value.ToString();
                    sTypeCode = dgvItemType.Rows[i].Cells["TYPE_CODE"].Value.ToString();
                    if (dgvItemType.Rows[i].Cells["SAMPLE_TYPE"].Value.ToString() == sSampleType && sQCResult == "N/A")
                    {
                        UpdateTestTypeSamplePlan(sTypeID, rSampleType);
                    }
                }
            }
            else
            {
                UpdateTestTypeSamplePlan(G_rIQC.sSelTypeID, rSampleType);
            }
            sTypeCode = dgvItemType.CurrentRow.Cells["TYPE_CODE"].Value.ToString();
            GetSYSTestType();
            GetIQCTestType();
            GetDefaultSampleType();
            if (dgvItemType.Rows.Count > 0)
            {
                
                SetSelectRow(dgvItemType, sTypeCode, "TYPE_CODE");
            }

        }
       
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void cmsdefect_Opening(object sender, CancelEventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) //刪除不良代碼
        {
            if (dgvDefect.Rows.Count == 0 || dgvDefect.CurrentCell == null)
                return;

            string sDefectID = dgvDefect.CurrentRow.Cells["DEFECT_ID"].Value.ToString();
            string sDefectCode = dgvDefect.CurrentRow.Cells["DEFECT_CODE"].Value.ToString();
            
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete this Defect Code ?", 1) + "  " + sDefectCode, 2) != DialogResult.Yes)
                return;

            sSQL = "DELETE SAJET.G_IQC_TEST_TYPE_DEFECT "
                + " WHERE LOT_NO =:LOT_NO "
                + "   AND ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND DEFECT_ID =:DEFECT_ID ";
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", G_rIQC.sSelTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_ID", sDefectID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            GetTestTypeDefect(G_rIQC.sLotNo, G_rIQC.sSelTypeID);
        }

        private TDefect CheckDefect(string sDefectCode)
        {
            sSQL = " SELECT DEFECT_ID,DEFECT_DESC,DEFECT_LEVEL,DEFECT_CODE "
                 + "   FROM SAJET.SYS_DEFECT "
                 + "  WHERE DEFECT_CODE =:DEFECT_CODE "
                 + "    AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_CODE", sDefectCode };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            TDefect rDefect = new TDefect();
            rDefect.sDefectID = "0";

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTemp.Tables[0].Rows[0];
                rDefect.sDefectID = dr["DEFECT_ID"].ToString();
                rDefect.sDefectDesc = dr["DEFECT_DESC"].ToString();
                rDefect.sDefectLevel = dr["DEFECT_LEVEL"].ToString();
                rDefect.sDefectCode = dr["DEFECT_CODE"].ToString();
            }
            return rDefect;
        }

        private void inspectItemToolStripMenuItem_Click(object sender, EventArgs e) //檢驗項目
        {
            if (G_rIQC.sSelTypeID == "0")
                return;
            //SajetCommon.Show_Message(G_rIQC.sLotNo, 0);
            fInspItem fData = new fInspItem();
            fData.g_sProgram = g_sProgram;
            fData.g_sLotNo = G_rIQC.sLotNo;
            fData.g_sTypeID = G_rIQC.sSelTypeID;
            fData.g_sTypeName = G_rIQC.sSelTypeName;
            fData.g_iLotSize = G_rIQC.iLotSize;
            fData.g_iSampleSize = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_SIZE"].Value.ToString());
            fData.g_iSampleID = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_ID"].Value.ToString());
            fData.g_iSampleLevel = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_LEVEL_ID"].Value.ToString());
            if (G_rIQC.sPARTTYPE != "N/A")
            {
                fData.g_sRecID = G_rIQC.sPARTTYPE;
                fData.g_sFlag = "PART_TYPE";
            }
            else
            {
                fData.g_sFlag = "PART";
                fData.g_sRecID = G_rIQC.sRECID;
            }
            
           
            fData.btnSave.Enabled = (dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString() == "N/A");
            

            try
            {
                fData.ShowDialog();
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void editFailQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
        }

        private void combLotNo_TextChanged(object sender, EventArgs e)
        {
            combItemSeq.Items.Clear();
            combItemSeq1.Items.Clear();
            combItemSeq.Text = string.Empty;
            clearData();
        }

        private bool CheckRoHS(ref string sRoHSResult, ref string sRoHSExist)
        {
            object[][] Params = new object[6][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", G_rIQC.sPartID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVENDORID", G_rIQC.sVendorID };
            Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TROHSEXIST", "" };
            Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TROHSRESULT", "" };
            Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_ROHS_CONFIRM", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult,0);
                return false;
            }

            sRoHSResult = dsTemp.Tables[0].Rows[0]["TROHSRESULT"].ToString();
            sRoHSExist = dsTemp.Tables[0].Rows[0]["TROHSEXIST"].ToString();
            return true;
        }

        private void sbtnPass_Click(object sender, EventArgs e) //批號檢驗結果
        {       
            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLotNo.SelectAll();
                return;
            }
            DataSet dsQResult;
            string dsQc = "SELECT QC_RESULT FROM SAJET.G_IQC_LOT "
                        + "WHERE LOT_NO = '" + combLot.Text.Trim() + "' ";
            dsQResult = ClientUtils.ExecuteSQL(dsQc);
            if (dsQResult.Tables[0].Rows[0][0].ToString() != "N/A")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Lot No Has determined"), 0);
                return;
            }
            int iTag = Convert.ToInt32((sender as Button).Tag.ToString()); //檢查按下哪個Button
            string sHint = (sender as Button).Text;
            int iOKItem = 0;
            int iNGItem = 0;
            string sRTNo = combLotNo.Text;
            string sRTSeq = combItemSeq.Text;
            string sLotNo = combLot.Text;
            int iIndex = -1;

            //暫停/解除暫停功能
            if (iTag == 5)
            {
                fHold fData = new fHold();
                try
                {
                    fData.g_sLotNo = G_rIQC.sLotNo;
                    fData.g_sHoldFlag = G_rIQC.sHoldFlag;
                    if (fData.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    clearData();
                    getIQCLot();
                    iIndex = combLotNo.Items.IndexOf(sRTNo);

                    if (iIndex >= 0)
                    {
                        combLotNo.SelectedIndex = iIndex;
                    }

                    iIndex = combItemSeq.Items.IndexOf(sRTSeq);

                    if (iIndex >= 0)
                    {
                        combItemSeq.SelectedIndex = iIndex;
                    }
                    return;
                }
                finally
                {
                    fData.Dispose();
                }
            }
            //========================================================

            if (dgvItemType.Rows.Count == 0)
            {
                SajetCommon.Show_Message("Not Define Test Type", 0);
                return;
            }

            string sItemTypeData = "N/A";
            string sMsg = string.Empty;
            string sItemNoResult = string.Empty;
            string sItemNG = string.Empty;
            string sLotLevel = "N/A";

            for (int i = 0; i <= dgvItemType.Rows.Count - 1; i++)
            {
                string sQCResult = dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString();
                string sTypeName = dgvItemType.Rows[i].Cells["TYPE_NAME"].Value.ToString();
                string sTypeCode = dgvItemType.Rows[i].Cells["TYPE_CODE"].Value.ToString();
                string sSampleLevelID = dgvItemType.Rows[i].Cells["SAMPLE_LEVEL_ID"].Value.ToString();
                if (sLotLevel =="N/A" && !string.IsNullOrEmpty(sSampleLevelID))
                    sLotLevel = sSampleLevelID;
                if (sQCResult == "N/A")
                {
                    sItemNoResult = sItemNoResult + sTypeCode + " - " + sTypeName + Environment.NewLine;
                }
                if (sQCResult == "NG")
                {
                    sItemNG = sItemNG + sTypeCode + " - " + sTypeName + Environment.NewLine;
                }
                if (iTag == 4 || iTag==8 ) //By Pass,允許大項不用驗,但還沒記資料庫的項目,要不要記?
                {
                    if (sQCResult == "N/A")
                    {
                        if (iTag == 4)
                        {
                            if (sSampleLevelID != "3") //非免驗的抽驗等級
                            {
                                sMsg = sMsg + sTypeCode + " - " + sTypeName + Environment.NewLine;
                            }
                        }
                        if (sItemTypeData == "N/A")
                        {
                            sItemTypeData = string.Empty;
                        }
                        sItemTypeData = sItemTypeData
                                       + dgvItemType.Rows[i].Cells["TYPE_ID"].Value.ToString() + "@"
                                       + dgvItemType.Rows[i].Cells["SAMPLING_ID"].Value.ToString() + "@"
                                       + dgvItemType.Rows[i].Cells["SAMPLE_LEVEL_ID"].Value.ToString() + "@"
                                       + dgvItemType.Rows[i].Cells["SAMPLE_SIZE"].Value.ToString() + "@";
                   }
                   
                }
                if (sQCResult == "OK") //OK大項數量
                    iOKItem += 1;
                if (sQCResult == "NG") //NG大項數量
                    iNGItem += 1;
            }
            if (iTag == 4) //免驗
            {
                if (!string.IsNullOrEmpty(sMsg))
                {
                    //if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Type Name", 1) + " : " + Environment.NewLine + Environment.NewLine
                    //                           + sMsg + Environment.NewLine + Environment.NewLine
                    //                           + SajetCommon.SetLanguage("Sample Level is not By Pass. Continue ?"), 2) != DialogResult.Yes)
                    //{
                    //    return;
                    //}
                    SajetCommon.Show_Message((SajetCommon.SetLanguage("Type Name", 1) + " : " + Environment.NewLine + Environment.NewLine
                                               + sMsg + Environment.NewLine 
                                               + SajetCommon.SetLanguage("Sample Level is not By Pass.")), 0);
                    return;
                }
                if (iNGItem > 0)
                {
                    sMsg = SajetCommon.SetLanguage("Type Name", 1) + " : " + Environment.NewLine + Environment.NewLine
                                             + sItemNG + Environment.NewLine 
                                             + SajetCommon.SetLanguage("Result", 1) + " =  NG" + Environment.NewLine
                                             + SajetCommon.SetLanguage("Can not By Pass this Lot", 1);
                    SajetCommon.Show_Message(sMsg, 0);
                    return;
                }
            }
            else if (iTag==8)
            {
            }
            else
            {
                if (iTag == 0 && iNGItem > 0) //按下允收但NG大項數量大於0
                {
                    sMsg = SajetCommon.SetLanguage("Type Name", 1) + " : " + Environment.NewLine + Environment.NewLine
                         + sItemNG + Environment.NewLine
                         + SajetCommon.SetLanguage("Result", 1) + " =  NG" + Environment.NewLine
                         + SajetCommon.SetLanguage("Can not Pass this Lot", 1);
                    SajetCommon.Show_Message(sMsg, 0);
                    return;
                }

                if (!string.IsNullOrEmpty(sItemNoResult))
                {
                    sMsg = SajetCommon.SetLanguage("Type Name", 1) + " : " + Environment.NewLine + Environment.NewLine
                          + sItemNoResult + Environment.NewLine
                          + SajetCommon.SetLanguage("Inspection Result unknow");
                    SajetCommon.Show_Message(sMsg, 0);
                    return;
                }
            }

            string sRoHSResult = "N/A";
            string sRoHSExist = "N";
            if (tpRoHS.Parent != null)
            {

                if (!CheckRoHS(ref sRoHSResult, ref sRoHSExist))
                {
                    return;
                }
                if (sRoHSExist == "Y")
                {
                    if (sRoHSResult == "1" && iTag != 1)
                    {
                        SajetCommon.Show_Message("RoHS NG,Please Reject this Lot", 0);
                        return;
                    }
                }
                else
                {
                    if (iTag != 1 && iTag != 4 && iTag != 8) //Reject和免驗不提示
                    {
                        if (SajetCommon.Show_Message("Not Found RoHS Records. Sure to Finish this Lot ?", 2) != DialogResult.Yes)
                            return;
                    }
                }
            }
            /*
            if (sRoHSExist == "Y" && sRoHSResult == "1") //ROHS NG一定要批退此批
            {
                if (iTag != 1)
                {
                    SajetCommon.Show_Message("RoHS NG,Please Reject this Lot",0);
                    return;
                }
            }
             */ 
            //add by rita 2014/12/17 for Check Tooling
            if (g_bCheckTool)
            {
                int iQty = GetTooling(G_rIQC.sLotNo);
                if (iQty == 0)
                {
                    SajetCommon.Show_Message("Please Input Tooling", 0);
                    return;
                }
            }

            if (SajetCommon.Show_Message(sHint+" "+ SajetCommon.SetLanguage("Lot No",1)+" : "+G_rIQC.sLotNo+" ?",2) !=DialogResult.Yes)
                return;
                  
            string sModel_Name;
            string sSQL = "";
            DataSet dsTemp = new DataSet();

            int iReceiveQty = G_rIQC.iLotSize; //預設是lot size
            int iInspWHour = 0;
            string sLot = string.Empty;
            string sDateCode = string.Empty;
            string sMaterialVer = string.Empty;
            string sDMDA = string.Empty;
            string sLotMemo = string.Empty;
            string sMRBMemo = string.Empty;
            string sWaiveNo = string.Empty;
            string sMessage = "";
            string sDisplayMemoForm = SajetCommon.GetSysBaseData(g_sProgram, "SHOW MEMO FORM", ref sMessage);
            
            //if (sDisplayMemoForm=="")
            //    sDisplayMemoForm = "Y";

            if(!string.IsNullOrEmpty(sDisplayMemoForm))
            { S1 = sDisplayMemoForm; }

            if ((S1.IndexOf(iTag.ToString()) >= 0))
            {

                fLotMemo formMemo = new fLotMemo();

                try
                {
                    formMemo.g_sExeName = g_sExeName;
                    formMemo.g_sLotLevel = sLotLevel;
                    formMemo.lablResult.BackColor = (sender as Button).BackColor;
                    formMemo.lablResult.ForeColor = (sender as Button).ForeColor;
                    // formMemo.initial(G_rIQC.sLotNo, G_rIQC.sQCResult, lablLotSize.Text, iOKItem, iNGItem, DateInsp.Value.ToString("yyyy/MM/dd"), iTag);
                    formMemo.initial(G_rIQC.sLotNo, sHint, lablLotSize.Text, iOKItem, iNGItem, DateInsp.Value.ToString("yyyy/MM/dd"), iTag);

                    if (formMemo.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    iReceiveQty = Convert.ToInt32(formMemo.editReceiveQty.Text);
                    iInspWHour = Convert.ToInt32(formMemo.editInspWorkHours.Text);
                    sLot = formMemo.editLot.Text;
                    sDateCode = formMemo.editDateCode.Text;
                    sMaterialVer = formMemo.editMaterialVer.Text;
                    sDMDA = formMemo.editDMDA.Text;
                    sLotMemo = formMemo.editLotMemo.Text;
                    sLotLevel = formMemo.g_sLotLevel;
                    sMRBMemo = formMemo.editMRB.Text;
                    sWaiveNo = formMemo.editWaiveNo.Text;
                }
                finally
                {
                    formMemo.Dispose();
                }

            }
            string sSampleFile = "", sFileName = "";
            if (iTag == 1)
            {
                sSampleFile = System.Windows.Forms.Application.StartupPath + "\\" + g_sExeName + "\\RejectReport.xlt";
                if (!File.Exists(sSampleFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Get Excel Sample File Error") + " ( " + sSampleFile + " )", 0);
                    return;
                }
              //SajetCommon.Show_Message(sSampleFile, -1);
             //   return;
                string sExceptionVendor = SajetCommon.GetSysBaseData(g_sProgram, "Exception Vendor", ref sMessage);
                if (!string.IsNullOrEmpty(sExceptionVendor))
                { S2 = sExceptionVendor; }
                if ((!string.IsNullOrEmpty(S2) && S2.IndexOf(G_rIQC.sVendorCode) >= 0))
                { }
                else
                {
                    sFileName = GetFileName();
                }
                    if (sFileName == "N/A")
                    return;               
            }

           // if (iTag == 1) //批退 RECEIVE_QTY = 0
           //     iReceiveQty = 0;

            object[][] Params = new object[15][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQCRESULT", iTag.ToString() };  //判別按鈕狀態 允收、批退
            Params[2] = new object[] { ParameterDirection.Input, OracleType.Int32, "TRECEIVEQTY", iReceiveQty };      //允收數量
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "TINSPWORKHOUR", iInspWHour };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTMEMO", sLotMemo };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDATECODE", sDateCode };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOT", sLot };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMATERIALVER", sMaterialVer };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDMDA", sDMDA };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEDATA", sItemTypeData };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TINSPDATE", DateInsp.Value.ToString("yyyy/MM/dd") };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTLEVEL", sLotLevel };
//            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMRBMEMO", sMRBMemo };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWAIVENO", sWaiveNo };
            Params[14] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_LOT_GO", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
            
            if (sResult != "OK")
            {
                ClientUtils.ShowMessage(sResult, 0);
                return;
            }
            // SajetCommon.Show_Message(G_rIQC.sLotNo, 0);
            //add by rita 2014/12若此批有治具符合告警條牛,則提示治具資料
            Show_Alarm_Event(G_rIQC.sLotNo);
           
            /*
            if (iTag == 1)
            {
                if ((!string.IsNullOrEmpty(S2) && S2.IndexOf(G_rIQC.sVendorCode) >= 0))
                { }
                else
                {
                    CreateMCIReport(sFileName, sSampleFile, G_rIQC.sLotNo);
                    showfLotMemo();
                }
            }
             */ 

            /*if (iTag == 0 || iTag == 4) //允收和免驗自動上傳ERP
                uploadERP(iTag);*/
         
            clearData();
            getIQCLot();
            iIndex = combLot.Items.IndexOf(sLotNo);

            if (iIndex >= 0)
            {
                combLot.SelectedIndex = iIndex;
            }
            else
            {
                string[] sLotNOList = sLotNo.Split('-');
                string g_sLen = sLotNOList[sLotNOList.Length - 1].Length.ToString();

                //ADD RITA 2011/09/27 自動帶同一張rt單的下一個項次
                sSQL = "SELECT C.RT_NO||'-'||TRIM(LPAD(B.RT_SEQ,'" + g_sLen +"',0)) LOT_NO "
                      +"  from "
                      +"  (SELECT RT_ID,RT_SEQ FROM SAJET.G_IQC_LOT A   "
                      +"   WHERE A.LOT_NO=:LOT_NO ) A "
                      +"  ,SAJET.G_IQC_LOT B "
                      +"  ,SAJET.G_ERP_RTNO C "
                      +" WHERE B.RT_ID = A.RT_ID "
                      +"   AND B.RT_SEQ > A.RT_SEQ "
                      +"   AND B.QC_RESULT='N/A' "
                      +"  AND B.RT_ID = C.RT_ID "
                      +" ORDER BY  B.RT_SEQ ";

                Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count > 0)
                    sLotNo = dsTemp.Tables[0].Rows[0]["LOT_NO"].ToString();
                iIndex = combLot.Items.IndexOf(sLotNo);
                if (iIndex >= 0)
                {
                    combLot.SelectedIndex = iIndex;
                }
            }
        }
        private void Show_Alarm_Event(string sLotNo)
        {
            string sSQL = "SELECT C.TOOLING_SN  "
                       + "  ,NVL(NVL(C.LIMIT_USED_COUNT,D.LIMIT_USED_COUNT),0)  LIMIT_USED_COUNT "
                       + "  ,NVL(NVL(C.MAX_USED_COUNT,D.MAX_USED_COUNT),0)  MAX_USED_COUNT "
                       + "  , NVL(B.USED_COUNT,0)  USED_COUNT "
                       + " FROM SAJET.G_IQC_TOOLING A "
                       + "     ,SAJET.G_TOOLING_SN_STATUS B "
                       + "     ,SAJET.SYS_TOOLING_SN C "
                       + "     ,SAJET.SYS_TOOLING D "
                       + " WHERE A.LOT_NO=:LOT_NO "
                       + "  AND A.ALARM_RECID <>0 "
                       + "  AND A.TOOLING_SN_ID = B.TOOLING_SN_ID "
                       + "  AND A.TOOLING_SN_ID = C.TOOLING_SN_ID "
                       + "  AND C.TOOLING_ID = D.TOOLING_ID "
                       + " ORDER BY C.TOOLING_SN ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            string sToolingData = "";
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                string sToolingSN = dr["TOOLING_SN"].ToString();
                string sLimitCount = dr["LIMIT_USED_COUNT"].ToString();
                string sMaxCount = dr["MAX_USED_COUNT"].ToString();
                string sUsedCount = dr["USED_COUNT"].ToString();
                sToolingData += sToolingSN + " [ " + sUsedCount + " / " + sMaxCount + " / " + sLimitCount + " ]" + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(sToolingData))
            {
                sToolingData = SajetCommon.SetLanguage("Tooling SN Over Warning or Limit Used Count") + Environment.NewLine + Environment.NewLine
                              + SajetCommon.SetLanguage("Tooling SN") + " [ " + SajetCommon.SetLanguage("Used Count") + " / " + SajetCommon.SetLanguage("Max Used Count") + " / " + SajetCommon.SetLanguage("Limit Used Count") + " ]" + Environment.NewLine
                             + sToolingData;
                SajetCommon.Show_Message(sToolingData, 3);
            }
        }

        private void showfLotMemo()
        {
           
        }
        private string GetFileName()
        {
            /*
                MCI-YYMMXX-供應商
                YY-西元年末兩碼
                MM-月份
                XX-流水號01~99
                供應商-供應商名稱
             */
            try
            {
                object[][] Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TFILENAME", "" };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_GET_MCI_REPPORT_FILENME", Params);
                string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

                if (sResult != "OK")
                {
                    ClientUtils.ShowMessage(sResult, 0);
                    return "N/A";
                }
                return dsTemp.Tables[0].Rows[0]["TFILENAME"].ToString();
            }
            catch (Exception EX)
            {
                SajetCommon.Show_Message(EX.Message, 0);
                return "N/A";
            }
        }
        private string[] Get_Lot_Defect_Image()
        {
            string g_sFilePath = System.Windows.Forms.Application.StartupPath + "\\" + g_sExeName + "\\Image";
            if (!Directory.Exists(g_sFilePath))
                Directory.CreateDirectory(g_sFilePath);
            else
            {
                string[] FileList = new string[] { "" };
                FileList = Directory.GetFiles(g_sFilePath, "*.*");
                for (int i = 0; i <= FileList.Length - 1; i++)
                {
                    try
                    {
                        File.Delete(FileList[i].ToString());
                    }
                    catch
                    {
                    }
                }
            }

            string sSQL = " Select A.* ,TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') FILENAME "
                      + "   from SAJET.G_IQC_LOT_IMAGE A "
                      + " where A.LOT_NO = '" + G_rIQC.sLotNo + "' "
                      + " ORDER BY A.FILE_NAME ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            ListBox listTemp = new ListBox();
            listTemp.Items.Add(".JPG");
            string[] sFileList = new string[dsTemp.Tables[0].Rows.Count];
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                byte[] blobFile = new byte[0];
                if (!dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"].ToString().Equals(""))
                {
                    blobFile = (byte[])dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"];
                }
                string sImgFile = dsTemp.Tables[0].Rows[i]["FILE_NAME"].ToString();
                FileInfo f = new FileInfo(sImgFile);
                string sExt = f.Extension;
                if (listTemp.Items.IndexOf(sExt.ToUpper()) < 0)
                    continue;
                string sFileName = g_sFilePath + "\\" + dsTemp.Tables[0].Rows[i]["FILENAME"].ToString() + i.ToString() + sExt;
                File.WriteAllBytes(sFileName, blobFile);
                sFileList[i] = sFileName;
            }
            return sFileList;
        }

        public static void RepeatCreateMCIReport(string g_sFileName, string g_sSampleFile, string g_sLotNoHistory)
        {
            //SajetCommon.Show_Message(g_sFileName + g_sSampleFile,0);
            //return;
            fMain ReportMCI = new fMain();
            ReportMCI.CreateMCIReport(g_sFileName, g_sSampleFile, g_sLotNoHistory);
        }

        private void CreateMCIReport(string sFileName, string sSampleFile, string g_sLotNoMain)
        {
            // 產生Excel
            G_rIQC.sLotNo = g_sLotNoMain;
            string[] sFileList = Get_Lot_Defect_Image();
            sSQL = " SELECT PARAM_VALUE FROM SAJET.SYS_BASE WHERE PARAM_NAME='MCI Path' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string g_sRejectReportPath = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString().Trim(); // 驗退異常單存放路徑

            sSQL = "SELECT TO_CHAR(A.END_TIME,'YYYY/MM/DD') INSP_DATE,TO_CHAR(A.END_TIME,'HH24:MI:SS') INSP_TIME "
                + "      ,A.LOT_SIZE,A.LOT_MEMO "
                + "      ,B.MODEL_ID ,B.PART_NO "
                + "      ,C.VENDOR_NAME "
                + "      ,D.RT_NO ,TO_CHAR(D.RECEIVE_TIME,'YYYY/MM/DD') RECEIVE_DATE "
                + "      ,E.EMP_NAME "
                + "      ,A.MCI_REPORT_FILENAME DEFECT_NO "
                + "  FROM SAJET.G_IQC_LOT A "
                + "      ,SAJET.SYS_PART B "
                + "      ,SAJET.SYS_VENDOR C "
                + "      ,SAJET.G_ERP_RTNO D "
                + "      ,SAJET.SYS_EMP E "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.PART_ID = B.PART_ID(+) "
                + "   AND A.VENDOR_ID = C.VENDOR_ID(+) "
                + "   AND A.RT_ID = D.RT_ID(+) "
                + "   AND A.EMP_ID = E.EMP_ID(+) ";

            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            DataSet dsData = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsData.Tables[0].Rows.Count == 0)
                return;
            DataRow dr = dsData.Tables[0].Rows[0];
            string sModelName = SajetCommon.GetID("SAJET.SYS_MODEL", "MODEL_NAME", "MODEL_ID", dr["MODEL_ID"].ToString(), "Y");
            if (sModelName == "0")
                sModelName = "";
            string sDefectNo = dr["DEFECT_NO"].ToString();
            sSQL = "SELECT A.SAMPLING_SIZE,A.FAIL_QTY "
                + "  FROM  SAJET.G_IQC_TEST_TYPE A "
                + " WHERE A.LOT_NO =:LOT_NO ";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            DataSet dsTestType = ClientUtils.ExecuteSQL(sSQL, Params);
            int iSampleSize = 0, iFailQty = 0;
            if (dsTestType.Tables[0].Rows.Count > 0)
            {
                iSampleSize = Convert.ToInt32(dsTestType.Tables[0].Rows[0]["SAMPLING_SIZE"].ToString());
            }
            for (int i = 0; i <= dsTestType.Tables[0].Rows.Count - 1; i++)
            {
                iFailQty += Convert.ToInt32(dsTestType.Tables[0].Rows[i]["FAIL_QTY"].ToString());
            }
            //add rita 2011/09/27 異常現象帶出檢驗批的不良現象代碼,描述
            sSQL = "SELECT A.DEFECT_MEMO,B.DEFECT_CODE,B.DEFECT_DESC "
                + "  FROM SAJET.G_IQC_TEST_TYPE_DEFECT A,SAJET.SYS_DEFECT B  "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.DEFECT_ID =B.DEFECT_ID(+) "
                + " ORDER BY B.DEFECT_CODE ";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            dsTestType = ClientUtils.ExecuteSQL(sSQL, Params);
            string sLotMemo = "";
            for (int i = 0; i <= dsTestType.Tables[0].Rows.Count - 1; i++)
            {
                sLotMemo = sLotMemo + dsTestType.Tables[0].Rows[i]["DEFECT_DESC"].ToString();
                if (!string.IsNullOrEmpty(dsTestType.Tables[0].Rows[i]["DEFECT_MEMO"].ToString()))
                    sLotMemo =sLotMemo+","+dsTestType.Tables[0].Rows[i]["DEFECT_MEMO"].ToString() + ";";
            }
            sLotMemo = sLotMemo.Trim(';');            
            ExportOfficeExcel.ExcelEdit ExcelClass = new ExportOfficeExcel.ExcelEdit();
            try
            {
                ExcelClass.Open(sSampleFile);
                //COLUMN,ROW
                ExcelClass.SetCellValue("Sheet1", 3, 3, "'" + sDefectNo);
                ExcelClass.SetCellValue("Sheet1", 6, 3, "'" + dr["INSP_DATE"].ToString());
                ExcelClass.SetCellValue("Sheet1", 9, 3, "'" + dr["INSP_TIME"].ToString());
                ExcelClass.SetCellValue("Sheet1", 7, 4, "'" + sModelName);
                ExcelClass.SetCellValue("Sheet1", 3, 5, "'" + dr["PART_NO"].ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 7, "'" + dr["VENDOR_NAME"].ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 8, "'" + dr["PART_NO"].ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 9, "'" + sModelName);
                ExcelClass.SetCellValue("Sheet1", 3, 11, "'" + dr["RT_NO"].ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 12, "'" + dr["RECEIVE_DATE"].ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 13, "'" + dr["LOT_SIZE"].ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 14, "'" +iSampleSize.ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 15, "'" +iFailQty.ToString());
                ExcelClass.SetCellValue("Sheet1", 3, 16, "'" + sLotMemo);
                ExcelClass.SetCellValue("Sheet1", 4, 17, "'" + dr["EMP_NAME"].ToString());
                ExcelClass.SetCellValue("Sheet1", 10, 3, "'" + sFileList.Length.ToString());
                for (int i = 0; i <= sFileList.Length - 1; i++)
                {
                    ExcelClass.SetCellValue("Sheet1", 10, i + 4, "'" + sFileList[i].ToString());
                    /*
                    if (File.Exists(sFileList[i]))
                        ExcelClass.InsertPicture("Sheet1", sFileList[i], 360, 170, 330, 175);
                     */ 
                }
                ExcelClass.RunMacro("RUN");
                if (!string.IsNullOrEmpty(g_sRejectReportPath))
                {
                    try
                    {
                        saveFileDialog1.DefaultExt = "xls";
                        saveFileDialog1.Filter = "Excel file (*.xls)|*.xls|All files (*.*)|*.*";
                        saveFileDialog1.FileName = sFileName;
                        saveFileDialog1.InitialDirectory = g_sRejectReportPath; //預設存檔路徑
                        if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                            return;
                        //saveFileDialog1.FileName = sFileName;
                        sFileName = saveFileDialog1.FileName;
                        //ExcelClass.SaveAs(g_sRejectReportPath + "\\" + sFileName);
                        ExcelClass.SaveAs(sFileName);
                    }
                    catch (Exception ex)
                    {
                        SajetCommon.Show_Message("Info:" + ex.Message ,0);
                        return;
                    }

                }
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Save MCI Report OK") + Environment.NewLine
                                        + SajetCommon.SetLanguage("File Path") + " : " + g_sRejectReportPath + Environment.NewLine
                                        + SajetCommon.SetLanguage("File Name") + " : " + sFileName, -1);
            }
            finally
            {
                ExcelClass.Close();
            }
            /*
            sSQL = " SELECT EMP_NAME FROM SAJET.SYS_EMP WHERE EMP_ID='" + ClientUtils.UserPara1 + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sEmp_Name = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString().Trim(); // 登入者名稱

            sSQL = " SELECT RT_NO FROM SAJET.G_ERP_RTNO A,SAJET.G_IQC_LOT B WHERE A.RT_ID=B.RT_ID AND B.LOT_NO='" + combLot.SelectedItem.ToString().Trim() + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sRT_NO = dsTemp.Tables[0].Rows[0]["RT_NO"].ToString().Trim(); // RT單號

            sSQL = " SELECT PARAM_VALUE FROM SAJET.SYS_BASE WHERE PARAM_NAME='MCI Path' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sMCI_Path = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString().Trim(); // 驗退異常單存放路徑

            try
            {
                sSQL = " SELECT MODEL_NAME FROM SAJET.SYS_PART A,SAJET.SYS_MODEL B WHERE A.MODEL_ID=B.MODEL_ID AND A.PART_NO='" + lablPartNo.Text.Trim() + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sModel_Name = dsTemp.Tables[0].Rows[0]["MODEL_NAME"].ToString().Trim(); // 料號品名
            }
            catch (IndexOutOfRangeException)
            {
                sModel_Name = ""; // 料號品名為空
            }

            Stream fs = new FileStream(Application.StartupPath + "\\" + g_sExeName + "\\驗退異常單.xls", FileMode.Open, FileAccess.Read);
            HSSFWorkbook workbook = new HSSFWorkbook(fs);

            workbook.GetSheetAt(0).GetRow(2).GetCell(5).SetCellValue(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day); // 日期
            workbook.GetSheetAt(0).GetRow(2).GetCell(8).SetCellValue(DateTime.Now); // 時間
            workbook.GetSheetAt(0).GetRow(3).GetCell(6).SetCellValue(sModel_Name); // 品名
            workbook.GetSheetAt(0).GetRow(4).GetCell(2).SetCellValue(lablPartNo.Text.Trim()); // 品號
            workbook.GetSheetAt(0).GetRow(6).GetCell(2).SetCellValue(lablVendor.Text.Trim()); // 廠商
            workbook.GetSheetAt(0).GetRow(7).GetCell(2).SetCellValue(lablPartNo.Text.Trim()); // 料號
            workbook.GetSheetAt(0).GetRow(8).GetCell(2).SetCellValue(sModel_Name); // 品名
            workbook.GetSheetAt(0).GetRow(10).GetCell(2).SetCellValue(sRT_NO); // 進貨單號
            workbook.GetSheetAt(0).GetRow(11).GetCell(2).SetCellValue(lablStartTime.Text.Trim()); // 進貨日期
            workbook.GetSheetAt(0).GetRow(12).GetCell(2).SetCellValue(lablLotSize.Text.Trim()); // 進貨數量

            int iSample_Size = 0;
            int iFail_Qty = 0;

            for (int i = 0; i <= dgvItemType.Rows.Count - 1; i++)
            {
                iSample_Size += Convert.ToInt32(dgvItemType.Rows[i].Cells["SAMPLE_SIZE"].Value.ToString());
                iFail_Qty += Convert.ToInt32(dgvItemType.Rows[i].Cells["FAIL_QTY"].Value.ToString());
            }

            workbook.GetSheetAt(0).GetRow(13).GetCell(2).SetCellValue(iSample_Size); // 抽驗數量
            workbook.GetSheetAt(0).GetRow(14).GetCell(2).SetCellValue(iFail_Qty); // 不良數量
            workbook.GetSheetAt(0).GetRow(15).GetCell(2).SetCellValue(sLotMemo); // 異常現象
            workbook.GetSheetAt(0).GetRow(16).GetCell(3).SetCellValue(sEmp_Name); // 提出者

            string sVendor_ID = "";

            sSQL = " SELECT VENDOR_ID FROM SAJET.SYS_VENDOR WHERE VENDOR_NAME='" + lablVendor.Text.Trim() + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            sVendor_ID = dsTemp.Tables[0].Rows[0]["VENDOR_ID"].ToString().Trim();

            string sName = "MCI-"; // 檔案命名規則

            sSQL = " SELECT SUBSTR(TO_CHAR(SYSDATE,'YYYYMM'),3,4) AS YYMM FROM DUAL ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            sName += dsTemp.Tables[0].Rows[0]["YYMM"].ToString().Trim();

            sSQL = " SELECT LPAD(COUNT(*) + 1,2,'0') AS XX "
                 + " FROM   SAJET.G_IQC_LOT "
                 + " WHERE  QC_RESULT ='1' "
                 + " AND    VENDOR_ID ='" + sVendor_ID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            sName += dsTemp.Tables[0].Rows[0]["XX"].ToString().Trim();

            sName += "-" + lablVendor.Text.Trim();

            workbook.GetSheetAt(0).GetRow(2).GetCell(2).SetCellValue(sName); // 編號

            // 參考照片
            /*
            sSQL = " SELECT UPLOAD_FILE FROM SAJET.G_IQC_LOT_IMAGE WHERE LOT_NO='" + combLot.SelectedItem.ToString().Trim() + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            HSSFPatriarch patriarch = workbook.GetSheetAt(0).CreateDrawingPatriarch();

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                byte[] bytes = (byte[])dsTemp.Tables[0].Rows[i]["UPLOAD_FILE"];
                int j = workbook.AddPicture(bytes, HSSFWorkbook.PICTURE_TYPE_JPEG);
                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 1023, 0, 5 + i, 7 + i, 6 + i, 14 + i);
                HSSFPicture pict = patriarch.CreatePicture(anchor, j);
            }

            HSSFClientAnchor anchor_adlink = new HSSFClientAnchor(0, 0, 1023, 0, 1, 0, 3, 2);
            byte[] bytes_adlink = System.IO.File.ReadAllBytes(Application.StartupPath + "/Image/adlink.jpg");
            int i_adlink = workbook.AddPicture(bytes_adlink, HSSFWorkbook.PICTURE_TYPE_JPEG);
            HSSFPicture pict2 = patriarch.CreatePicture(anchor_adlink, i_adlink);
            */
            /*
            Stream file = new FileStream(sMCI_Path + sName + ".xls", FileMode.OpenOrCreate);

            workbook.Write(file);
            file.Close();
             */ 
        }


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sbtnHistory_Click(object sender, EventArgs e)
        {

        }

        private void sbtnRollback_Click(object sender, EventArgs e) //復原
        {
            string sRTNo = combLotNo.Text;
            string sRTSeq = combItemSeq.Text;
            int iIndex = -1;

            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLotNo.SelectAll();
                return;
            }
            if (G_rIQC.sHoldFlag == "1") //暫停
            {
                SajetCommon.Show_Message("Lot Status is hold. Please unhold this lot first", 0);
                combLotNo.SelectAll();
                return;
            }
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Rollback Lot No", 1) + " : " + G_rIQC.sLotNo + " ?", 2) != DialogResult.Yes)
                return;

            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_LOT_ROLLBACK", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

            if (sResult != "OK")
            {
                ClientUtils.ShowMessage(sResult, 0);
                return;
            }

            clearData();
            getIQCLot();
            iIndex = combLotNo.Items.IndexOf(sRTNo);

            if (iIndex >= 0)
            {
                combLotNo.SelectedIndex = iIndex;
            }

            iIndex = combItemSeq.Items.IndexOf(sRTSeq);

            if (iIndex >= 0)
            {
                combItemSeq.SelectedIndex = iIndex;
            }

//            picStatus.Hide();
        }

        private void btnStartRoHS_Click(object sender, EventArgs e)
        {
            if (!G_rIQC.bLotInput)
            {
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Start RoHS")+" ?", 2) != DialogResult.Yes)
                return;

            UpdateRoHSStatus("1");
            btnStartRoHS.Enabled = false;
            btnStopRoHS.Enabled = true;
            lablRoHSOn.Visible = LotInspRoHS();
        }

        private void UpdateRoHSStatus(string sStatus)
        {
            object[][] Params = new object[6][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo  };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", G_rIQC.sPartID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVENDORID", G_rIQC.sVendorID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSTATUS", sStatus };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_ROHS_CONTROL", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult, 0);
                return;
            }

            /*
            sSQL = "SELECT STATUS FROM SAJET.G_IQC_ROHS_CTL "
                + " WHERE PART_ID =:PART_ID "
                + "   AND VENDOR_ID =:VENDOR_ID "
                + "   AND ROWNUM = 1 ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", G_rIQC.sVendorID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = "INSERT INTO SAJET.G_IQC_ROHS_CTL "
                    + "(PART_ID,VENDOR_ID,STATUS,UPDATE_USERID,UPDATE_TIME,CTL_FLAG,CTL_START_TIME ) "
                    + " VALUES "
                    + "(:PART_ID,:VENDOR_ID,:STATUS,:UPDATE_USERID,SYSDATE,DECODE(:STATUS,'0','N/A','1'),DECODE(:STATUS,'0',NULL,SYSDATE) ) ";
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", G_rIQC.sVendorID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STATUS", sStatus };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                sSQL = " UPDATE SAJET.G_IQC_ROHS_CTL "
                    + "    SET STATUS =:STATUS "
                    + "       ,UPDATE_USERID =:UPDATE_USERID "
                    + "       ,UPDATE_TIME = SYSDATE "
                    + "       ,CTL_FLAG = DECODE(:STATUS,'0','N/A','1') "
                    + "       ,CTL_START_TIME = DECODE(:STATUS,'0',NULL,SYSDATE) "
                    + "  WHERE PART_ID =:PART_ID "
                    + "    AND VENDOR_ID =:VENDOR_ID ";
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STATUS", sStatus };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", G_rIQC.sVendorID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
             */ 
        }
        
        private void btnStopRoHS_Click(object sender, EventArgs e)
        {
            if (!G_rIQC.bLotInput)
            {
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Stop RoHS")+" ?", 2) != DialogResult.Yes)
                return;

            UpdateRoHSStatus("0");
            btnStartRoHS.Enabled = true;
            btnStopRoHS.Enabled = false;
            lablRoHSOn.Visible = LotInspRoHS();
        }

        private void btnTypeAppend_Click(object sender, EventArgs e)
        {

        }

        private void btnTypeModify_Click(object sender, EventArgs e)
        {


        }

        private void btnTypeDelete_Click(object sender, EventArgs e)
        {

        }

        private void tsbtnRoHSPass_Click(object sender, EventArgs e)
        {

        }

        private void btnRoHSOK_Click(object sender, EventArgs e) //RoHS檢驗結果
        {
            if (dgvRoHSItem.Rows.Count == 0)
                return;

            editRoHSMemo.Text = editRoHSMemo.Text.Trim();
            string sStatus = (sender as Button).Tag.ToString(); //OK or NG
            string sRoHSResult = (sender as Button).Text;
            object[][] Params = new object[7][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", G_rIQC.sPartID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVENDORID", G_rIQC.sVendorID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQCRESULT", sStatus };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", editRoHSMemo.Text };
            Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_TRANSFER_ROHS", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult, 0);
                return;
            }

            //不允許再維護ROHS的項目內容

            PanelRoHS.Enabled = false;
            btnRoHSOK.Visible = PanelRoHS.Enabled;
            btnRoHSNG.Visible = PanelRoHS.Enabled;
            lablRoHSResult.Visible = (!PanelRoHS.Enabled);
            lablRoHSResult.Text = sRoHSResult;
            lablRoHSResult.BackColor = Color.Green;
            lablRoHSResult.ForeColor = Color.White;
            btnRollbackRoHs.Visible = lablRoHSResult.Visible;

            if (sRoHSResult == "NG")
            {
                lablRoHSResult.BackColor = Color.Red;
                lablRoHSResult.ForeColor = Color.Yellow;
            }
        }

        private void btnAppend_Click(object sender, EventArgs e) //新增RoHS
        {
            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLotNo.SelectAll();
                return;
            }

            fRoHSItem fData = new fRoHSItem();

            try
            {
                fData.g_sLotNo = G_rIQC.sLotNo;
                fData.g_sUpdateType = "APPEND";
                fData.ShowDialog();
                GetRoHSItem();
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void btnModify_Click(object sender, EventArgs e) //修改RoHS
        {
            if (dgvRoHSItem.Rows.Count == 0 || dgvRoHSItem.CurrentRow == null || dgvRoHSItem.SelectedCells.Count == 0)
            {
                return;
            }

            fRoHSItem fData = new fRoHSItem();

            try
            {
                fData.g_sLotNo = G_rIQC.sLotNo;
                fData.g_sPosition = dgvRoHSItem.CurrentRow.Cells["POSITION"].Value.ToString();
                fData.g_sUpdateType = "MODIFY";
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    GetRoHSItem();
                }

            }
            finally
            {
                fData.Dispose();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) //刪除RoHS
        {
            if (dgvRoHSItem.Rows.Count == 0 || dgvRoHSItem.CurrentCell == null || dgvRoHSItem.SelectedCells.Count == 0)
            {
                return;
            }

            string sPosition = dgvRoHSItem.CurrentRow.Cells["POSITION"].Value.ToString();

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Position ?", 1) + " " + sPosition, 2) != DialogResult.Yes)
                return;

            sSQL = "DELETE SAJET.G_IQC_ROHS_ITEM "
                + " WHERE LOT_NO=:LOT_NO "
                + "   AND POSITION=:POSITION ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "POSITION", sPosition };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            GetRoHSItem();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnNotes_Click(object sender, EventArgs e) //注意事項
        {

        }

        private void btnSaveTestTypeMemo_Click(object sender, EventArgs e) //備註按鈕
        {
            editTestTypeMemo.Text = editTestTypeMemo.Text.Trim();

            if (G_rIQC.sSelTypeID == "0")
            {
                return;
            }

            int iSampleID = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_ID"].Value.ToString());
            int iSampleLevel = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_LEVEL_ID"].Value.ToString());
            int iSampleSize = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_SIZE"].Value.ToString());
            object[][] Params = new object[8][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_rIQC.sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", G_rIQC.sSelTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTESTTYPEMEMO", editTestTypeMemo.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLEID", iSampleID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLELEVEL", iSampleLevel };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLESIZE", iSampleSize };
            Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_INPUT_TESTTYPE_MEMO", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult, 0);
                return;
            }

            SajetCommon.Show_Message("Save Memo OK", 3);
        }

        private void button1_Click(object sender, EventArgs e) //批號備註
        {
            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLotNo.SelectAll();
                return;
            }

            int iOKItem = 0; //OK大項數量
            int iNGItem = 0; //NG大項數量

            for (int i = 0; i <= dgvItemType.Rows.Count - 1; i++) //計算OK/NG大項數量
            {
                string sQCResult = dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString();
                if (sQCResult == "OK")
                    iOKItem += 1;
                if (sQCResult == "NG")
                    iNGItem += 1;
            }

            fLotMemo formMemo = new fLotMemo();

            try
            {
                formMemo.g_sExeName = g_sExeName;
                formMemo.g_sType = "0";
                formMemo.initial(G_rIQC.sLotNo, G_rIQC.sQCResult, G_rIQC.iLotSize.ToString(), iOKItem, iNGItem, DateInsp.Value.ToString("yyyy/MM/dd"), 0);
                
                if (formMemo.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                int  iInspWHour = Convert.ToInt32(formMemo.editInspWorkHours.Text);
                string sLot = formMemo.editLot.Text;
                string sDateCode = formMemo.editDateCode.Text;
                string sMaterialVer = formMemo.editMaterialVer.Text;
                string sDMDA = formMemo.editDMDA.Text;
                string sLotMemo = formMemo.editLotMemo.Text;
                string sLotLevel = formMemo.g_sLotLevel;
                //string sMRBMemo = formMemo.editMRB.Text;
                sSQL = "UPDATE SAJET.G_IQC_LOT "
                    + "   SET LOT ='" + sLot + "' "
                    + "      ,DATECODE='" + sDateCode + "' "
                    + "      ,MATERIAL_VER ='" + sMaterialVer + "' "
                    + "      ,DMDA ='" + sDMDA + "' "
                    + "      ,INSP_WORKHOURS ='" + iInspWHour.ToString() + "' "
                    + "      ,LOT_MEMO ='" + sLotMemo + "' "
                    + "      ,EMP_ID = '" + ClientUtils.UserPara1 + "' "
                    + "      ,LOT_LEVEL ='" + sLotLevel + "' "
                    //+ "      ,MRB_MEMO ='" + sMRBMemo + "' "
                    + "  WHERE LOT_NO ='" + G_rIQC.sLotNo + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Lot No") + " : " + G_rIQC.sLotNo + Environment.NewLine
                                      + SajetCommon.SetLanguage("Save OK"), 3);
            }
            finally
            {
                formMemo.Dispose();
            }
        }

        private void combLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
            if (e.KeyChar != (char)Keys.Return)
                return;
            combLotNo.Text = combLotNo.Text.Trim();

            int iIndex = combLotNo.Items.IndexOf(combLotNo.Text);
            if (iIndex < 0)
            {
                SajetCommon.Show_Message("RT No Error", 0);
                combLotNo.Focus();
                combLotNo.SelectAll();
                return;
            }
            combLotNo.SelectedIndex = iIndex;
             */ 
        }

        private void btnRollbackRoHs_Click(object sender, EventArgs e)
        {
            string sRTNo = combLotNo.Text;
            string sRTSeq = combItemSeq.Text;
            int iIndex = -1;

            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLotNo.SelectAll();
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Rollback")+" "+SajetCommon.SetLanguage("RoHS")+" ?"+Environment.NewLine
                                       + SajetCommon.SetLanguage("Lot No", 1) + " : " + G_rIQC.sLotNo ,2)!=DialogResult.Yes)

                return;

            sSQL = "UPDATE SAJET.G_IQC_ROHS_RESULT "
                + "   SET END_TIME = NULL "
                + "      ,QC_RESULT='N/A' "
                + "      ,UPDATE_USERID =:UPDATE_USERID "
                + "      ,UPDATE_TIME = SYSDATE "
                + "  WHERE LOT_NO =:LOT_NO ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            clearData();
            getIQCLot();

            iIndex = combLotNo.Items.IndexOf(sRTNo);

            if (iIndex >= 0)
            {
                combLotNo.SelectedIndex = iIndex;
            }

            iIndex = combItemSeq.Items.IndexOf(sRTSeq);

            if (iIndex >= 0)
            {
                combItemSeq.SelectedIndex = iIndex;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLotNo.SelectAll();
                return;
            }
            if (combItemSeq.Text == combItemSeq1.Text)
            {
                return;
            }
            if (dgvRoHSItem.Rows.Count == 0)
                return;

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Copy RoHS Data to RT Item") + " : " + combItemSeq1.Text + " ? ", 2) != DialogResult.Yes)
                return;

            string sNewLotNo = combLotNo.Text + "-" + combItemSeq1.Text;
            sSQL = " INSERT INTO SAJET.G_IQC_ROHS_ITEM "
                  + "  (LOT_NO, POSITION,PB,CD,HG,CR,BR,CL,MEMO,UPDATE_USERID,UPDATE_TIME) "
                  + " SELECT :NEW_LOT_NO,POSITION,PB,CD,HG,CR,BR,CL,MEMO ,:UPDATE_USERID,SYSDATE "
                  + "  FROM SAJET.G_IQC_ROHS_ITEM "
                  + " WHERE LOT_NO =:LOT_NO ";
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEW_LOT_NO", sNewLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            SajetCommon.Show_Message(SajetCommon.SetLanguage("Copy Success"),3);
            combItemSeq1.SelectedIndex = -1;
        }

        private void PanelRoHS_Paint(object sender, PaintEventArgs e)
        {

        }
        private void TestItemSNInfo(string g_sLotNo)
        {
            dgvSN.Rows.Clear();
            string sSQL = "SELECT A.LOT_NO,A.SERIAL_NUMBER,A.DATECODE,A.TEST_VALUE,A.REMARK,A.CREATE_TIME,B.EMP_NAME "
                        + "FROM SAJET.G_IQC_SN A, SAJET.SYS_EMP B "
                        + "WHERE LOT_NO ='" + g_sLotNo + "' "
                        + "AND A.CREATE_USERID = B.EMP_ID ";
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);            

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                dgvSN.Rows.Add();
                dgvSN.Rows[dgvSN.Rows.Count - 1].Cells["SERIAL_NUMBER"].Value = dr["SERIAL_NUMBER"].ToString();
                dgvSN.Rows[dgvSN.Rows.Count - 1].Cells["DATECODE"].Value = dr["DATECODE"].ToString();
                dgvSN.Rows[dgvSN.Rows.Count - 1].Cells["TEST_VALUE"].Value = dr["TEST_VALUE"].ToString();
                dgvSN.Rows[dgvSN.Rows.Count - 1].Cells["REMARK"].Value = dr["REMARK"].ToString();
                dgvSN.Rows[dgvSN.Rows.Count - 1].Cells["CREATE_TIME"].Value = dr["CREATE_TIME"].ToString();
                dgvSN.Rows[dgvSN.Rows.Count - 1].Cells["CREATE_USER"].Value = dr["EMP_NAME"].ToString();
            }
        }
//==========================================================================================         
        private void combLot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sMsgKeypass)
                sMsgCount++;
            try
            {
                if (combLot.SelectedIndex != -1)
                {
                    G_rIQC.bLotInput = false;
                    clearData();
                    getIQCLotData(combLot.Text); //Lot No 基本資訊
                    getRTData(); //Lot No中所包含之RT
                    GetCriticalPart();
                    GetSYSTestType(); //料號+廠商測試項目
                    GetIQCTestType(); //有檢驗結果的測試項目                    
                    GetDefaultSampleType(); //沒有針對供應商設抽驗計畫，會從系統找一組預設的                    
                    GetRoHSCtl();
                    GetRoHSItem();
                    TestItemSNInfo(combLot.Text); //批號之檢驗序號明細
                    
                   // ShowComponentPic(); //顯示零件圖片 2013/3/10 Modify By Kobe
                   // ShowComponentSizePic(); //顯示重點尺寸圖片 2015/6/23 add by rita
                    btnStartRoHS.Enabled = (!G_rIQC.bRoHSOn);
                    btnStopRoHS.Enabled = G_rIQC.bRoHSOn;
                    PanelRoHS.Enabled = (!G_rIQC.bRoHSFinish);
                    btnRoHSOK.Visible = PanelRoHS.Enabled;
                    btnRoHSNG.Visible = PanelRoHS.Enabled;
                    lablRoHSResult.Visible = (G_rIQC.bRoHSFinish);
                    lablRoHSResult.BackColor = Color.Green;
                    lablRoHSResult.ForeColor = Color.White;
                    btnRollbackRoHs.Visible = lablRoHSResult.Visible;

                    if (lablRoHSResult.Text == "NG")
                    {
                        lablRoHSResult.BackColor = Color.Red;
                        lablRoHSResult.ForeColor = Color.Yellow;
                    }

                    lablRoHSOn.Visible = LotInspRoHS();
                    //    editRoHSResult.Visible = (G_rIQC.bRoHSFinish);

                    SetSelectRow(dgvItemType, "", "TYPE_ID");
                    sbtnPass.Enabled = (G_rIQC.sHoldFlag == "0");
                    sbtnReject.Enabled = sbtnPass.Enabled;
                    sbtnWaive.Enabled = sbtnPass.Enabled;
                    sbtnByPass.Enabled = sbtnPass.Enabled;
                    sbtnSpecialWaive.Enabled = sbtnPass.Enabled;
                    //                PanelRoHS.Enabled = sbtnPass.Enabled;
                    //                PanelRoHSResult.Enabled = sbtnPass.Enabled;
                    inspectItemToolStripMenuItem.Enabled = sbtnPass.Enabled;
                    sbtnHold.Enabled = true;
                    sbtnHold.Text = SajetCommon.SetLanguage("Hold", 1);

                    if (G_rIQC.sHoldFlag == "1")
                    {
                        sbtnHold.Text = SajetCommon.SetLanguage("Unhold", 1);
                        //sbtnHold.Enabled = (g_iPrivilege == 2);
                    }

                    G_rIQC.bLotInput = true;

                    if (G_rIQC.sQCResult != "N/A")
                    {
                        labResult.Text = G_rIQC.sQCResult;
                        sbtnPass.Enabled = false;
                        sbtnReject.Enabled = false;
                        sbtnWaive.Enabled = false;
                        sbtnByPass.Enabled = false;
                        sbtnHold.Enabled = false;
                    }

                    if (G_rIQC.sQCResultCode == "2")
                        btWaiveResult.Visible = true;
                    else
                        btWaiveResult.Visible = false;
                }
            }
            finally
            {
                tsbtnTooling.Text = SajetCommon.SetLanguage("Tooling") + " (" + GetTooling(G_rIQC.sLotNo).ToString() + ")";
                tsbtnNote.Text = SajetCommon.SetLanguage("Note") + " (" + GetPartNoteCount(G_rIQC.sPartID).ToString() + ")";
                
                lablPartNo.Text = G_rIQC.sPartNo;
                lablSpec1.Text = G_rIQC.sPartSpec;
                lablVendor.Text = G_rIQC.sVendorNane;
                lablLotSize.Text = G_rIQC.iLotSize.ToString();
                lablStartTime.Text = G_rIQC.sStartTime;
                lablEndTime.Text = G_rIQC.sTargetTime;
                ckbUrgent.Checked = (G_rIQC.sUrgentType == "Y");
                //-------20110916-------------------------------
                lablSpecA.Text = G_rIQC.sSpecA; 
                lablSpecB.Text = G_rIQC.sSpecB;
                lablModelName.Text = G_rIQC.sMName;
                //---------------------------------------------
                lablUrgentType.Text = G_rIQC.sUrgentType;
                btnRDAdmit.Enabled = (G_rIQC.sRDFlag == "Y");

                string sSamplingMsg = "";
                for (int i = 0; i <= dgvItemType.Rows.Count - 1; i++)
                {
                    string sPriorSampleID = string.Empty, sPriorSampleType = string.Empty; int sPriorSampleLevel = 0;
                    if (sMsgCount == 1)
                    {
                        sNowTypeID = dgvItemType.Rows[i].Cells["TYPE_ID"].Value.ToString();
                        string sTypeName= dgvItemType.Rows[i].Cells["TYPE_NAME"].Value.ToString();
                        if (GetPriorSamplingLevel(sNowTypeID, ref sPriorSampleLevel, ref sPriorSampleID, ref sPriorSampleType))
                        {
                            if (sNowSamplingLevel != sPriorSampleLevel || sNowSamplingID != sPriorSampleID)
                            {
                                sSamplingMsg = sSamplingMsg + SajetCommon.SetLanguage("[") + sTypeName + "]" + Environment.NewLine
                                             + SajetCommon.SetLanguage("Sampling Plan") + " : " + sPriorSampleType + " " + SajetCommon.SetLanguage("Level") + " : " + G_rIQC.lstSampleLevel[sPriorSampleLevel] + Environment.NewLine
                                             + SajetCommon.SetLanguage("Change") + Environment.NewLine
                                             + SajetCommon.SetLanguage("Sampling Plan") + " : " + sNowSamplingType + " " + SajetCommon.SetLanguage("Level") + " : " + G_rIQC.lstSampleLevel[sNowSamplingLevel] + Environment.NewLine;

                            }
                        }
                    }
                }
                if (sSamplingMsg != "")
                    SajetCommon.Show_Message(sSamplingMsg, 1);
                sMsgCount = 0;
                sMsgKeypass = false;
            }
            /*
            string sSQL = "";
            string sUrgent_Type = "";
            DataSet dsTemp = new DataSet();

            try
            {
                sSQL = " SELECT URGENT_TYPE "
                     + " FROM   SAJET.G_IQC_LOT "
                     + " WHERE  LOT_NO='" + combLot.SelectedItem.ToString().Trim() + "'";

                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sUrgent_Type = dsTemp.Tables[0].Rows[0]["URGENT_TYPE"].ToString();

                if (sUrgent_Type == "Y")
                {
                    ckbUrgent.Checked = true;
                }
                else
                {
                    ckbUrgent.Checked = false;
                }
               
            }
            catch (Exception)
            {
             //   this.fMain_Load(sender, e);
            }
             */ 
        }
        private void GetCriticalPart()
        {
            timer1.Enabled = false;
            G_rIQC.bCriticalPart = false;
            string sSQL = "SELECT PART_TYPE "
                      + " FROM SAJET.SYS_PART_CRITICAL "
                      + " WHERE PART_ID =:PART_ID "
                      + "   AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_rIQC.sPartID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                lablCriticalPartType.Text = SajetCommon.SetLanguage("Safety") + " : " + dsTemp.Tables[0].Rows[0]["PART_TYPE"].ToString();
                G_rIQC.bCriticalPart = true;
            }
           // panelCriticalPart.Visible = G_rIQC.bCriticalPart;
            timer1.Enabled = G_rIQC.bCriticalPart;
            lablCriticalPartType.Visible = G_rIQC.bCriticalPart;
            lablCriticalPartType.BackColor = Color.Orange;
            if (G_rIQC.bCriticalPart)
                lablVendor.Size = new Size(175, 18);
            else
                lablVendor.Size = new Size(330, 18);
        }
        private void getRTData()
        {
//            if (combLot.SelectedIndex != -1)
//            {
//                dgRT.Rows.Clear();
//                object[][] Params;
//                DataSet ds;
//                string sSQL = @"SELECT A.RT_NO,A.PO_NO,INCOMING_QTY,B.RT_SEQ,B.RECEIVE_QTY,B.REJECT_QTY
//                            FROM SAJET.G_ERP_RTNO A, SAJET.G_ERP_RT_ITEM B
//                            WHERE A.RT_ID=B.RT_ID AND B.LOT_NO = :LOT_NO
//                            ORDER BY A.RT_NO";
//                Params = new object[1][];
//                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", combLot.Text };
//                ds = ClientUtils.ExecuteSQL(sSQL, Params);
//                foreach (DataRow dr in ds.Tables[0].Rows)
//                {
//                    dgRT.Rows.Add();
//                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[0].Value = dr["RT_NO"].ToString();
//                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[1].Value = dr["PO_NO"].ToString();
//                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[2].Value = dr["INCOMING_QTY"].ToString();
//                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[3].Value = dr["RT_SEQ"].ToString();
//                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[4].Value = dr["RECEIVE_QTY"].ToString();
//                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[5].Value = dr["REJECT_QTY"].ToString();
//                }
//            }

            if (combLot.SelectedIndex != -1)
            {
                string sSQL = "SELECT NVL(C.EMP_NO,B.RT_CREATE_EMPNO) EMP_NO, C.EMP_NAME,B.RT_DEPT  "
                           + " FROM SAJET.G_IQC_LOT A ,SAJET.G_ERP_RTNO B ,SAJET.SYS_EMP C "
                           + " WHERE A.LOT_NO=:LOT_NO "
                           + "  AND A.RT_ID = B.RT_ID "
                           + "  AND B.RT_CREATE_EMPNO = C.EMP_NO(+)  "
                           + "  AND ROWNUM = 1 ";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", combLot.Text };
                DataSet  dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    lablCreateEmp.Text = "["+dsTemp.Tables[0].Rows[0]["EMP_NO"].ToString() + " " + dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString()+"]"
                                        + "/[" + dsTemp.Tables[0].Rows[0]["RT_DEPT"].ToString() + "]";

                }
                 
                           
                /*
                dgRT.Rows.Clear();
                object[][] Params;
                DataSet ds;
                string sSQL = @"SELECT A.RT_NO,A.PO_NO,INCOMING_QTY,B.RT_SEQ
                            FROM SAJET.G_ERP_RTNO A, SAJET.G_ERP_RT_ITEM B
                            WHERE A.RT_ID=B.RT_ID AND B.LOT_NO = :LOT_NO
                            ORDER BY A.RT_NO";
                Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", combLot.Text };
                ds = ClientUtils.ExecuteSQL(sSQL, Params);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dgRT.Rows.Add();
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[0].Value = dr["RT_NO"].ToString();
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[1].Value = dr["PO_NO"].ToString();
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[2].Value = dr["INCOMING_QTY"].ToString();
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[3].Value = dr["RT_SEQ"].ToString();
                }
                 */ 
            }
        }

        private void btERP_Click(object sender, EventArgs e) //上傳ERP
        {
            int flag = 0;

            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLot.SelectAll();
                return;
            }
            if(G_rIQC.sQCResult=="N/A")
            {
                SajetCommon.Show_Message("Inspection Result unknow", 0);
                combLot.SelectAll();
                return;
            }
            if (G_rIQC.sQCResultCode == "2")  //檢查特採是否有檢驗結果
            {
                for (int i = dgRT.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgRT.Rows[i].Cells[4].Value.ToString() != "" && dgRT.Rows[i].Cells[5].Value.ToString() != "") 
                        flag++;
                }

                if (flag != dgRT.Rows.Count)
                {
                    SajetCommon.Show_Message("Not input waive result", 0);
                    combLot.SelectAll();
                    return;
                }
            }

            if (G_rIQC.sQCResultCode == "1" || G_rIQC.sQCResultCode == "2") //批退 OR 特採
            {
                if (SajetCommon.Show_Message("Upload ERP?", 2) == DialogResult.Yes)
                {
                    uploadERP(Convert.ToInt32(G_rIQC.sQCResultCode));
                }
            }
        }

        private void uploadERP(int result)
        {
            string sRes = "";
            DataSet ds;
            object[][] Params;

            for (int i = dgRT.Rows.Count - 1; i >= 0; i--)
            {
                if (result == 2) //特採
                {
                    Params = new object[10][];
                    Params[0] = new object[] { "INPUT", "1", "TLOTNO", combLot.Text };
                    Params[1] = new object[] { "INPUT", "1", "TTYPE", result};
                    Params[2] = new object[] { "INPUT", "1", "TPART", G_rIQC.sPartNo };
                    Params[3] = new object[] { "INPUT", "1", "TVENDOR", G_rIQC.sVendorCode };
                    Params[4] = new object[] { "INPUT", "1", "TRTNO", dgRT.Rows[i].Cells[0].Value.ToString() };
                    Params[5] = new object[] { "INPUT", "1", "TRTSEQ", dgRT.Rows[i].Cells[3].Value.ToString() };
                    Params[6] = new object[] { "INPUT", "1", "TQTY", Convert.ToInt32(dgRT.Rows[i].Cells[2].Value.ToString()) };
                    Params[7] = new object[] { "INPUT", "1", "TACCEPT", Convert.ToInt32(dgRT.Rows[i].Cells[4].Value) };
                    Params[8] = new object[] { "INPUT", "1", "TREJECT", Convert.ToInt32(dgRT.Rows[i].Cells[5].Value) };
                    Params[9] = new object[] { "OUTPUT", "1", "TRES", "" };     
                }
                else  //全部允收0 批退1 免驗4
                {
                    Params = new object[10][];
                    Params[0] = new object[] { "INPUT", "1", "TLOTNO", combLot.Text };
                    Params[1] = new object[] { "INPUT", "1", "TTYPE", result }; 
                    Params[2] = new object[] { "INPUT", "1", "TPART", G_rIQC.sPartNo };
                    Params[3] = new object[] { "INPUT", "1", "TVENDOR", G_rIQC.sVendorCode };
                    Params[4] = new object[] { "INPUT", "1", "TRTNO", dgRT.Rows[i].Cells[0].Value.ToString() };
                    Params[5] = new object[] { "INPUT", "1", "TRTSEQ", dgRT.Rows[i].Cells[3].Value.ToString() };
                    Params[6] = new object[] { "INPUT", "1", "TQTY", Convert.ToInt32(dgRT.Rows[i].Cells[2].Value.ToString()) };
                    Params[7] = new object[] { "INPUT", "1", "TACCEPT", Convert.ToInt32(dgRT.Rows[i].Cells[2].Value) };
                    Params[8] = new object[] { "INPUT", "1", "TREJECT", Convert.ToInt32(dgRT.Rows[i].Cells[2].Value) };
                    Params[9] = new object[] { "OUTPUT", "1", "TRES", "" };   
                }
                ds = ClientUtils.ExecuteProc("SAJET.SJ_IQC_UPLOAD_ERP", Params);
                sRes = ds.Tables[0].Rows[0]["TRES"].ToString();

                if (sRes != "OK")
                {
                    SajetCommon.Show_Message(sRes, 0);
                }
            }
            if (sRes != "OK")
            {
                SajetCommon.Show_Message(sRes, 0);
            }
            else
            {
                SajetCommon.Show_Message("Upload ERP Success", 3);
                clearData();
                getIQCLot();
            }
        }

        private void importTestResultToolStripMenuItem_Click(object sender, EventArgs e) //輸入測試大項結果
        {
            if (G_rIQC.sSelTypeID == "0")
            {
                return;
            }
            fTestResult formType = new fTestResult();
            formType.g_sItemTypeID = G_rIQC.sSelTypeID;
            formType.g_iLotSize = G_rIQC.iLotSize;
            formType.g_sItemTypeName = G_rIQC.sSelTypeName;
            formType.g_sLot = G_rIQC.sLotNo;
            formType.g_sSampleID = dgvSampleType.CurrentRow.Cells["TSAMPLE_ID"].Value.ToString();
            formType.g_sSampleLevelID = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_LEVEL_ID"].Value.ToString());

            formType.g_sSampleSize = Convert.ToInt32(dgvSampleType.CurrentRow.Cells["TSAMPLE_SIZE"].Value.ToString());
            formType.g_sUserID = g_sUserID;
            formType.g_sSampleType = dgvSampleType.Rows[0].Cells["TSAMPLE_TYPE"].Value.ToString();
            formType.g_sSampleLevel = dgvSampleType.Rows[0].Cells["TSAMPLE_LEVEL"].Value.ToString();
            formType.g_sProgram = g_sProgram;


            if (G_rIQC.sPARTTYPE != "N/A")
            {
                formType.g_sPARTTYPE = G_rIQC.sPARTTYPE;
                formType.g_sFlag = "PART_TYPE";
            }
            else
            {
                formType.g_sFlag = "PART";
                formType.g_sSRECID = G_rIQC.sRECID;
            }

           // formType.g_sSRECID = G_rIQC.sRECID;
           // formType.g_sPARTTYPE = G_rIQC.sPARTTYPE;
            if (formType.ShowDialog() != DialogResult.OK) return;
            combLot_SelectedIndexChanged(sender, e);
        }

        private void btnShowRT_Click(object sender, EventArgs e) //Show RT
        {
            string sLotNo = combLot.Text.Trim() + "%";
            sSQL = "SELECT A.LOT_NO,B.PART_NO FROM SAJET.G_IQC_LOT A,SAJET.SYS_PART B  "
                + " WHERE A.LOT_NO like '" + sLotNo + "' "
                + "   AND A.QC_RESULT='N/A' "
                + "   AND A.PART_ID = B.PART_ID "
                + "   AND A.INSP_LOT = 'Y' " //2012/02/15 增加判斷是否要驗，不需驗就不顯示
                + " ORDER BY A.LOT_NO ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

           
            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                sLotNo = f.dgvData.CurrentRow.Cells["LOT_NO"].Value.ToString();
                int iIndex = combLot.Items.IndexOf(sLotNo);
                if (iIndex < 0)
                {
                    btnRefresh_Click(sender, e);
                }
                iIndex = combLot.Items.IndexOf(sLotNo);
                if (iIndex >= 0)
                {
                    combLot.SelectedIndex = iIndex;
                }            
            }
            /*
            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLot.SelectAll();
                return;
            }

            fShowRT show = new fShowRT();
            show.g_sLot = G_rIQC.sLotNo;
            show.ShowDialog();
             */ 
        }

        private void btWaiveResult_Click(object sender, EventArgs e) //輸入特採結果按鈕
        {
            if (G_rIQC.sQCResultCode == "2")
            {
                int iIndex = -1;
                string sLot = combLot.Text;
                fWaive waive = new fWaive();
                waive.g_LotNo = combLot.Text;
                waive.g_Tag = btWaiveResult.Tag.ToString();

                if (waive.ShowDialog() != DialogResult.OK)
                {
                    clearData();
                    getIQCLot();
                    iIndex = combLot.Items.IndexOf(sLot);
                    if (iIndex >= 0)
                    {
                        combLot.SelectedIndex = iIndex;
                    }
                    return;
                }
                clearData();
                getIQCLot();
                iIndex = combLot.Items.IndexOf(sLot);

                if (iIndex >= 0)
                {
                    combLot.SelectedIndex = iIndex;
                }
            }
        }

        private void saveQTY(int iResult) //批退儲存Accept & Reject QTY
        {
            DataSet ds;
            string sRes = "";

            for (int i = dgRT.Rows.Count - 1; i >= 0; i--)
            {
                object[][] Params = new object[5][];
                Params[0] = new object[] { "INPUT", "1", "TRTNO", dgRT.Rows[i].Cells[0].Value.ToString() };
                Params[1] = new object[] { "INPUT", "1", "TRTSEQ", dgRT.Rows[i].Cells[3].Value.ToString() };
                if(iResult == 1)
                    Params[2] = new object[] { "INPUT", "1", "TACCEPT", 0 };
                else
                    Params[2] = new object[] { "INPUT", "1", "TACCEPT", Convert.ToInt32(dgRT.Rows[i].Cells[2].Value) };
                if(iResult == 1)
                    Params[3] = new object[] { "INPUT", "1", "TREJECT", Convert.ToInt32(dgRT.Rows[i].Cells[2].Value) };
                else
                    Params[3] = new object[] { "INPUT", "1", "TREJECT", 0 };
                Params[4] = new object[] { "OUTPUT", "1", "TRES", "" };
                ds = ClientUtils.ExecuteProc("SAJET.SJ_IQC_SAVE_QTY", Params);
                sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    SajetCommon.Show_Message(sRes, 0);
                    return;
                }
            }
        }

        private void btnTooling_Click(object sender, EventArgs e)
        {
  
        }

        private void btnMPN_Click(object sender, EventArgs e)
        {
           
        }

        private void ckbUrgent_CheckedChanged(object sender, EventArgs e)
        {
            if (G_rIQC.sLotNo == "N/A")
                return;
            string sSQL = "";
            DataSet dsTemp = new DataSet();

            try
            {
                string sUrgendType = "N";
                if (ckbUrgent.Checked == true)
                {
                    sUrgendType = "Y";

                }
                if (G_rIQC.sUrgentType == sUrgendType)
                    return;
                
                G_rIQC.sUrgentType = sUrgendType;
                sSQL = " UPDATE SAJET.G_IQC_LOT "
                      + "   SET URGENT_TYPE=:URGENT_TYPE "
                      + " WHERE LOT_NO=:LOT_NO ";
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "URGENT_TYPE", sUrgendType };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO",G_rIQC.sLotNo };
                dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);

                sSQL = "SELECT SAJET.GET_IQC_LOT_TARGETTIME(:LOT_NO) TARGET_TIME FROM DUAL ";
                Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                DateTime dtTargetTime = (DateTime)dsTemp.Tables[0].Rows[0]["TARGET_TIME"];

                sSQL = " UPDATE SAJET.G_IQC_LOT "
                     + "    SET TARGET_TIME = :TARGET_TIME "
                     + " WHERE LOT_NO=:LOT_NO ";
                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TARGET_TIME", dtTargetTime };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_rIQC.sLotNo };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                CheckLotUrgent(sUrgendType);
            }
            catch (Exception E)
            {
                SajetCommon.Show_Message(E.Message, 0);
            }
        }

        private void tsbuttonHistory_ButtonClick(object sender, EventArgs e)
        {

        }

        private void tsButtonMPN_ButtonClick(object sender, EventArgs e)
        {

        }

        private void tsButtonTooling_ButtonClick(object sender, EventArgs e)
        {

        }

        private void tsButtonInspNote_ButtonClick(object sender, EventArgs e)
        {

        }

        private void tsbtnHistory_Click(object sender, EventArgs e)
        {
            fInspHistory fData = new fInspHistory();

            try
            {
                if (G_rIQC.bLotInput)
                {
                    fData.editPartNo.Text = G_rIQC.sPartNo;
                    fData.editVendor.Text = G_rIQC.sVendorCode;
                }
                fData.ShowDialog();
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void tsbtnMPN_Click(object sender, EventArgs e)
        {
            fMPN fData = new fMPN();

            try
            {
                fData.lblLotNo_show.Text = combLot.SelectedItem.ToString().Trim();
                fData.lblPartNo_show.Text = G_rIQC.sPartNo;
                fData.lblVN_show.Text = lablVendor.Text.Trim();

                fData.ShowDialog();
            }
            catch (NullReferenceException)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 1);
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void tsbtnTooling_Click(object sender, EventArgs e)
        {
            int iSampleSize = 0;
            if (dgvSampleType.Rows.Count > 0)
            {
                try
                {
                    iSampleSize = Convert.ToInt32(dgvSampleType.Rows[0].Cells["TSAMPLE_SIZE"].Value);
                }
                catch
                {

                }
            }
            ShowTooling(iSampleSize);
            tsbtnTooling.Text = SajetCommon.SetLanguage("Tooling") + " (" + GetTooling(G_rIQC.sLotNo).ToString() + ")";

            /*
            fTooling fData = new fTooling(g_sProgram, g_sUserID, iSampleSize);
            try
            {
                fData.lblLotNo_show.Text = combLot.SelectedItem.ToString().Trim();
                fData.lblPartNo_show.Text = G_rIQC.sPartNo;
               
                fData.ShowDialog();
            }
            catch (NullReferenceException)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 1);
            }
            finally
            {
                fData.Dispose();
            }
             */
 
        }
        private void ShowTooling(int iSampleSize)
        {
            Assembly assembly = null;
            object obj = null;
            Type type = null;
            string strApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(strApplicationPath + "\\IQCToolingdll.dll"))//在本地端發現DLL檔案則不另外組字串，否則從資料庫中搜尋程式在哪!
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("File Not Exist") + Environment.NewLine
                                       + SajetCommon.SetLanguage("File") + " : " + strApplicationPath + "\\IQCToolingdll.dll", 0);
                return;
            }
            try
            {
                //組裝資訊
                assembly = Assembly.LoadFrom(strApplicationPath + "\\IQCToolingdll.dll");
                type = assembly.GetType(("IQCToolingdll.fTooling"));
                obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, new object[] { g_sExeName, g_sProgram, g_sUserID, G_rIQC.sLotNo, G_rIQC.sPartNo, iSampleSize }, null, null);
                ((Form)obj).StartPosition = FormStartPosition.CenterScreen;
                ((Form)obj).WindowState = FormWindowState.Normal;
                ((Form)obj).ShowDialog();

            }
            catch (Exception ex)
            { SajetCommon.Show_Message("Load Function Error" + Environment.NewLine + ex.Message, 0); }
        }
        private void tsbtnNote_Click(object sender, EventArgs e)
        {
            fInspNotes fData = new fInspNotes();

            try
            {
                fData.g_sProgram = g_sProgram;
                fData.g_sUserID = g_sUserID;
                fData.g_sPartNo = G_rIQC.sPartNo;
                fData.ShowDialog();
                tsbtnNote.Text = SajetCommon.SetLanguage("Note") + " (" +GetPartNoteCount(G_rIQC.sPartID).ToString() + ")";
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
            if (!(sender is ToolStripSplitButton))
                return;
            string sTag = (sender as ToolStripSplitButton).Tag.ToString();
            string sItemName = "";
            switch (sTag)
            {
                case "1": sItemName = "Component Photo Path"; break;
                case "2": sItemName = "Abnormal Report"; break;
                case "3": sItemName = "Component IQC SOP"; break;
                default: break;
            }
            if (sItemName == "")
                return;
            string sMessage = "";
            string sFilePath = SajetCommon.GetSysBaseData("IQC", sItemName, ref sMessage);
            if (sMessage != "")
            {
                SajetCommon.Show_Message(sItemName, 0); 
                return;
            }
            if (sFilePath == "")
                return;
            try
            {
                System.Diagnostics.Process.Start(sFilePath);
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Can not open File Paht")+" : "+sFilePath, 0); 
            }
        }

        private void tsbtnPhoto_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripButton))
                return;
            string sTag = (sender as ToolStripButton).Tag.ToString();
            string sItemName = "";
            switch (sTag)
            {
                case "1": sItemName = "Component Photo Path"; break;
                case "2": sItemName = "Abnormal Report"; break;
                case "3": sItemName = "Component IQC SOP"; break;
                case "4": sItemName = "Quality Report"; break;
                default: break;
            }
            if (sItemName == "")
                return;
            string sMessage = "";
            string sFilePath = SajetCommon.GetSysBaseData("IQC", sItemName, ref sMessage);
            if (sMessage != "")
            {
                SajetCommon.Show_Message(sItemName, 0);
                return;
            }
            if (sFilePath == "")
                return;
            try
            {
                System.Diagnostics.Process.Start(sFilePath);
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Can not open File Path") + " : " + sFilePath, 0);
            }
        }

        private void tsbtnPDM_Click(object sender, EventArgs e)
        {
            /*
            if (!G_rIQC.bLotInput)
            {
                SajetCommon.Show_Message("Please Select Lot No", 0);
                combLotNo.SelectAll();
                return;
            }
            AdlinkWebService.ClassService adlink = new AdlinkWebService.ClassService();
            string sUrl = "", sMessage = "";
            bool bOK = adlink.ProcessPDMPart(lablPartNo.Text,ref sUrl,ref sMessage);
            if (!bOK)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage(sMessage)+" : "+lablPartNo.Text, 0);
            }
             */ 
        }

        private void combLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            combLot.Text = combLot.Text.Trim();
            int iIndex = combLot.Items.IndexOf(combLot.Text);
            G_rIQC.bLotInput = false;
            clearData();
            if (iIndex >= 0)
            {
                sMsgKeypass = true;
                combLot.SelectedIndex = iIndex;
                combLot_SelectedIndexChanged(sender, e);
            }
        }

        private void combLot_TextChanged(object sender, EventArgs e)
        {
            G_rIQC.bLotInput = false;
            clearData();
        }

        internal static void CreateMCIReport(string g_sFileName, object g_sSampleFile)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void toolStripDelete_Click(object sender, EventArgs e)
        {
            if (dgvSN.Rows.Count == 0 || dgvSN.SelectedRows.Count != 1)
                return;
            string g_sSNumber = dgvSN.CurrentRow.Cells["SERIAL_NUMBER"].Value.ToString();
            string g_sConfirmMsg = "Do you want to delete test record";            
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage(g_sConfirmMsg) + Environment.NewLine
                + SajetCommon.SetLanguage("Serial Number") + " : " + g_sSNumber, 2) != DialogResult.Yes)
            {
                return;
            }
            sSQL = "DELETE FROM SAJET.G_IQC_SN "
                 + "WHERE SERIAL_NUMBER = '" + g_sSNumber + "' "
                 + "AND LOT_NO = '" + combLot.Text.Trim() + "' ";
            ClientUtils.ExecuteSQL(sSQL);
            dgvSN.Rows.RemoveAt(dgvSN.CurrentRow.Index);
            //TestItemSNInfo(combLot.Text);
        }

        private void toolStripInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(combLot.Text))
            {
                SajetCommon.Show_Message("Please Choose LotNo", 0);
                return;
            }
            //string g_sSNumber = dgvSN.CurrentRow.Cells["SERIAL_NUMBER"].Value.ToString();
            fTestItemInput f = new fTestItemInput();
            try 
            {
                f.g_sLotNo = combLot.Text.Trim();
                f.g_sUserID = g_sUserID;
                //f.g_sfSerialNumber = g_sSNumber;
                f.g_sType = "INSERT";
                if (f.ShowDialog() == DialogResult.OK)
                TestItemSNInfo(combLot.Text.Trim());
            }
            finally 
            {
                f.Dispose();
            }
        }

        private void toolStripModify_Click(object sender, EventArgs e)
        {
            if (dgvSN.Rows.Count == 0 || dgvSN.SelectedRows.Count != 1)
                return;
            if (string.IsNullOrEmpty(combLot.Text))
            {
                SajetCommon.Show_Message("Please Choose LotNo", 0);
                return;
            }
            string g_sSNumber = dgvSN.CurrentRow.Cells["SERIAL_NUMBER"].Value.ToString();
            fTestItemInput f = new fTestItemInput();
            try
            {
                f.g_sLotNo = combLot.Text.Trim();
                f.g_sUserID = g_sUserID;
                f.g_sfSerialNumber = g_sSNumber;
                f.g_sType = "MODIFY";
                if (f.ShowDialog() == DialogResult.OK)
                TestItemSNInfo(combLot.Text.Trim());
            }
            finally
            {
                f.Dispose();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lablCriticalPartType.Visible = !lablCriticalPartType.Visible;            
        }

        private void btnException_Click(object sender, EventArgs e)
        {
           
        }

        private void rbtnExceptionYes_Click(object sender, EventArgs e)
        {
            if (!G_rIQC.bLotInput)
                return;
            if (!(sender is RadioButton))
                return;
            string sDelayHold = (sender as RadioButton).Tag.ToString();
            string sLotNo = G_rIQC.sLotNo;
            
            sSQL = "SELECT A.DELAY_HOLD "
                + "  FROM SAJET.G_IQC_LOT A "
                + " WHERE A.LOT_NO = :LOT_NO "
                + "   AND ROWNUM = 1 ";

            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
            if (ds.Tables[0].Rows.Count == 0)
                return;

            if (sDelayHold == ds.Tables[0].Rows[0]["DELAY_HOLD"].ToString())
                return;
            string sMessage = "Stop Exception Process";
            if (sDelayHold == "Y")
                sMessage = "Start Exception Process";
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Lot No") + " :" + sLotNo + Environment.NewLine
                                        + SajetCommon.SetLanguage(sMessage) + " ? ", 2) != DialogResult.Yes)
                return;
            string sMemo = "";
            fExceptionProcessMemo f = new fExceptionProcessMemo(sLotNo, sDelayHold);
            try
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    if (sDelayHold == "Y")
                        rbtnExceptionNo.Checked = true;
                    else
                        rbtnExceptionYes.Checked = true;
                    return;
                }
               sMemo = f.g_sMemo;
            }
            finally
            {
                f.Dispose();
            }


            sSQL = " UPDATE SAJET.G_IQC_LOT "
                   + "    SET DELAY_HOLD =:DELAY_HOLD ";
            if (sDelayHold == "Y")
                sSQL = sSQL + "  ,DELAY_HOLD_MEMO =:MEMO ";
            else
                sSQL = sSQL + "  ,DELAY_RELEASE_MEMO =:MEMO ";
            sSQL = sSQL + "  WHERE LOT_NO =:LOT_NO ";
            Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DELAY_HOLD", sDelayHold };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", sMemo };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void ShowComponentPic()
        {
            picPart1.Image = null;
            picPart2.Image = null;
            picPart3.Image = null;
            picPart4.Image = null;

            string sMessage = "";
            string sItemName = "Component Picture Path";
            string sFilePath = SajetCommon.GetSysBaseData("IQC", sItemName, ref sMessage);
            if (sMessage != "")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Define Parameter") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Parameter Name") + " : " + sItemName, 0);

                return;
            }
            sItemName = "Component Picture Ext";
            string sFileExt = SajetCommon.GetSysBaseData("IQC", sItemName, ref sMessage);
            if (sMessage != "")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Define Parameter") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Parameter Name") + " : " + sItemName, 0);

                return;
            }

            int iPicQty = 0;
            G_rIQC.sPartPic1 = "N/A";
            G_rIQC.sPartPic2 = "N/A";
            G_rIQC.sPartPic3 = "N/A";
            G_rIQC.sPartPic4 = "N/A";
            string sFile1 = "N/A";
            string sFile2 = "N/A";
            string sFile3 = "N/A";
            string sFile4 = "N/A";
            picPart1.SizeMode = PictureBoxSizeMode.Zoom;
            picPart2.SizeMode = PictureBoxSizeMode.Zoom;
            picPart3.SizeMode = PictureBoxSizeMode.Zoom;
            picPart4.SizeMode = PictureBoxSizeMode.Zoom;
            if (sFilePath == "")
                return;
            try
            {
                //取得PART_TYPE
                sSQL = @"SELECT MATERIAL_TYPE  FROM SAJET.SYS_PART
                        WHERE PART_NO = :PART_NO";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", G_rIQC.sPartNo };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                string sPartType = dsTemp.Tables[0].Rows[0]["MATERIAL_TYPE"].ToString();
                sFilePath = sFilePath + "\\" + sPartType; 
                //判斷路徑是否存在
                if (!Directory.Exists(sFilePath))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Component Picture Path Error") + " : " + sFilePath, 0);
                    return;
                }
                string sFileName;
                DirectoryInfo di = new DirectoryInfo(sFilePath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    //資料夾中檔名
                    sFileName = fi.Name;
                    //取得附檔名
                    string sExt = Path.GetExtension(sFileName);
                    if (sFileExt.ToUpper().IndexOf(sExt.ToUpper()) < 0)
                        continue;
                    //取得檔案名稱
                    string sFiles = Path.GetFileNameWithoutExtension(sFileName);
                    //取得料號
                    string sPart = sFiles.Substring(0, sFiles.Length - 2);
                    if (sPart != G_rIQC.sPartNo)
                        continue;
                    string sSEQ = sFiles.Substring(sFiles.Length - 1, 1);
                    switch (sSEQ)
                    {
                        case "1": sFile1 = sFilePath + "\\" + sFileName; break;
                        case "2": sFile2 = sFilePath + "\\" + sFileName; break;
                        case "3": sFile3 = sFilePath + "\\" + sFileName; break;
                        case "4": sFile4 = sFilePath + "\\" + sFileName; break;
                        default: break;
                    }

                }               
                if (File.Exists(sFile1))
                {
                    Image Img1 = Image.FromFile(sFile1);
                    picPart1.Image = Img1;
                    G_rIQC.sPartPic1 = sFile1;
                    iPicQty++;
                }

                if (File.Exists(sFile2))
                {
                    Image Img2 = Image.FromFile(sFile2);
                    picPart2.Image = Img2;
                    G_rIQC.sPartPic2 = sFile2;
                    iPicQty++;
                }

                if (File.Exists(sFile3))
                {
                    Image Img3 = Image.FromFile(sFile3);
                    picPart3.Image = Img3;
                    G_rIQC.sPartPic3 = sFile3;
                    iPicQty++;
                }

                if (File.Exists(sFile4))
                {
                    Image Img4 = Image.FromFile(sFile4);
                    picPart4.Image = Img4;
                    G_rIQC.sPartPic4 = sFile4;
                    iPicQty++;
                }


                tabControl1.TabPages["tpComponentPic"].Text = SajetCommon.SetLanguage("Component Picture") + "(" + iPicQty.ToString() + ")";
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Can not open File Path") + " : " + sFilePath, 0);
            }
        }

        private void ShowComponentSizePic()
        {
            picBoxSize1.Image = null;
            picBoxSize2.Image = null;
            string sMessage = "";
            string sItemName = "Component Size Picture Path";
            string sFilePath = SajetCommon.GetSysBaseData("IQC", sItemName, ref sMessage);
            if (sMessage != "")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Define Parameter") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Parameter Name") + " : " + sItemName, 0);

                return;
            }
            sItemName = "Component Size Picture Ext";
            string sFileExt = SajetCommon.GetSysBaseData("IQC", "Component Size Picture Ext", ref sMessage);
            if (sMessage != "")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Define Parameter") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Parameter Name") + " : " + sItemName, 0);

                return;
            }
            sItemName = "Component Size Picture FileName Prefix";
            string sFileNamePrefix = SajetCommon.GetSysBaseData("IQC", sItemName, ref sMessage);
            string[] PrefixList = sFileNamePrefix.Split(';');
            try
            {
                sFileNamePrefix = PrefixList[0] + G_rIQC.sPartNo + PrefixList[1];
            }
            catch
            {
                sFileNamePrefix = G_rIQC.sPartNo;
            }

            int iPicQty = 0;
            string sFile1 = "N/A";
            string sFile2 = "N/A";
            G_rIQC.sPartPic5 = "N/A";
            G_rIQC.sPartPic6 = "N/A";
            picBoxSize1.Image = null;
            picBoxSize2.Image = null;
            picBoxSize1.SizeMode = PictureBoxSizeMode.Zoom;
            picBoxSize2.SizeMode = PictureBoxSizeMode.Zoom;

            if (sFilePath == "")
                return;

            try
            {
                //取得PART_TYPE
                sSQL = @"SELECT MATERIAL_TYPE  FROM SAJET.SYS_PART
                        WHERE PART_NO = :PART_NO";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", G_rIQC.sPartNo };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                string sPartType = dsTemp.Tables[0].Rows[0]["MATERIAL_TYPE"].ToString();
                sFilePath = sFilePath + "\\" + sPartType;
                //判斷路徑是否存在
                if (!Directory.Exists(sFilePath))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Component Size Picture Path Error") + " : " + sFilePath, 0);
                    return;
                }
                string sFileName;
                DirectoryInfo di = new DirectoryInfo(sFilePath);

                DirectoryInfo dirinfo = new DirectoryInfo(sFilePath);
                FileInfo[] allFiles = dirinfo.GetFiles(sFileNamePrefix + "*.*");
                Array.Sort(allFiles, new FIleLastTimeComparer());
                for (int i = 0; i <= allFiles.Length - 1; i++)
                {
                    sFileName = allFiles[i].ToString();

                    //取得附檔名
                    string sExt = Path.GetExtension(sFileName);
                    if (sFileExt.ToUpper().IndexOf(sExt.ToUpper()) < 0)
                        continue;

                    string sFiles = Path.GetFileNameWithoutExtension(sFileName);
                    string sPart = sFiles.Substring(0, sFiles.Length - 2);
                    if (sPart != sFileNamePrefix)
                        continue;
                    if (sFile1=="N/A")
                        sFile1 = sFilePath + "\\" + sFileName;
                    else if (sFile2 == "N/A")
                        sFile2 = sFilePath + "\\" + sFileName;
                   
                    /*
                    string sSEQ = sFiles.Substring(sFiles.Length - 1, 1);
                    switch (sSEQ)
                    {
                        case "1": sFile1 = sFilePath + "\\" + sFileName; break;
                        case "2": sFile2 = sFilePath + "\\" + sFileName; break;
                        default: break;
                    }
                     */ 
                }


                if (File.Exists(sFile1))
                {
                    Image Img1 = Image.FromFile(sFile1);
                    G_rIQC.sPartPic5 = sFile1;
                    picBoxSize1.Image = Img1;
                    iPicQty++;
                }

                if (File.Exists(sFile2))
                {
                    Image Img2 = Image.FromFile(sFile2);
                    G_rIQC.sPartPic6 = sFile2;
                    picBoxSize2.Image = Img2;
                    iPicQty++;
                }
                tabPageSize.Text = SajetCommon.SetLanguage("Component Size Picture") + "(" + iPicQty.ToString() + ")";
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Can not open File Path") + " : " + sFilePath + Environment.NewLine
                    + ex.Message, 0);
            }
        }

        private void picPart1_Click(object sender, EventArgs e)
        {
            try
            {
                string sTag = (sender as PictureBox).Tag.ToString();
                string sFileName = "";
                if (sTag == "1")
                {
                    if (G_rIQC.sPartPic1 == "N/A")
                        return;
                    sFileName = G_rIQC.sPartPic1;
                }
                if (sTag == "2")
                {
                    if (G_rIQC.sPartPic2 == "N/A")
                        return;
                    sFileName = G_rIQC.sPartPic2;
                }
                if (sTag == "3")
                {
                    if (G_rIQC.sPartPic3 == "N/A")
                        return;
                    sFileName = G_rIQC.sPartPic3;
                }
                if (sTag == "4")
                {
                    if (G_rIQC.sPartPic4 == "N/A")
                        return;
                    sFileName = G_rIQC.sPartPic4;
                }
                if (sTag == "5")
                {
                    if (G_rIQC.sPartPic5 == "N/A")
                        return;
                    sFileName = G_rIQC.sPartPic5;
                }
                if (sTag == "6")
                {
                    if (G_rIQC.sPartPic6 == "N/A")
                        return;
                    sFileName = G_rIQC.sPartPic6;
                }
                //建立新的系统进程  
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //设置文件名，此处为图片的真实路径+文件名  
                process.StartInfo.FileName = sFileName;
                //此为关键部分。设置进程运行参数，此时为最大化窗口显示图片。  
                process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll,ImageView_Fullscreen";
                //此项为是否使用Shell执行程序，因系统默认为true，此项也可不设，但若设置必须为true  
                process.StartInfo.UseShellExecute = true;
                //此处可以更改进程所打开窗体的显示样式，可以不设  
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.Close();
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Can not open File"), 0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
             
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", "55N6-2014030153"};
            Params[1] = new object[] { ParameterDirection.Input, OracleType.Int32, "TTERMINALID", "20000552" };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID","20000001" };
            Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_EVENT_LOW_YIELD_TEST", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
           
                SajetCommon.Show_Message(sResult, 0);

        }
        private int GetTooling(string sLotNo)
        {
            string sSQL="SELECT COUNT(*) QTY "
                       +"  FROM SAJET.G_IQC_TOOLING "
                       +" WHERE LOT_NO=:LOT_NO ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["QTY"].ToString());
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private int GetPartNoteCount(string sPartID)
        {
            string sSQL = "SELECT COUNT(*) QTY FROM sajet.g_iqc_notes "

                       + " WHERE PART_ID =:PART_ID "
                       + "   AND ENABLED='Y' ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["QTY"].ToString());
        }

        private void btnPicture5_Click(object sender, EventArgs e)
        {
            string sTag = (sender as Button).Tag.ToString();
            string sSourceFileName = "N/A";
            switch (sTag)
            {
                case "1": sSourceFileName = G_rIQC.sPartPic1; break;
                case "2": sSourceFileName = G_rIQC.sPartPic2; break;
                case "3": sSourceFileName = G_rIQC.sPartPic3; break;
                case "4": sSourceFileName = G_rIQC.sPartPic4; break;
                case "5": sSourceFileName = G_rIQC.sPartPic5; break;
                case "6": sSourceFileName = G_rIQC.sPartPic6; break;
                default: break;
            }
            if (!string.IsNullOrEmpty(sSourceFileName) && sSourceFileName != "N/A")
            {
                saveFileDialog1.FileName = sSourceFileName;
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                    return;

                string sFile = saveFileDialog1.FileName;
                if (File.Exists(sFile))
                {
                    SajetCommon.Show_Message("File Exist", 0);
                    return;
                }

                File.Copy(sSourceFileName, sFile);


                SajetCommon.Show_Message(SajetCommon.SetLanguage("File Save OK") + Environment.NewLine
                                        + SajetCommon.SetLanguage("File Name") + " : " + sFile, 3);
            }
        }
    }
    //文件夹中按时间排序最新的文件读取
    public class DirectoryLastTimeComparer : IComparer<DirectoryInfo>
    {
        #region IComparer<DirectoryInfo> 成员

        public int Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return x.LastWriteTime.CompareTo(y.LastWriteTime);
            //依名称排序    
            //return x.FullName.CompareTo(y.FullName);//递增
            //return y.FullName.CompareTo(x.FullName);//递减

            //依修改日期排序    
            //return x.LastWriteTime.CompareTo(y.LastWriteTime);//递增
            //return y.LastWriteTime.CompareTo(x.LastWriteTime);//递减
        }

        #endregion
    }

    //文件夹中按时间排序最新的文件读取
    public class FIleLastTimeComparer : IComparer<FileInfo>
    {
        #region IComparer<FileInfo> 成员

        public int Compare(FileInfo x, FileInfo y)
        {
            //return x.LastWriteTime.CompareTo(y.LastWriteTime);
            return x.Name.CompareTo(y.Name);
        }

        #endregion
    }
}