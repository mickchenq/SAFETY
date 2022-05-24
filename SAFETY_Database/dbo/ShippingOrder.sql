CREATE TABLE [dbo].[ShippingOrder] (
    [OrderId]               INT            IDENTITY (1, 1) NOT NULL,
    [OrderNo]               NVARCHAR (20)  NOT NULL,
    [DcId]                  INT            NOT NULL,
    [CustomerId]            INT            NOT NULL,
    [EstimatedShippingDate] DATE           NOT NULL,
    [EstimatedReceiveDate]  DATE           NOT NULL,
    [OperationType]         TINYINT        NOT NULL,
    [ArriveType]            TINYINT        NOT NULL,
    [ContactName]           NVARCHAR (20)  NOT NULL,
    [ContactPhone]          NVARCHAR (20)  NOT NULL,
    [ContactAddress]        NVARCHAR (100) NOT NULL,
    [ZipCode]               NVARCHAR (10)  NULL,
    [ShippingStatus]        TINYINT        NOT NULL,
    [Remarks]               NVARCHAR (200) DEFAULT ('') NOT NULL,
    [CreateId]              INT            NOT NULL,
    [CreateDate]            DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]              INT            NULL,
    [ModifyDate]            DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨通知單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingOrder';

