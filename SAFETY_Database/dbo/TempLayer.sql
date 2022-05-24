CREATE TABLE [dbo].[TempLayer] (
    [TempId]     INT            IDENTITY (1, 1) NOT NULL,
    [TempCode]   NVARCHAR (10)  NOT NULL,
    [TempName]   NVARCHAR (10)  NOT NULL,
    [MaxTemp]    DECIMAL (5, 2) NOT NULL,
    [MinTemp]    DECIMAL (5, 2) NOT NULL,
    [Remarks]    NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]     CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]   INT            NOT NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]   INT            NULL,
    [ModifyDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([TempId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'溫層資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TempLayer';

