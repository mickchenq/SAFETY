CREATE TABLE [dbo].[StockInDetail] (
    [OrderDetailId]    INT            IDENTITY (1, 1) NOT NULL,
    [OrderId]          INT            NOT NULL,
    [ProductId]        INT            NOT NULL,
    [ProductLotNo]     NVARCHAR (20)  NOT NULL,
    [Unit]             INT            DEFAULT ((1)) NOT NULL,
    [Quantity]         INT            NOT NULL,
    [LocationQuantity] INT            NOT NULL,
    [MakeDate]         DATE           NULL,
    [ExpirationDate]   DATE           NULL,
    [Remarks]          NVARCHAR (200) DEFAULT ('') NOT NULL,
    [ProductStatus]    TINYINT        NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderDetailId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'入庫明細id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockInDetail', @level2type = N'COLUMN', @level2name = N'OrderDetailId';

