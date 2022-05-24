CREATE TABLE [dbo].[StockInOrder] (
    [OrderId]     INT           IDENTITY (1, 1) NOT NULL,
    [OrderNo]     NVARCHAR (20) NOT NULL,
    [RelatedId]   INT           NOT NULL,
    [OrderDate]   DATE          NOT NULL,
    [StockUserId] INT           NOT NULL,
    [StockStatus] TINYINT       DEFAULT ('1') NOT NULL,
    [CreateId]    INT           NOT NULL,
    [CreateDate]  DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifyId]    INT           NULL,
    [ModifyDate]  DATETIME      NULL,
    [OrderType]   TINYINT       DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'入庫單id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockInOrder', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態 | 1:待入庫  2.入庫中 3:已入庫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockInOrder', @level2type = N'COLUMN', @level2name = N'StockStatus';

