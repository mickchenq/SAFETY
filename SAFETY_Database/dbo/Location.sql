CREATE TABLE [dbo].[Location] (
    [LocationId]    INT            IDENTITY (1, 1) NOT NULL,
    [LayerId]       INT            NOT NULL,
    [TagAddr]       INT            NOT NULL,
    [LocationCode]  NVARCHAR (10)  NOT NULL,
    [SequenceNo]    SMALLINT       NOT NULL,
    [PlateQuantity] SMALLINT       NOT NULL,
    [MedianX]       DECIMAL (9, 2) NOT NULL,
    [MedianY]       DECIMAL (9, 2) NOT NULL,
    [Width]         DECIMAL (9, 2) NOT NULL,
    [IsStackable]   CHAR (1)       DEFAULT ('N') NOT NULL,
    [IsMixable]     CHAR (1)       DEFAULT ('N') NOT NULL,
    [Square]        DECIMAL (9, 4) NOT NULL,
    [Weight]        DECIMAL (9, 2) NOT NULL,
    [Remarks]       NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]        CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]      INT            NOT NULL,
    [CreateDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]      INT            NULL,
    [ModifyDate]    DATETIME       NULL,
    [TagGateWay]    INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([LocationId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tag機器ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Location', @level2type = N'COLUMN', @level2name = N'TagAddr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位GateWay', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Location', @level2type = N'COLUMN', @level2name = N'TagGateWay';

