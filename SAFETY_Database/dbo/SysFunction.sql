CREATE TABLE [dbo].[SysFunction] (
    [FunctionId]   INT            IDENTITY (1, 1) NOT NULL,
    [FunctionName] NVARCHAR (20)  CONSTRAINT [DF__tmp_ms_xx__Funct__7FB5F314] DEFAULT ('') NOT NULL,
    [FnGroup]      VARCHAR (30)   CONSTRAINT [DF__tmp_ms_xx__FnGro__00AA174D] DEFAULT ('') NOT NULL,
    [FnGroupName]  NVARCHAR (30)  CONSTRAINT [DF__tmp_ms_xx__FnGro__019E3B86] DEFAULT ('') NOT NULL,
    [FnClass]      VARCHAR (30)   CONSTRAINT [DF__tmp_ms_xx__FnCla__02925FBF] DEFAULT ('') NOT NULL,
    [FnArea]       VARCHAR (30)   CONSTRAINT [DF__tmp_ms_xx__FnAre__038683F8] DEFAULT ('') NOT NULL,
    [FnController] VARCHAR (30)   CONSTRAINT [DF__tmp_ms_xx__FnCon__047AA831] DEFAULT ('') NOT NULL,
    [FnPageName]   VARCHAR (30)   CONSTRAINT [DF__tmp_ms_xx__FnPag__056ECC6A] DEFAULT ('') NOT NULL,
    [Remarks]      NVARCHAR (200) CONSTRAINT [DF__tmp_ms_xx__Remar__0662F0A3] DEFAULT ('') NOT NULL,
    [IsStop]       CHAR (1)       CONSTRAINT [DF__tmp_ms_xx__IsSto__075714DC] DEFAULT ('N') NOT NULL,
    [CreateId]     INT            NOT NULL,
    [CreateDate]   DATETIME       CONSTRAINT [DF__tmp_ms_xx__Creat__084B3915] DEFAULT (getdate()) NOT NULL,
    [ModifyId]     INT            NULL,
    [ModifyDate]   DATETIME       NULL,
    CONSTRAINT [PK_SysFunction] PRIMARY KEY CLUSTERED ([FunctionId] ASC)
);

