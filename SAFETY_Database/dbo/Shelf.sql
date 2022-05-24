CREATE TABLE [dbo].[Shelf] (
    [ShelfId]         INT            IDENTITY (1, 1) NOT NULL,
    [ShelfCode]       NVARCHAR (10)  NOT NULL,
    [AreaId]          INT            NOT NULL,
    [ShelfTypeId]     INT            NOT NULL,
    [Racks]           TINYINT        NOT NULL,
    [PrevShelfId]     INT            NOT NULL,
    [NextShelfId]     INT            NOT NULL,
    [DownAisleWidth]  DECIMAL (9, 2) NULL,
    [UpAisleWidth]    DECIMAL (9, 2) NULL,
    [LeftAisleWidth]  DECIMAL (9, 2) NULL,
    [RightAisleWidth] DECIMAL (9, 2) NULL,
    [Width]           DECIMAL (9, 2) NULL,
    [Length]          DECIMAL (9, 2) NULL,
    [X1]              DECIMAL (9, 2) NULL,
    [X2]              DECIMAL (9, 2) NULL,
    [Y1]              DECIMAL (9, 2) NULL,
    [Y2]              DECIMAL (9, 2) NULL,
    [Remarks]         NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]          CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]        INT            NOT NULL,
    [CreateDate]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]        INT            NULL,
    [ModifyDate]      DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ShelfId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨架資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shelf';

