CREATE TABLE [dbo].[Layer] (
    [LayerId]      INT            IDENTITY (1, 1) NOT NULL,
    [LayerCode]    NVARCHAR (10)  NOT NULL,
    [ShelfId]      INT            NOT NULL,
    [CurrentLayer] TINYINT        NOT NULL,
    [Fields]       SMALLINT       NOT NULL,
    [Height]       DECIMAL (9, 2) NOT NULL,
    [Depth]        DECIMAL (9, 2) NOT NULL,
    [Remarks]      NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]       CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]     INT            NOT NULL,
    [CreateDate]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]     INT            NULL,
    [ModifyDate]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([LayerId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層架資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Layer';

