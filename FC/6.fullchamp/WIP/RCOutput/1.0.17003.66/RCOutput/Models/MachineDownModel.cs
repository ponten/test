﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OtSrv = RCOutput.Services.OtherService;

namespace RCOutput.Models
{
    /// <summary>
    /// 記錄機台使用資訊的模型
    /// </summary>
    public class MachineDownModel
    {
        public MachineDownModel() { }

        public MachineDownModel(MachineDownModel e)
        {
            Select = e.Select;

            MACHINE_ID = e.MACHINE_ID;

            MACHINE_CODE = e.MACHINE_CODE;

            MACHINE_DESC = e.MACHINE_DESC;

            STATUS_NAME = e.STATUS_NAME;

            TYPE_ID = e.TYPE_ID;

            REASON_ID = e.REASON_ID;

            START_TIME = e.START_TIME;

            END_TIME = e.END_TIME;

            LOAD_QTY = e.LOAD_QTY;

            DATE_CODE = e.DATE_CODE;

            STOVE_SEQ = e.STOVE_SEQ;

            REMARK = e.REMARK;
        }

        public bool Select { get; set; } = false;

        public int MACHINE_ID { get; set; } = 0;

        public string MACHINE_CODE { get; set; } = string.Empty;

        public string MACHINE_DESC { get; set; } = string.Empty;

        public string STATUS_NAME { get; set; } = string.Empty;

        public int TYPE_ID { get; set; } = 0;

        public int REASON_ID { get; set; } = 0;

        public DateTime START_TIME { get; set; } = OtSrv.GetDBDateTimeNow();

        public DateTime END_TIME { get; set; } = OtSrv.GetDBDateTimeNow();

        public int? LOAD_QTY { get; set; } = 0;

        public string DATE_CODE { get; set; } = string.Empty;

        public string STOVE_SEQ { get; set; } = string.Empty;

        public string REMARK { get; set; } = string.Empty;
    }
}
