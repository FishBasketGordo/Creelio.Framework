--------------------------------------------------------------------------------------------------------------------------
-- Auto-generated by T4 template                                                                                        --
--                                                                                                                      --
-- Template Path : C:\Users\Gordon\Development\Creelio.Framework\Creelio.Framework.Test\Templating\CrudGeneratorTest.tt --
-- Generated On  : 7/4/2012 11:26:57 AM                                                                                 --
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
       AND r.ROUTIME_NAME   = 'AccountTypeHistory_SELECT'
)
BEGIN
    EXEC sp_executesql N'
    CREATE PROCEDURE [dbo].[AccountTypeHistory_SELECT]
    AS
    BEGIN
        SELECT 1
    END
    '
END
GO
ALTER PROCEDURE [dbo].[AccountTypeHistory_SELECT]
(
     @AccountTypeHistoryID INT = NULL
    ,@HistoryUserName VARCHAR(30) = NULL
    ,@HistoryTimestamp DATETIME = NULL
    ,@HistoryAction VARCHAR = NULL
    ,@AccountTypeID INT = NULL
    ,@Type VARCHAR(20) = NULL
    ,@Description VARCHAR(50) = NULL
)
AS
BEGIN
    -- TODO
    SELECT 1
END
GO
