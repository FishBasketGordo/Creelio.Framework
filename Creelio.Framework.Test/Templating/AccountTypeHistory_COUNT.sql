--------------------------------------------------------------------------------------------------------------------------
-- Auto-generated by T4 template                                                                                        --
--                                                                                                                      --
-- Template Path : C:\Users\Gordon\Development\Creelio.Framework\Creelio.Framework.Test\Templating\CrudGeneratorTest.tt --
-- Generated On  : 6/9/2012 9:42:17 PM                                                                                  --
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
       AND r.ROUTIME_NAME   = 'AccountTypeHistory_COUNT'
)
BEGIN
    EXEC sp_executesql N'
    CREATE PROCEDURE [dbo].[AccountTypeHistory_COUNT]
    AS
    BEGIN
        SELECT 1
    END
    '
END
GO

ALTER PROCEDURE [dbo].[AccountTypeHistory_COUNT]
AS
BEGIN
    SELECT COUNT(*) FROM AccountTypeHistory
END
GO
