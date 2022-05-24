CREATE TABLE [dbo].[StockOutLocationDetail] (
    [LocationDetailId] INT            IDENTITY (1, 1) NOT NULL,
    [OrderDetailId]    INT            NOT NULL,
    [LocationId]       INT            NOT NULL,
    [Unit]             INT            DEFAULT ((1)) NOT NULL,
    [LocationQuantity] INT            NOT NULL,
    [Remarks]          NVARCHAR (200) DEFAULT ('') NOT NULL,
    [DetailStatus]     TINYINT        DEFAULT ('1') NOT NULL,
    PRIMARY KEY CLUSTERED ([LocationDetailId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出庫單下架資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockOutLocationDetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態 | 1:待揀貨 2:揀貨中 3:已揀貨', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StockOutLocationDetail', @level2type = N'COLUMN', @level2name = N'DetailStatus';

