delete sajet.sys_module_param p where p.module_name='PACKING' and p.function_name='PACK CARTON';

Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, 
    UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', 'PACK BASE', 'PARTNO', 10000001, 
    TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, 
    UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', 'PACK ACTION', 'RC->CARTON', 10000001, 
    TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, 
    UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', 'PRINT LABEL', 'Y', 10000001, 
    TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, 
    UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', 'WEIGHT CARTON', 'N', 10000001, 
    TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, 
    UPDATE_USERID, UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', '4', 'CAPACITY', '300', 
    10000001, TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, 
    UPDATE_USERID, UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', '6', 'CAPACITY', '300', 
    10000001, TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, 
    UPDATE_USERID, UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', '2', 'CAPACITY', '500', 
    10000001, TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, 
    UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', 'PATH', 'D:\CartonList', 10000001, 
    TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
Insert into SAJET.SYS_MODULE_PARAM
   (MODULE_NAME, FUNCTION_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, 
    UPDATE_TIME, ENABLED)
 Values
   ('PACKING', 'PACK CARTON', 'PROCESS', '100037', 10000001, 
    TO_DATE('11/02/2016 16:45:34', 'MM/DD/YYYY HH24:MI:SS'), 'Y');
COMMIT;


