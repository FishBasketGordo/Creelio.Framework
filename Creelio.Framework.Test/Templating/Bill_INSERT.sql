--------------------------------------------------------------------------------------------------------------------------
-- Auto-generated by T4 template                                                                                        --
--                                                                                                                      --
-- Template Path : C:\Users\Gordon\Development\Creelio.Framework\Creelio.Framework.Test\Templating\CrudGeneratorTest.tt --
-- Generated On  : 7/3/2012 1:35:34 AM                                                                                  --
--                                                                                                                      --
-- WARNING: Do not modify this file directly. Your changes will be overwritten when the file is regenerated.            --
--------------------------------------------------------------------------------------------------------------------------
USE [Checkbook]
GO

IF NOT EXISTS
(
    SELECT TOP 1 1 FROM INFORMATION_SCHEMA.ROUTINES r
     WHERE r.ROUTINE_TYPE   = 'PROCEDURE'
       AND r.ROUTINE_SCHEMA = 'dbo'
       AND r.ROUTIME_NAME   = 'Bill_INSERT'
)
BEGIN
    EXEC sp_executesql N'
    CREATE PROCEDURE [dbo].[Bill_INSERT]
    AS
    BEGIN
        SELECT 1
    END
    '
END
GO

ALTER PROCEDURE [dbo].[Bill_INSERT]
AS
BEGIN
    -- TODO
    SELECT 1
END
GO
