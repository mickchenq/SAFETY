CREATE TABLE [dbo].[StockAdjustment] (
    [OrderId]       INT           IDENTITY (1, 1) NOT NULL,
    [OrderNo]       NVARCHAR (20) NOT NULL,
    [DcId]          INT           NOT NULL,
    [HouseId]       INT           NOT NULL,
    [RoomId]        INT           NOT NULL,
    [CustomerId]    INT           NOT NULL,
    [ProductId]     INT           NOT NULL,
    [OrderDate]     DATE          NULL,
    [StockAdjustId] INT           NOT NULL,
    [AdjustStatus]  TINYINT       DEFAULT ('1') NOT NULL,
    [CreateId]      INT           NOT NULL,
    [CreateDate]    DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifyId]      INT           NULL,
    [ModifyDate]    DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'盤點通知單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockAdjustment', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'盤點狀態 | 1:盤點通知  2.盤點中 3:盤點完成', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockAdjustment', @level2type = N'COLUMN', @level2name = N'AdjustStatus';

