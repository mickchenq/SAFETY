CREATE TABLE [dbo].[Area] (
    [AreaId]     INT            IDENTITY (1, 1) NOT NULL,
    [AreaCode]   NVARCHAR (10)  NOT NULL,
    [AreaName]   NVARCHAR (20)  NOT NULL,
    [RoomId]     INT            NOT NULL,
    [AreaType]   TINYINT        NOT NULL,
    [Remarks]    NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]     CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]   INT            NOT NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]   INT            NULL,
    [ModifyDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([AreaId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲區資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲區id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'AreaId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲區代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'AreaCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲區名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'AreaName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫別id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'RoomId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲區型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'AreaType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'Remarks';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否停用 | Y:停用 N:不停用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'IsStop';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'CreateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'ModifyId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'ModifyDate';

