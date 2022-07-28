
  CREATE TABLE "SAJET"."G_RC_TRAVEL_MACHINE_DOWN" 
   (	"RC_NO" VARCHAR2(50 BYTE), 
	"NODE_ID" NUMBER, 
	"TRAVEL_ID" NUMBER, 
	"MACHINE_ID" VARCHAR2(20 BYTE), 
	"START_TIME" DATE, 
	"END_TIME" DATE, 
	"REASON_ID" NUMBER, 
	"LOAD_QTY" NUMBER, 
	"WORK_TIME_MINUTE" NUMBER, 
	"WORK_TIME_SECOND" FLOAT(126), 
	"REMARK" VARCHAR2(500 BYTE), 
	"UPDATE_USERID" NUMBER, 
	"UPDATE_TIME" VARCHAR2(20 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSBS" ;
 

   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."RC_NO" IS '流程卡號';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."NODE_ID" IS '途程節點ID
';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."TRAVEL_ID" IS '歷程編號';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."MACHINE_ID" IS '機台';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."START_TIME" IS '運轉開始時間';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."END_TIME" IS '運轉結束時間';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."REASON_ID" IS '停機代碼(ID)';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."LOAD_QTY" IS '負載量';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."WORK_TIME_MINUTE" IS '花費時間（分鐘）';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."WORK_TIME_SECOND" IS '花費時間（秒鐘）';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."REMARK" IS '備註';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."UPDATE_USERID" IS '資料異動人員';
 
   COMMENT ON COLUMN "SAJET"."G_RC_TRAVEL_MACHINE_DOWN"."UPDATE_TIME" IS '最後更新時間';
 
