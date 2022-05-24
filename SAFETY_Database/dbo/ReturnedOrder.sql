CREATE TABLE [dbo].[ReturnedOrder] (
    [OrderId]              INT            IDENTITY (1, 1) NOT NULL,
    [OrderNo]              NVARCHAR (20)  NOT NULL,
    [DcId]                 INT            NOT NULL,
    [RelatedId]            INT            NOT NULL,
    [CustomerId]           INT            NOT NULL,
    [CustomerOrderNo]      NVARCHAR (20)  NOT NULL,
    [ReturnedDate]         DATE           NOT NULL,
    [EstimatedReceiveDate] DATE           NOT NULL,
    [OperateType]          TINYINT        NOT NULL,
    [ReturnType]           TINYINT        NOT NULL,
    [ContactName]          NVARCHAR (20)  NOT NULL,
    [ContactPhone]         NVARCHAR (20)  NOT NULL,
    [ContactAddress]       NVARCHAR (100) NOT NULL,
    [AcceptDate]           DATE           NULL,
    [AcceptId]             INT            NULL,
    [AcceptRemarks]        NVARCHAR (200) DEFAULT ('') NOT NULL,
    [ReturnStatus]         TINYINT        NOT NULL,
    [Remarks]              NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]               CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]             INT            NOT NULL,
    [CreateDate]           DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]             INT            NULL,
    [ModifyDate]           DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退貨通知單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnedOrder';

