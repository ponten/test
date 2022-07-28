﻿using RCIPQC.References;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OtSrv = RCIPQC.Services.OtherService;

namespace RCIPQC
{
    public partial class fChangeMachine : Form
    {
        #region 參數

        /// <summary>
        /// 流程卡號。
        /// </summary>
        public string Runcard = string.Empty;

        /// <summary>
        /// 流程卡所在製程 ID。
        /// </summary>
        public string ProcessID = string.Empty;

        public string EmpID = string.Empty;

        public bool usingT4OrT6stove = false;

        /// <summary>
        /// 機台的清單。
        /// </summary>
        private List<MachineDownModel> MachineList = new List<MachineDownModel>();

        /// <summary>
        /// 使用者現在選到的資料列。
        /// </summary>
        private MachineDownModel CurrentRow = null;

        #endregion

        public fChangeMachine()
        {
            #region 屬性

            InitializeComponent();

            SajetClass.SajetCommon.SetLanguageControl(this);

            #endregion

            #region 事件

            this.Load += FChangeMachine_Load;

            BtnOK.Click += BtnOK_Click;

            DgvMachine.CellClick += DgvWipIn_CellClick;

            BtnAdd.Click += BtnAdd_Click;

            BtnRemove.Click += BtnRemove_Click;

            #endregion
        }

        #region 事件

        private void FChangeMachine_Load(object sender, EventArgs e)
        {
            dATECODEDataGridViewTextBoxColumn.Visible = usingT4OrT6stove;

            sTOVESEQDataGridViewTextBoxColumn.Visible = usingT4OrT6stove;

            var d = OtSrv.GetMachineList(Runcard);

            MachineList = OtSrv.GetModels(d);

            MachineList.ForEach(x => x.Select = false);

            DgvMachine.DataSource = MachineList;

            // 重繪 DataGridView
            RearrangeDataGridView(ref DgvMachine);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var f = new fAddMachine
            {
                Runcard = Runcard,
                ProcessID = ProcessID,
                EmpID = EmpID,
            })
            {
                f.ShowDialog();

                var d = OtSrv.GetMachineList(Runcard);

                MachineList = OtSrv.GetModels(d);

                MachineList.ForEach(x => x.Select = false);

                DgvMachine.DataSource = MachineList;

                RearrangeDataGridView(ref DgvMachine);

                CurrentRow = null;
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (CurrentRow != null)
            {
                using (var f = new fRemoveMachine
                {
                    Runcard = Runcard,
                    Machine = CurrentRow,
                    EmpID = EmpID,
                    usingT4OrT6stove = usingT4OrT6stove,
                })
                {
                    f.ShowDialog();

                    var d = OtSrv.GetMachineList(Runcard);

                    MachineList = OtSrv.GetModels(d);

                    MachineList.ForEach(x => x.Select = false);

                    DgvMachine.DataSource = MachineList;

                    RearrangeDataGridView(ref DgvMachine);

                    CurrentRow = null;
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void DgvWipIn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                CurrentRow = MachineList
                    .First(x => x.MACHINE_CODE.ToString() == DgvMachine.CurrentRow.Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString());

                foreach (DataGridViewRow row in DgvMachine.Rows)
                {
                    row.Cells[nameof(selectDataGridViewCheckBoxColumn)].Value =
                        row.Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString() == CurrentRow.MACHINE_CODE;
                }
            }
            else
            {
                CurrentRow = null;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 重新佈置 DataGridView 各項屬性。
        /// </summary>
        /// <param name="x"></param>
        private void RearrangeDataGridView(ref DataGridView x)
        {
            x.Update();

            x.Refresh();

            x.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        #endregion
    }
}
