
  CREATE TABLE "SAJET"."SYS_PART_QC_PLAN" 
   (	"SPQP_ID" NUMBER NOT NULL ENABLE, 
	"PART_ID" NUMBER NOT NULL ENABLE, 
	"VERSION" VARCHAR2(30 BYTE) DEFAULT 'N/A' NOT NULL ENABLE, 
	"ROUTE_ID" NUMBER NOT NULL ENABLE, 
	"NODE_ID" NUMBER NOT NULL ENABLE, 
	"SAMPLING_ID" NUMBER NOT NULL ENABLE, 
	"UPDATE_USERID" NUMBER NOT NULL ENABLE, 
	"UPDATE_TIME" DATE NOT NULL ENABLE, 
	"ENABLED" VARCHAR2(1 BYTE) DEFAULT 'Y' NOT NULL ENABLE, 
	 CONSTRAINT "SYS_PART_QC_PLAN_PK" PRIMARY KEY ("SPQP_ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSBS"  ENABLE
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSBS" ;
 

   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."SPQP_ID" IS '物料生產途程製程與抽驗計畫關聯表的 ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."PART_ID" IS 'SYS_PART.PART_ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."VERSION" IS 'SYS_PART.VERSION';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."ROUTE_ID" IS 'SYS_RC_ROUTE.ROUTE_ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."NODE_ID" IS 'SYS_RC_ROUTE_DETAIL.NODE_ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."SAMPLING_ID" IS 'SYS_QC_SAMPLING_PLAN.SAMPLING_ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."UPDATE_USERID" IS '更新資料人員的 ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."UPDATE_TIME" IS '更新時間';
 
   COMMENT ON COLUMN "SAJET"."SYS_PART_QC_PLAN"."ENABLED" IS '資料啟用狀態';
 

  CREATE INDEX "SAJET"."SPQP_IDX_1" ON "SAJET"."SYS_PART_QC_PLAN" ("PART_ID", "VERSION", "ROUTE_ID", "NODE_ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSBS" ;
 
