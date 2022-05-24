CREATE TABLE [dbo].[ReturnedDetail] (
    [ReturnedDetailId] INT           IDENTITY (1, 1) NOT NULL,
    [OrderId]          INT           NOT NULL,
    [ProductId]        INT           NOT NULL,
    [ProductLotNo]     NVARCHAR (20) NOT NULL,
    [Unit]             INT           NOT NULL,
    [Quantity]         INT           NOT NULL,
    [ProductStatus]    TINYINT       NOT NULL,
    [ExpirationDate]   DATE          NOT NULL,
    [MakeDate]         DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([ReturnedDetailId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退貨通知單明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnedDetail';

