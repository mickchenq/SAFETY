CREATE TABLE [dbo].[SysUser] (
    [UserId]     INT            IDENTITY (1, 1) NOT NULL,
    [UserName]   NVARCHAR (20)  NOT NULL,
    [LoginId]    NVARCHAR (20)  NOT NULL,
    [UserType]   TINYINT        NOT NULL,
    [LoginPwd]   NVARCHAR (100) NOT NULL,
    [Remarks]    NVARCHAR (200) DEFAULT ('') NOT NULL,
    [IsStop]     CHAR (1)       DEFAULT ('N') NOT NULL,
    [CreateId]   INT            NOT NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]   INT            NULL,
    [ModifyDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SysUser';

