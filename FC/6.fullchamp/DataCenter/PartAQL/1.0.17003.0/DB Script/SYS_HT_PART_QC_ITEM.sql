
  CREATE TABLE "SAJET"."SYS_HT_PART_QC_ITEM" 
   (	"PART_ID" NUMBER, 
	"VERSION" VARCHAR2(30 BYTE), 
	"ROUTE_ID" NUMBER, 
	"NODE_ID" NUMBER, 
	"PROCESS_ID" NUMBER, 
	"ITEM_ID" NUMBER, 
	"ITEM_NAME" VARCHAR2(500 BYTE), 
	"ITEM_TYPE" VARCHAR2(1 BYTE), 
	"ITEM_PHASE" VARCHAR2(1 BYTE), 
	"ITEM_SEQ" NUMBER, 
	"INPUT_TYPE" VARCHAR2(1 BYTE), 
	"VALUE_TYPE" VARCHAR2(1 BYTE), 
	"VALUE_DEFAULT" VARCHAR2(20 BYTE), 
	"VALUE_LIST" VARCHAR2(3000 BYTE), 
	"CONVERT_TYPE" VARCHAR2(1 BYTE), 
	"COLUMN_ITEM" VARCHAR2(20 BYTE), 
	"ROW_ITEM" VARCHAR2(20 BYTE), 
	"NECESSARY" VARCHAR2(1 BYTE), 
	"PRINT" VARCHAR2(1 BYTE), 
	"UNIT_ID" NUMBER, 
	"UPDATE_USERID" NUMBER, 
	"UPDATE_TIME" DATE, 
	"ENABLED" VARCHAR2(1 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSBS" ;
 
