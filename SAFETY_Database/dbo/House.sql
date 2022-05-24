CREATE TABLE [dbo].[House] (
    [HouseId]     INT            IDENTITY (1, 1) NOT NULL,
    [HouseCode]   NVARCHAR (10)  NOT NULL,
    [HouseName]   NVARCHAR (20)  NOT NULL,
    [HouseTypeId] INT            NOT NULL,
    [DcId]        INT            NOT NULL,
    [Remarks]     NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]      CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]    INT            NOT NULL,
    [CreateDate]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]    INT            NULL,
    [ModifyDate]  DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([HouseId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'HouseId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'HouseCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'HouseName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別類型id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'HouseTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物流中心id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'DcId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'Remarks';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否停用 | Y:停用 N:不停用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'IsStop';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'CreateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'ModifyId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'House', @level2type = N'COLUMN', @level2name = N'ModifyDate';

