CREATE TABLE [dbo].[Product] (
    [ProductId]   INT            IDENTITY (1, 1) NOT NULL,
    [ProductCode] NVARCHAR (10)  NOT NULL,
    [CustomerId]  INT            NOT NULL,
    [ProductName] NVARCHAR (50)  NOT NULL,
    [Barcode]     NVARCHAR (20)  NOT NULL,
    [TempLayerId] INT            NULL,
    [CategoryId]  INT            NULL,
    [TypeId]      INT            NULL,
    [SupplierId]  INT            NULL,
    [Remarks]     NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]      CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]    INT            NOT NULL,
    [CreateDate]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]    INT            NULL,
    [ModifyDate]  DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶商品資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Product';

