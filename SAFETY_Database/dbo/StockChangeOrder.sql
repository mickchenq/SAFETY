CREATE TABLE [dbo].[StockChangeOrder] (
    [OrderId]          INT            IDENTITY (1, 1) NOT NULL,
    [OrderNo]          NVARCHAR (20)  NOT NULL,
    [ProductStatus]    TINYINT        NOT NULL,
    [DcId]             INT            NOT NULL,
    [CustomerId]       INT            NOT NULL,
    [ProductId]        INT            NOT NULL,
    [ProductLotNo]     NVARCHAR (20)  NOT NULL,
    [Unit]             INT            DEFAULT ((0)) NOT NULL,
    [ExpirationDate]   DATE           NULL,
    [RoomId]           INT            NOT NULL,
    [LocationId]       INT            NOT NULL,
    [LocationQuantity] INT            NOT NULL,
    [ChangeQuantity]   INT            NULL,
    [OrderDate]        DATE           NULL,
    [StockAdjustId]    INT            NOT NULL,
    [ChangeReason]     NVARCHAR (200) NOT NULL,
    [ChangeStatus]     TINYINT        DEFAULT ('1') NOT NULL,
    [CreateId]         INT            NOT NULL,
    [CreateDate]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]         INT            NULL,
    [ModifyDate]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存調整單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockChangeOrder', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整狀態 | 1:調整通知  2.調整中 3:調整完成', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockChangeOrder', @level2type = N'COLUMN', @level2name = N'ChangeStatus';

