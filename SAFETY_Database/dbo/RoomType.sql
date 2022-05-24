CREATE TABLE [dbo].[RoomType] (
    [RoomTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [RoomTypeCode] NVARCHAR (10)  NOT NULL,
    [RoomTypeName] NVARCHAR (20)  NOT NULL,
    [Remarks]      NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]       CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]     INT            NOT NULL,
    [CreateDate]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]     INT            NULL,
    [ModifyDate]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RoomTypeId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫別類別資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RoomType';

