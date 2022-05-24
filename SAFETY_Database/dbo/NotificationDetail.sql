CREATE TABLE [dbo].[NotificationDetail] (
    [DetailId]       INT            IDENTITY (1, 1) NOT NULL,
    [OrderId]        INT            NOT NULL,
    [ProductId]      INT            NOT NULL,
    [ProductLotNo]   NVARCHAR (20)  NOT NULL,
    [Unit]           INT            DEFAULT ((1)) NOT NULL,
    [Quantity]       INT            NOT NULL,
    [MakeDate]       DATE           NOT NULL,
    [ExpirationDate] DATE           NOT NULL,
    [Remarks]        NVARCHAR (200) DEFAULT ('') NOT NULL,
    [ProductStatus]  TINYINT        NOT NULL,
    PRIMARY KEY CLUSTERED ([DetailId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進貨通知單明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationDetail';

