CREATE TABLE [dbo].[SysParam] (
    [SysParamId]     INT           IDENTITY (1, 1) NOT NULL,
    [SysParamCode]   NVARCHAR (10) NOT NULL,
    [SysParamNameCh] NVARCHAR (20) NOT NULL,
    [SysParamNameEn] NVARCHAR (50) NOT NULL,
    [DataValue]      NVARCHAR (10) NOT NULL,
    [IsStop]         CHAR (1)      DEFAULT ('N') NOT NULL,
    [CreateId]       INT           NOT NULL,
    [CreateDate]     DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifyId]       INT           NULL,
    [ModifyDate]     DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([SysParamId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'參數設定資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SysParam';

