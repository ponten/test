
  CREATE TABLE "SAJET"."SYS_HT_MACHINE_DOWN_TYPE" 
   (	"TYPE_ID" NUMBER, 
	"TYPE_CODE" VARCHAR2(20 BYTE), 
	"TYPE_DESC" VARCHAR2(100 BYTE), 
	"UPDATE_USERID" NUMBER, 
	"UPDATE_TIME" DATE, 
	"ENABLED" VARCHAR2(10 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSBS" ;
 

   COMMENT ON COLUMN "SAJET"."SYS_HT_MACHINE_DOWN_TYPE"."TYPE_ID" IS '停機異常類型ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_HT_MACHINE_DOWN_TYPE"."TYPE_CODE" IS '停機異常類型代碼';
 
   COMMENT ON COLUMN "SAJET"."SYS_HT_MACHINE_DOWN_TYPE"."TYPE_DESC" IS '停機異常類型描述';
 
   COMMENT ON COLUMN "SAJET"."SYS_HT_MACHINE_DOWN_TYPE"."UPDATE_USERID" IS '最後異動人員';
 
   COMMENT ON COLUMN "SAJET"."SYS_HT_MACHINE_DOWN_TYPE"."UPDATE_TIME" IS '最後更新時間';
 
   COMMENT ON COLUMN "SAJET"."SYS_HT_MACHINE_DOWN_TYPE"."ENABLED" IS '啟用狀態';
 
