CREATE TABLE [dbo].[ProductType] (
    [TypeId]     INT            IDENTITY (1, 1) NOT NULL,
    [TypeCode]   NVARCHAR (10)  NOT NULL,
    [TypeName]   NVARCHAR (20)  NOT NULL,
    [Remarks]    NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]     CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]   INT            NOT NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]   INT            NULL,
    [ModifyDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([TypeId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductType';

