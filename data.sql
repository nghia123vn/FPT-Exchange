USE [FPT_Exchange_DB]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImageProduct]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageProduct](
	[Id] [uniqueidentifier] NOT NULL,
	[Product_ID] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateAt] [datetime] NOT NULL,
	[SendTo] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[Price] [int] NOT NULL,
	[CategoryID] [uniqueidentifier] NOT NULL,
	[Status_ID] [uniqueidentifier] NOT NULL,
	[StationID] [uniqueidentifier] NOT NULL,
	[AddByID] [uniqueidentifier] NOT NULL,
	[SellerID] [uniqueidentifier] NULL,
	[BuyerID] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductActivy]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductActivy](
	[Id] [uniqueidentifier] NOT NULL,
	[ActionType] [nvarchar](50) NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[Stations_ID] [uniqueidentifier] NOT NULL,
	[OldStatus] [uniqueidentifier] NOT NULL,
	[NewStatus] [uniqueidentifier] NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTransfer]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTransfer](
	[Id] [uniqueidentifier] NOT NULL,
	[StationIDForm] [uniqueidentifier] NOT NULL,
	[StationIDTo] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[DateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTransferItem]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTransferItem](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[ProductTransferID] [uniqueidentifier] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stations]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stations](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[WalletID] [uniqueidentifier] NOT NULL,
	[Amount] [int] NOT NULL,
	[Fee] [int] NOT NULL,
	[Receive] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Avatar] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](256) NULL,
	[AccessToken] [nvarchar](256) NULL,
	[RefreshToken] [nvarchar](256) NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[StationID] [uniqueidentifier] NULL,
	[Status] [nvarchar](256) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 29/06/2023 10:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[ID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Score] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Category] ([Id], [Name]) VALUES (N'888c2541-04eb-4fe9-b883-e8092fceb1e9', N'Đồng phục')
GO
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'a530e7c2-9d44-4ce7-82f7-3edd17b57734', N'Admin')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'3d2c1859-4e4f-4cfa-9efb-afbcd6a7a17d', N'Staff')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'61b99726-8e73-4a0c-be55-c7855bb0a0c3', N'Customer')
GO
INSERT [dbo].[Stations] ([Id], [Name], [Address]) VALUES (N'e6cd9eee-b8dd-4d72-ba5f-7b8eaea6de68', N'Đại học FPT HCM', N'Long Thạch Mỹ, Quận 9, TP.HCM')
INSERT [dbo].[Stations] ([Id], [Name], [Address]) VALUES (N'6ed3195e-8a08-4fc2-b46a-b22da4f0d89d', N'Nhà Văn Hóa', N'Thủ Đức')
GO
INSERT [dbo].[Status] ([Id], [Name]) VALUES (N'8dda5aac-4c23-4abd-a2e1-f5fe677dc198', N'Active')
GO
INSERT [dbo].[User] ([Id], [Name], [Avatar], [Email], [Password], [AccessToken], [RefreshToken], [RoleID], [StationID], [Status], [CreateAt]) VALUES (N'39f8c764-4ac1-4044-b828-1b49b3d1fbc8', N'Nguyễn Tấn Trung', NULL, N'tantrung.staff@gmail.com', N'123456', NULL, NULL, N'3d2c1859-4e4f-4cfa-9efb-afbcd6a7a17d', N'6ed3195e-8a08-4fc2-b46a-b22da4f0d89d', N'Active', CAST(N'2023-06-29T10:46:32.613' AS DateTime))
INSERT [dbo].[User] ([Id], [Name], [Avatar], [Email], [Password], [AccessToken], [RefreshToken], [RoleID], [StationID], [Status], [CreateAt]) VALUES (N'40f5d35a-2414-4580-b35f-6af67417c4d2', N'Nguyễn Thanh Nam', NULL, N'thanhnam.staff@gmail.com', N'123456', NULL, NULL, N'3d2c1859-4e4f-4cfa-9efb-afbcd6a7a17d', N'e6cd9eee-b8dd-4d72-ba5f-7b8eaea6de68', N'Active', CAST(N'2023-06-29T10:51:47.030' AS DateTime))
GO
/****** Object:  Index [UQ__Wallet__1788CCADD3191D25]    Script Date: 29/06/2023 10:55:39 ******/
ALTER TABLE [dbo].[Wallet] ADD UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProductActivy] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ProductTransfer] ADD  DEFAULT (getdate()) FOR [DateTime]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[ImageProduct]  WITH CHECK ADD FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([SendTo])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([AddByID])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([BuyerID])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([SellerID])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([StationID])
REFERENCES [dbo].[Stations] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([Status_ID])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[ProductActivy]  WITH CHECK ADD FOREIGN KEY([NewStatus])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[ProductActivy]  WITH CHECK ADD FOREIGN KEY([OldStatus])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[ProductActivy]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[ProductActivy]  WITH CHECK ADD FOREIGN KEY([Stations_ID])
REFERENCES [dbo].[Stations] ([Id])
GO
ALTER TABLE [dbo].[ProductActivy]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ProductTransfer]  WITH CHECK ADD FOREIGN KEY([StationIDForm])
REFERENCES [dbo].[Stations] ([Id])
GO
ALTER TABLE [dbo].[ProductTransfer]  WITH CHECK ADD FOREIGN KEY([StationIDTo])
REFERENCES [dbo].[Stations] ([Id])
GO
ALTER TABLE [dbo].[ProductTransfer]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ProductTransferItem]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[ProductTransferItem]  WITH CHECK ADD FOREIGN KEY([ProductTransferID])
REFERENCES [dbo].[ProductTransfer] ([Id])
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([WalletID])
REFERENCES [dbo].[Wallet] ([ID])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([StationID])
REFERENCES [dbo].[Stations] ([Id])
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Wallet] CHECK CONSTRAINT [FK_Wallet_User]
GO
