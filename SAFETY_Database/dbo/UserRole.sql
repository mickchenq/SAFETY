CREATE TABLE [dbo].[UserRole] (
    [UserRoleId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserRoleName] NCHAR (10)     NULL,
    [Remarks]      NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]       CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]     INT            NOT NULL,
    [CreateDate]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]     INT            NULL,
    [ModifyDate]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([UserRoleId] ASC)
);

