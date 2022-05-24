CREATE TABLE [dbo].[UploadFiles] (
    [UploadId]   INT            IDENTITY (1, 1) NOT NULL,
    [FormKind]   INT            NOT NULL,
    [FormId]     INT            NOT NULL,
    [FileName]   NVARCHAR (50)  NOT NULL,
    [FilePath]   NVARCHAR (200) NOT NULL,
    [CreateId]   INT            NOT NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]   INT            NULL,
    [ModifyDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([UploadId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上傳資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上傳檔案id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'UploadId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表單類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'FormKind';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表單Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'FormId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檔案名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'FileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檔案路徑', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'FilePath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'CreateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改者id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'ModifyId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UploadFiles', @level2type = N'COLUMN', @level2name = N'ModifyDate';

