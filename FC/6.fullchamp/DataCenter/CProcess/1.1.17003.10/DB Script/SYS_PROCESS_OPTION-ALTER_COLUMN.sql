ALTER TABLE SAJET.SYS_PROCESS_OPTION MODIFY
    OPTION6 VARCHAR2(50) NULL;

ALTER TABLE SAJET.SYS_PROCESS_OPTION MODIFY
    OPTION6 DEFAULT '0';

COMMENT ON COLUMN "SAJET"."SYS_PROCESS_OPTION"."OPTION6" IS    '設備使用（機台類型）';

ALTER TABLE SAJET.SYS_PROCESS_OPTION MODIFY
    OPTION7 VARCHAR2(50) NULL;

ALTER TABLE SAJET.SYS_PROCESS_OPTION MODIFY
    OPTION7 DEFAULT '0';

COMMENT ON COLUMN "SAJET"."SYS_PROCESS_OPTION"."OPTION7" IS    '生產單位（部門）';

ALTER TABLE SAJET.SYS_HT_PROCESS_OPTION MODIFY
    OPTION6 VARCHAR2(50);

ALTER TABLE SAJET.SYS_HT_PROCESS_OPTION MODIFY
    OPTION7 VARCHAR2(50);

UPDATE SAJET.SYS_PROCESS_OPTION
SET
    OPTION6 = '0',
    OPTION7 = '0';