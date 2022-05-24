CREATE TABLE [dbo].[StockAdjustmentDetail] (
    [OrderDetailId]    INT           IDENTITY (1, 1) NOT NULL,
    [OrderId]          INT           NOT NULL,
    [ProductStatus]    TINYINT       NOT NULL,
    [DcId]             INT           NOT NULL,
    [CustomerId]       INT           NOT NULL,
    [ProductId]        INT           NOT NULL,
    [ProductLotNo]     NVARCHAR (20) NOT NULL,
    [Unit]             INT           DEFAULT ((0)) NOT NULL,
    [ExpirationDate]   DATE          NULL,
    [RoomId]           INT           NOT NULL,
    [LocationId]       INT           NOT NULL,
    [LocationQuantity] INT           NOT NULL,
    [AdjustQuantity]   INT           NULL,
    PRIMARY KEY CLUSTERED ([OrderDetailId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'盤點通知單明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockAdjustmentDetail';

