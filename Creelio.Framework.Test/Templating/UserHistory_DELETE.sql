--------------------------------------------------------------------------------------------------------------------------
-- Auto-generated by T4 template                                                                                        --
--                                                                                                                      --
-- Template Path : C:\Users\Gordon\Development\Creelio.Framework\Creelio.Framework.Test\Templating\CrudGeneratorTest.tt --
-- Generated On  : 5/21/2012 8:32:04 PM                                                                                 --
--                                                                                                                      --
-- WARNING: Do not modify this file directly. Your changes will be overwritten when the file is regenerated.            --
--------------------------------------------------------------------------------------------------------------------------
USE [Checkbook]
GO

CREATE PROCEDURE UserHistory_DELETE
(
     @UserHistoryID INT 
)
AS
BEGIN
    DELETE FROM [dbo].[UserHistory]
    WHERE ([UserHistoryID] = @UserHistoryID)
END
