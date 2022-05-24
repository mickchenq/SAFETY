CREATE TABLE [dbo].[HouseType] (
    [HouseTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [HouseTypeCode] NVARCHAR (10)  NOT NULL,
    [HouseTypeName] NVARCHAR (20)  NOT NULL,
    [Remarks]       NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]        CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]      INT            NOT NULL,
    [CreateDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]      INT            NULL,
    [ModifyDate]    DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([HouseTypeId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別類型資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'HouseType';

