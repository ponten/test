--------------------------------------------------------
--  已建立檔案 - 星期三-五月-12-2021   
--------------------------------------------------------
REM INSERTING into SAJET.S_SYS_PRINT_DATA
SET DEFINE OFF;
Insert into SAJET.S_SYS_PRINT_DATA (DATA_TYPE,DATA_ORDER,DATA_SQL,INPUT_PARAM,INPUT_FIELD,OUTPUT_PARAM,DATA_SQL2,INPUT_PARAM2,INPUT_FIELD2,OUTPUT_PARAM2) values ('BC_RC No','1',' SELECT RC_NO, WORK_ORDER, PART_ID, ROUTE_ID, CURRENT_QTY FROM SAJET.G_RC_STATUS WHERE RC_NO = @01 AND ROWNUM = 1 ','@01','RC_NO',null,null,null,null,null);
Insert into SAJET.S_SYS_PRINT_DATA (DATA_TYPE,DATA_ORDER,DATA_SQL,INPUT_PARAM,INPUT_FIELD,OUTPUT_PARAM,DATA_SQL2,INPUT_PARAM2,INPUT_FIELD2,OUTPUT_PARAM2) values ('BC_RC No','2',' SELECT A.* FROM SAJET.SYS_PART A WHERE A.PART_ID = @01 ','@01','PART_ID',null,null,null,null,null);
Insert into SAJET.S_SYS_PRINT_DATA (DATA_TYPE,DATA_ORDER,DATA_SQL,INPUT_PARAM,INPUT_FIELD,OUTPUT_PARAM,DATA_SQL2,INPUT_PARAM2,INPUT_FIELD2,OUTPUT_PARAM2) values ('BC_RC No','3',' SELECT A.PROCESS_NAME, A.PROCESS_CODE, C.MACHINE_TYPE_DESC, D.DEPT_NAME FROM TABLE ( SAJET.SJ_GET_RC_ROUTE_PROCESS(@01) ) A LEFT JOIN SAJET.SYS_PROCESS_OPTION B ON A.PROCESS_ID = B.PROCESS_ID LEFT JOIN SAJET.SYS_MACHINE_TYPE C ON B.OPTION6 = TO_CHAR(C.MACHINE_TYPE_ID) LEFT JOIN SAJET.SYS_DEPT D ON B.OPTION7 = TO_CHAR(D.DEPT_ID) ORDER BY A.SORT_SEQ ','@01','ROUTE_ID','PROCESS_;CODE_;EQUIP_;DEPT_','SELECT ITEM_NAME || '' : '' || VALUE_DEFAULT || '' '' || REPLACE(C.UNIT_NO, ''---'', '' '') ITEM_NAME FROM SAJET.SYS_RC_PROCESS_PARAM_PART A, SAJET.SYS_PROCESS B, SAJET.SYS_UNIT C WHERE A.PART_ID = @PART_ID AND B.PROCESS_NAME = @01 AND A.PROCESS_ID = B.PROCESS_ID AND ITEM_TYPE = ''0'' AND A.UNIT_ID = C.UNIT_ID (+) ORDER BY ITEM_SEQ, ITEM_NAME','@01;@PART_ID','PROCESS_NAME;PART_ID','CONDITION');
Insert into SAJET.S_SYS_PRINT_DATA (DATA_TYPE,DATA_ORDER,DATA_SQL,INPUT_PARAM,INPUT_FIELD,OUTPUT_PARAM,DATA_SQL2,INPUT_PARAM2,INPUT_FIELD2,OUTPUT_PARAM2) values ('BC_RC No','4',' SELECT A.DATENUMBER || ''-'' || A.NUMBER1 PO, A.TARGET_QTY, CASE TRIM(A.OPERATION_ID) WHEN ''10A'' THEN A.PRODUCENO1 WHEN ''10B'' THEN A.PRODUCENO2 WHEN ''10C'' THEN A.PRODUCE_NUMBER ELSE A.PRODUCE_NUMBER END PRODUCE_NUMBER, A.OLD_NUMBER, A.BLUEPRINT, CASE A.MADE_CATEGORY WHEN ''1'' THEN ''試製'' WHEN ''2'' THEN ''樣試'' WHEN ''3'' THEN ''量試'' WHEN ''4'' THEN ''量產'' WHEN ''5'' THEN ''重工'' ELSE ''--'' END MADE_CATEGORY, TO_CHAR(SYSDATE, ''YYYY/MM/DD'') PRINTDATE, A.REMARK, ''['' || B.EMP_NO || '']'' || B.EMP_NAME SIGNATURE, A.WO_OPTION2 FROM SAJET.G_WO_BASE A, SAJET.SYS_EMP B WHERE A.UPDATE_USERID = B.EMP_ID AND WORK_ORDER = @01','@01','WORK_ORDER',null,null,null,null,null);
