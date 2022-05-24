CREATE TABLE [dbo].[Customer] (
    [CustomerId]   INT            IDENTITY (1, 1) NOT NULL,
    [CustomerCode] NVARCHAR (10)  NOT NULL,
    [CustomerName] NVARCHAR (20)  NOT NULL,
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
    PRIMARY KEY CLUSTERED ([CustomerId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶基本資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'CustomerId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'CustomerCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國家', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'CountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'城市', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'City';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'Addr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'郵遞區號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'ZipCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'統一編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'VatNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'Phone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'Remarks';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否停用 | Y:停用 N:不停用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'IsStop';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'CreateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'ModifyId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Customer', @level2type = N'COLUMN', @level2name = N'ModifyDate';

