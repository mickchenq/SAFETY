CREATE TABLE [dbo].[Inventory] (
    [InventoryId]      INT           IDENTITY (1, 1) NOT NULL,
    [InventoryKind]    CHAR (1)      NOT NULL,
    [InventoryStatus]  TINYINT       NOT NULL,
    [DcId]             INT           NOT NULL,
    [InvDate]          DATE          NOT NULL,
    [LocationDetailId] INT           NOT NULL,
    [CustomerId]       INT           NOT NULL,
    [ProductId]        INT           NOT NULL,
    [LocationId]       INT           NOT NULL,
    [LocationQuantity] INT           NOT NULL,
    [Unit]             INT           NOT NULL,
    [ProductLotNo]     NVARCHAR (20) NOT NULL,
    [ExpirationDate]   DATE          NOT NULL,
    [ProductStatus]    TINYINT       NOT NULL,
    [CreateId]         INT           NOT NULL,
    [CreateDate]       DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifyId]         INT           NULL,
    [ModifyDate]       DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([InventoryId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出入庫流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'InventoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別 | I:入庫 O:出庫 A:盤點調整 S:庫存調整', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'InventoryKind';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態 | 1:待上架 2:已上架 3:待揀貨 4:已揀貨', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Inventory', @level2type = N'COLUMN', @level2name = N'InventoryStatus';

