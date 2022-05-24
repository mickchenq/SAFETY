CREATE TABLE [dbo].[DataCenter] (
    [DcId]       INT            IDENTITY (1, 1) NOT NULL,
    [DcCode]     NVARCHAR (10)  NOT NULL,
    [DcName]     NVARCHAR (20)  NOT NULL,
    [Remarks]    NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]     CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]   INT            NOT NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]   INT            NULL,
    [ModifyDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([DcId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物流中心', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物流中心id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'DcId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物流中心代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'DcCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物流中心名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'DcName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'Remarks';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否停用 | Y:停用 N:不停用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'IsStop';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'CreateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'ModifyId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DataCenter', @level2type = N'COLUMN', @level2name = N'ModifyDate';

