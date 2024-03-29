USE [otomasyon_veritabani]
GO
/****** Object:  Table [dbo].[biletler]    Script Date: 1/14/2024 1:21:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[biletler](
	[bilet_id] [int] IDENTITY(1,1) NOT NULL,
	[koltuk_no] [varchar](30) NOT NULL,
	[fiyat] [money] NOT NULL,
	[sefer_id] [int] NOT NULL,
	[musteri_id] [int] NOT NULL,
	[odeme_tarihi] [date] NOT NULL,
	[odeme_turu] [varchar](20) NOT NULL,
	[bilet_turu] [varchar](20) NOT NULL,
 CONSTRAINT [PK_ucak_bileti] PRIMARY KEY CLUSTERED 
(
	[bilet_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[check_in]    Script Date: 1/14/2024 1:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[check_in](
	[check_in_id] [int] IDENTITY(1,1) NOT NULL,
	[check_in_tarihi] [date] NOT NULL,
	[bilet_id] [int] NOT NULL,
 CONSTRAINT [PK_check_in] PRIMARY KEY CLUSTERED 
(
	[check_in_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[musteri_bilgileri]    Script Date: 1/14/2024 1:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[musteri_bilgileri](
	[musteri_id] [int] IDENTITY(1,1) NOT NULL,
	[tc_pas_no] [varchar](500) NOT NULL,
	[ad] [varchar](20) NOT NULL,
	[soyad] [varchar](20) NOT NULL,
	[telefon_no] [varchar](50) NOT NULL,
	[uyruk] [varchar](20) NOT NULL,
	[tekerlekli_sandalye] [bit] NOT NULL,
 CONSTRAINT [PK_musteri_bilgileri] PRIMARY KEY CLUSTERED 
(
	[musteri_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[personeller]    Script Date: 1/14/2024 1:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[personeller](
	[personal_id] [int] IDENTITY(1,1) NOT NULL,
	[ad] [varchar](20) NOT NULL,
	[soyad] [varchar](20) NOT NULL,
	[tc_no] [bigint] NOT NULL,
	[dogum_tarihi] [date] NOT NULL,
	[sifre] [varchar](20) NULL,
	[gorev] [varchar](20) NOT NULL,
	[fotoğraf] [varbinary](max) NULL,
	[tecrube] [date] NULL,
	[cinsiyet] [varchar](20) NOT NULL,
	[cv] [varbinary](max) NULL,
	[calistigi_birim] [varchar](20) NULL,
 CONSTRAINT [PK_personeller] PRIMARY KEY CLUSTERED 
(
	[personal_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[seferler]    Script Date: 1/14/2024 1:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[seferler](
	[sefer_id] [int] IDENTITY(1,1) NOT NULL,
	[arac_id] [int] NOT NULL,
	[ulasim_turu] [varchar](20) NOT NULL,
	[kalkis_yeri] [varchar](100) NOT NULL,
	[kalkis_tarihi] [date] NOT NULL,
	[kalkis_saati] [time](0) NOT NULL,
	[varis_yeri] [varchar](100) NOT NULL,
	[varis_tarihi] [date] NOT NULL,
	[varis_saati] [time](0) NOT NULL,
	[fiyat] [money] NOT NULL,
	[personel_1_id] [int] NULL,
	[personel_2_id] [int] NULL,
	[personel_3_id] [int] NULL,
	[personel_4_id] [int] NULL,
	[personel_5_id] [int] NULL,
	[personel_6_id] [int] NULL,
 CONSTRAINT [PK_personel] PRIMARY KEY CLUSTERED 
(
	[sefer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ulasim_araclari]    Script Date: 1/14/2024 1:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ulasim_araclari](
	[arac_id] [int] IDENTITY(1,1) NOT NULL,
	[arac_modelli] [varchar](30) NOT NULL,
	[ulasim_tur] [varchar](20) NOT NULL,
	[koltuk_sayisi] [int] NOT NULL,
 CONSTRAINT [PK_ulasim_araclari] PRIMARY KEY CLUSTERED 
(
	[arac_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[biletler]  WITH CHECK ADD  CONSTRAINT [FK_biletler_musteri_bilgileri] FOREIGN KEY([musteri_id])
REFERENCES [dbo].[musteri_bilgileri] ([musteri_id])
GO
ALTER TABLE [dbo].[biletler] CHECK CONSTRAINT [FK_biletler_musteri_bilgileri]
GO
ALTER TABLE [dbo].[biletler]  WITH CHECK ADD  CONSTRAINT [FK_biletler_seferler] FOREIGN KEY([sefer_id])
REFERENCES [dbo].[seferler] ([sefer_id])
GO
ALTER TABLE [dbo].[biletler] CHECK CONSTRAINT [FK_biletler_seferler]
GO
ALTER TABLE [dbo].[check_in]  WITH CHECK ADD  CONSTRAINT [FK_check_in_biletler] FOREIGN KEY([bilet_id])
REFERENCES [dbo].[biletler] ([bilet_id])
GO
ALTER TABLE [dbo].[check_in] CHECK CONSTRAINT [FK_check_in_biletler]
GO
ALTER TABLE [dbo].[seferler]  WITH CHECK ADD  CONSTRAINT [FK_seferler_personeller] FOREIGN KEY([personel_1_id])
REFERENCES [dbo].[personeller] ([personal_id])
GO
ALTER TABLE [dbo].[seferler] CHECK CONSTRAINT [FK_seferler_personeller]
GO
ALTER TABLE [dbo].[seferler]  WITH CHECK ADD  CONSTRAINT [FK_seferler_personeller1] FOREIGN KEY([personel_2_id])
REFERENCES [dbo].[personeller] ([personal_id])
GO
ALTER TABLE [dbo].[seferler] CHECK CONSTRAINT [FK_seferler_personeller1]
GO
ALTER TABLE [dbo].[seferler]  WITH CHECK ADD  CONSTRAINT [FK_seferler_personeller2] FOREIGN KEY([personel_3_id])
REFERENCES [dbo].[personeller] ([personal_id])
GO
ALTER TABLE [dbo].[seferler] CHECK CONSTRAINT [FK_seferler_personeller2]
GO
ALTER TABLE [dbo].[seferler]  WITH CHECK ADD  CONSTRAINT [FK_seferler_personeller3] FOREIGN KEY([personel_4_id])
REFERENCES [dbo].[personeller] ([personal_id])
GO
ALTER TABLE [dbo].[seferler] CHECK CONSTRAINT [FK_seferler_personeller3]
GO
ALTER TABLE [dbo].[seferler]  WITH CHECK ADD  CONSTRAINT [FK_seferler_personeller4] FOREIGN KEY([personel_5_id])
REFERENCES [dbo].[personeller] ([personal_id])
GO
ALTER TABLE [dbo].[seferler] CHECK CONSTRAINT [FK_seferler_personeller4]
GO
ALTER TABLE [dbo].[seferler]  WITH CHECK ADD  CONSTRAINT [FK_seferler_personeller5] FOREIGN KEY([personel_6_id])
REFERENCES [dbo].[personeller] ([personal_id])
GO
ALTER TABLE [dbo].[seferler] CHECK CONSTRAINT [FK_seferler_personeller5]
GO
ALTER TABLE [dbo].[seferler]  WITH CHECK ADD  CONSTRAINT [FK_seferler_ulasim_araclari] FOREIGN KEY([arac_id])
REFERENCES [dbo].[ulasim_araclari] ([arac_id])
GO
ALTER TABLE [dbo].[seferler] CHECK CONSTRAINT [FK_seferler_ulasim_araclari]
GO
