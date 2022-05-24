CREATE TABLE [dbo].[Room] (
    [RoomId]      INT            IDENTITY (1, 1) NOT NULL,
    [RoomCode]    NVARCHAR (10)  NOT NULL,
    [RoomName]    NVARCHAR (20)  NOT NULL,
    [DcId]        INT            NOT NULL,
    [HouseId]     INT            NOT NULL,
    [RoomTypeId]  INT            NOT NULL,
    [TempLayerId] INT            NOT NULL,
    [Remarks]     NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]      CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]    INT            NOT NULL,
    [CreateDate]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]    INT            NULL,
    [ModifyDate]  DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RoomId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫別資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Room';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物流中心id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Room', @level2type = N'COLUMN', @level2name = N'DcId';

