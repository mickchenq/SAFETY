CREATE TABLE [dbo].[ProductPackage] (
    [PackageId]             INT            IDENTITY (1, 1) NOT NULL,
    [PackageName]           NVARCHAR (10)  NOT NULL,
    [ProductId]             INT            NOT NULL,
    [Length]                DECIMAL (9, 2) NOT NULL,
    [Width]                 DECIMAL (9, 2) NOT NULL,
    [Height]                DECIMAL (9, 2) NOT NULL,
    [Weight]                DECIMAL (9, 2) NOT NULL,
    [ParentPackageId]       INT            NOT NULL,
    [ParentPackageQuantity] INT            NOT NULL,
    [IsMinSku]              CHAR (1)       DEFAULT ('N') NOT NULL,
    [Remarks]               NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]                CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]              INT            NOT NULL,
    [CreateDate]            DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]              INT            NULL,
    [ModifyDate]            DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([PackageId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商品包裝資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductPackage';

