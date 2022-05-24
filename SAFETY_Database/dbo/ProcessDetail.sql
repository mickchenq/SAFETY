CREATE TABLE [dbo].[ProcessDetail] (
    [DetailId]      INT     IDENTITY (1, 1) NOT NULL,
    [OrderId]       INT     NOT NULL,
    [ProductId]     INT     NOT NULL,
    [Unit]          INT     NOT NULL,
    [Quantity]      INT     NOT NULL,
    [ProductStatus] TINYINT NOT NULL,
    PRIMARY KEY CLUSTERED ([DetailId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工通知單明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProcessDetail';

