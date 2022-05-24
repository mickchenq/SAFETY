CREATE TABLE [dbo].[StockOutDetail] (
    [OrderDetailId]  INT           IDENTITY (1, 1) NOT NULL,
    [OrderId]        INT           NOT NULL,
    [ProductId]      INT           NOT NULL,
    [ProductLotNo]   NVARCHAR (20) NOT NULL,
    [Unit]           INT           DEFAULT ((1)) NOT NULL,
    [Quantity]       INT           NOT NULL,
    [ProductStatus]  TINYINT       NOT NULL,
    [ExpirationDate] DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderDetailId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出庫單明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockOutDetail';

