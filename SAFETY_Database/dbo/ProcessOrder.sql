CREATE TABLE [dbo].[ProcessOrder] (
    [OrderId]                 INT            IDENTITY (1, 1) NOT NULL,
    [OrderNo]                 NVARCHAR (20)  NOT NULL,
    [CustomerId]              INT            NOT NULL,
    [EstimatedProcessingDate] DATE           NOT NULL,
    [OperationType]           TINYINT        NOT NULL,
    [ProcessingStatus]        TINYINT        NOT NULL,
    [Remarks]                 NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]                  CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]                INT            NOT NULL,
    [CreateDate]              DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]                INT            NULL,
    [ModifyDate]              DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工通知單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProcessOrder';

