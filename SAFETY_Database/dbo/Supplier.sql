CREATE TABLE [dbo].[Supplier] (
    [SupplierId]   INT            IDENTITY (1, 1) NOT NULL,
    [SupplierCode] NVARCHAR (10)  NULL,
    [SupplierName] NVARCHAR (20)  NULL,
    [CountryCode]  NVARCHAR (10)  NULL,
    [City]         NVARCHAR (20)  NULL,
    [Addr]         NVARCHAR (100) NULL,
    [ZipCode]      NVARCHAR (10)  NULL,
    [VatNo]        NVARCHAR (20)  NULL,
    [Phone]        NVARCHAR (20)  NULL,
    [Remarks]      NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]       CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]     INT            NOT NULL,
    [CreateDate]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]     INT            NULL,
    [ModifyDate]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([SupplierId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商基本資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supplier';

