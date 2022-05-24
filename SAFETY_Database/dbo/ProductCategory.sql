CREATE TABLE [dbo].[ProductCategory] (
    [CategoryId]   INT            IDENTITY (1, 1) NOT NULL,
    [CategoryCode] NVARCHAR (10)  NOT NULL,
    [CategoryName] NVARCHAR (20)  NOT NULL,
    [Remarks]      NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]       CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]     INT            NOT NULL,
    [CreateDate]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]     INT            NULL,
    [ModifyDate]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產品分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductCategory';

