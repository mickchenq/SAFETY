﻿CREATE TABLE [dbo].[LocationChangeOrder] (
    [OrderId]          INT            IDENTITY (1, 1) NOT NULL,
    [OrderNo]          NVARCHAR (20)  NOT NULL,
    [OrderDate]        DATE           NULL,
    [ProductStatus]    TINYINT        NOT NULL,
    [DcId]             INT            NOT NULL,
    [CustomerId]       INT            NOT NULL,
    [ProductId]        INT            NOT NULL,
    [ProductLotNo]     NVARCHAR (20)  NOT NULL,
    [Unit]             INT            DEFAULT ((0)) NOT NULL,
    [ExpirationDate]   DATE           NULL,
    [RoomId]           INT            NOT NULL,
    [LocationId]       INT            NOT NULL,
    [LocationQuantity] INT            NOT NULL,
    [ChangeQuantity]   INT            NULL,
    [NewDcId]          INT            NOT NULL,
    [NewHouseId]       INT            NOT NULL,
    [NewRoomId]        INT            NOT NULL,
    [NewShelfId]       INT            NOT NULL,
    [NewLayerId]       INT            NOT NULL,
    [NewLocationId]    INT            NOT NULL,
    [StockAdjustId]    INT            NOT NULL,
    [ChangeReason]     NVARCHAR (200) NOT NULL,
    [ChangeStatus]     TINYINT        NOT NULL,
    [CreateId]         INT            NOT NULL,
    [CreateDate]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyId]         INT            NULL,
    [ModifyDate]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);

