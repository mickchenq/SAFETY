CREATE TABLE [dbo].[UserRoleFunction] (
    [UserRoleId] INT      NOT NULL,
    [FunctionId] INT      NOT NULL,
    [CreateId]   INT      NOT NULL,
    [CreateDate] DATETIME DEFAULT (getdate()) NOT NULL,
    [ModifyId]   INT      NULL,
    [ModifyDate] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([FunctionId] ASC, [UserRoleId] ASC)
);

