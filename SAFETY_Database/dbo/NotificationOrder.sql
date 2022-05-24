CREATE TABLE [dbo].[NotificationOrder] (
    [OrderId]              INT            IDENTITY (1, 1) NOT NULL,
    [OrderNo]              NVARCHAR (20)  NOT NULL,
    [DcId]                 INT            NOT NULL,
    [CustomerId]           INT            NOT NULL,
    [CustomerOrderNo]      NVARCHAR (20)  NOT NULL,
    [EstimatedReceiveDate] DATE           NOT NULL,
    [ActualReceiveDate]    DATE           NOT NULL,
    [ReceiveType]          TINYINT        NOT NULL,
    [OperateType]          TINYINT        NOT NULL,
    [IsUrgentOrder]        TINYINT        NOT NULL,
    [ContactName]          NVARCHAR (20)  NOT NULL,
    [ContactPhone]         NVARCHAR (20)  NOT NULL,
    [ContactAddress]       NVARCHAR (100) NOT NULL,
    [NotificationStatus]   TINYINT        NOT NULL,
    [Remarks]              NVARCHAR (200) DEFAULT ('') NOT NULL,
    [AcceptDate]           DATE           NULL,
    [AcceptId]             INT            NULL,
    [AcceptRemarks]        NVARCHAR (200) DEFAULT ('') NOT NULL,
    [CreateId]             INT            NOT NULL,
    [CreateDate]           DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]             INT            NULL,
    [ModifyDate]           DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進貨通知單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進貨驗收日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationOrder', @level2type = N'COLUMN', @level2name = N'AcceptDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'驗收人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationOrder', @level2type = N'COLUMN', @level2name = N'AcceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'驗收備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationOrder', @level2type = N'COLUMN', @level2name = N'AcceptRemarks';

