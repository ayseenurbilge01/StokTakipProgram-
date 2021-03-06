USE [stoktakipprogrami]
GO
/****** Object:  Table [dbo].[kullanicilar]    Script Date: 11.06.2021 15:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[kullanicilar](
	[kullaniciid] [int] IDENTITY(1,1) NOT NULL,
	[kullaniciadi] [nvarchar](50) NOT NULL,
	[sifre] [varchar](8) NULL,
	[bakiye] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[kullaniciid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sepetim]    Script Date: 11.06.2021 15:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sepetim](
	[sepetid] [int] IDENTITY(1,1) NOT NULL,
	[kullaniciid] [int] NOT NULL,
	[urunkodu] [int] NOT NULL,
	[tarih] [datetime] NULL,
	[saat] [time](7) NULL,
	[urunadedi] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[sepetid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[urunler]    Script Date: 11.06.2021 15:41:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[urunler](
	[urunkodu] [int] NOT NULL,
	[urunadi] [nvarchar](max) NOT NULL,
	[urunadedi] [int] NOT NULL,
	[tarih] [datetime] NOT NULL,
	[saat] [time](7) NOT NULL,
	[resim] [text] NULL,
	[cinsiyet] [nvarchar](5) NULL,
	[fiyat] [int] NULL,
 CONSTRAINT [PK_urunler] PRIMARY KEY CLUSTERED 
(
	[urunkodu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[kullanicilar] ON 

INSERT [dbo].[kullanicilar] ([kullaniciid], [kullaniciadi], [sifre], [bakiye]) VALUES (5, N'Yonetici', N'Sifre123', 0)
INSERT [dbo].[kullanicilar] ([kullaniciid], [kullaniciadi], [sifre], [bakiye]) VALUES (7, N'cumali', N'123456a', 20)
SET IDENTITY_INSERT [dbo].[kullanicilar] OFF
INSERT [dbo].[urunler] ([urunkodu], [urunadi], [urunadedi], [tarih], [saat], [resim], [cinsiyet], [fiyat]) VALUES (724, N'ELBİSE', 4, CAST(N'2021-11-06T00:00:00.000' AS DateTime), CAST(N'14:08:00' AS Time), N'C:\Users\hp\Desktop\kıyafetler\kadıntshirt.jpg', N'KADIN', 500)
INSERT [dbo].[urunler] ([urunkodu], [urunadi], [urunadedi], [tarih], [saat], [resim], [cinsiyet], [fiyat]) VALUES (919, N'T-SHİRT', 4, CAST(N'2021-11-06T00:00:00.000' AS DateTime), CAST(N'14:07:00' AS Time), N'C:\Users\hp\Desktop\kıyafetler\kadıntshirt.jpg', N'KADIN', 80)
INSERT [dbo].[urunler] ([urunkodu], [urunadi], [urunadedi], [tarih], [saat], [resim], [cinsiyet], [fiyat]) VALUES (950, N'T-SHİRT', 4, CAST(N'2021-11-06T00:00:00.000' AS DateTime), CAST(N'14:08:00' AS Time), N'C:\Users\hp\Desktop\kıyafetler\kadıntshirt.jpg', N'ERKEK', 80)
ALTER TABLE [dbo].[kullanicilar] ADD  CONSTRAINT [kullanici_bakiye_durum]  DEFAULT ((0)) FOR [bakiye]
GO
ALTER TABLE [dbo].[sepetim]  WITH CHECK ADD FOREIGN KEY([kullaniciid])
REFERENCES [dbo].[kullanicilar] ([kullaniciid])
GO
ALTER TABLE [dbo].[sepetim]  WITH CHECK ADD FOREIGN KEY([urunkodu])
REFERENCES [dbo].[urunler] ([urunkodu])
GO
