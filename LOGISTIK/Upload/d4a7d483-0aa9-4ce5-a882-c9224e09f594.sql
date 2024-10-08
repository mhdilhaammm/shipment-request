USE [master]
GO
/****** Object:  Database [GOODSAdminContext-20211103141405]    Script Date: 21/08/2023 18.45.23 ******/
CREATE DATABASE [GOODSAdminContext-20211103141405]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GOODSAdminContext-20211103141405.mdf', FILENAME = N'C:\Users\NugrohoMuham\Documents\GOODSAdmin\GOODSAdmin\App_Data\GOODSAdminContext-20211103141405.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GOODSAdminContext-20211103141405_log.ldf', FILENAME = N'C:\Users\NugrohoMuham\Documents\GOODSAdmin\GOODSAdmin\App_Data\GOODSAdminContext-20211103141405_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GOODSAdminContext-20211103141405].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET ARITHABORT OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET  ENABLE_BROKER 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET  MULTI_USER 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET QUERY_STORE = OFF
GO
USE [GOODSAdminContext-20211103141405]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [GOODSAdminContext-20211103141405]
GO
/****** Object:  Schema [LOGISTIC_GOODSADMIN]    Script Date: 21/08/2023 18.45.23 ******/
CREATE SCHEMA [LOGISTIC_GOODSADMIN]
GO
/****** Object:  Table [LOGISTIC_GOODSADMIN].[__MigrationHistory]    Script Date: 21/08/2023 18.45.23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LOGISTIC_GOODSADMIN].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [LOGISTIC_GOODSADMIN].[ItemNames]    Script Date: 21/08/2023 18.45.23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LOGISTIC_GOODSADMIN].[ItemNames](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [nvarchar](max) NULL,
	[Qty] [nvarchar](max) NULL,
	[Uom] [nvarchar](max) NULL,
	[Item2] [nvarchar](max) NULL,
	[Qty2] [nvarchar](max) NULL,
	[Uom2] [nvarchar](max) NULL,
	[Item3] [nvarchar](max) NULL,
	[Qty3] [nvarchar](max) NULL,
	[Uom3] [nvarchar](max) NULL,
	[Item4] [nvarchar](max) NULL,
	[Qty4] [nvarchar](max) NULL,
	[Uom4] [nvarchar](max) NULL,
	[Item5] [nvarchar](max) NULL,
	[Qty5] [nvarchar](max) NULL,
	[Uom5] [nvarchar](max) NULL,
	[Item6] [nvarchar](max) NULL,
	[Qty6] [nvarchar](max) NULL,
	[Uom6] [nvarchar](max) NULL,
	[Item7] [nvarchar](max) NULL,
	[Qty7] [nvarchar](max) NULL,
	[Uom7] [nvarchar](max) NULL,
	[Item8] [nvarchar](max) NULL,
	[Qty8] [nvarchar](max) NULL,
	[Uom8] [nvarchar](max) NULL,
	[Item9] [nvarchar](max) NULL,
	[Qty9] [nvarchar](max) NULL,
	[Uom9] [nvarchar](max) NULL,
	[Item10] [nvarchar](max) NULL,
	[Qty10] [nvarchar](max) NULL,
	[Uom10] [nvarchar](max) NULL,
	[Item11] [nvarchar](max) NULL,
	[Qty11] [nvarchar](max) NULL,
	[Uom11] [nvarchar](max) NULL,
	[Item12] [nvarchar](max) NULL,
	[Qty12] [nvarchar](max) NULL,
	[Uom12] [nvarchar](max) NULL,
	[Item13] [nvarchar](max) NULL,
	[Qty13] [nvarchar](max) NULL,
	[Uom13] [nvarchar](max) NULL,
	[Item14] [nvarchar](max) NULL,
	[Qty14] [nvarchar](max) NULL,
	[Uom14] [nvarchar](max) NULL,
	[Item15] [nvarchar](max) NULL,
	[Qty15] [nvarchar](max) NULL,
	[Uom15] [nvarchar](max) NULL,
	[ToEmail] [nvarchar](max) NULL,
	[Id_ListTransactions] [int] NULL,
 CONSTRAINT [PK_dbo.ItemNames] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [LOGISTIC_GOODSADMIN].[ListTransactions]    Script Date: 21/08/2023 18.45.23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LOGISTIC_GOODSADMIN].[ListTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[PurchaseOrder] [nvarchar](max) NULL,
	[DeliveryOrder] [nvarchar](max) NULL,
	[Forwarder] [nvarchar](max) NULL,
	[InvoiceNo] [nvarchar](max) NULL,
	[ShipperName] [nvarchar](max) NULL,
	[AWBReff] [nvarchar](max) NULL,
	[Package] [nvarchar](max) NULL,
	[SatuanPackage] [nvarchar](max) NULL,
	[Unit] [nvarchar](max) NULL,
	[EnduserName] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[CollectorName] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Images] [nvarchar](max) NULL,
	[Kategori] [nvarchar](max) NULL,
	[Locations] [nvarchar](50) NULL,
	[ReceiverName] [nvarchar](50) NULL,
	[QR] [nvarchar](max) NULL,
	[DateCollect] [datetime] NULL,
	[CycleTime] [nvarchar](50) NULL,
	[RunNumber] [int] NULL,
 CONSTRAINT [PK_dbo.ListTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [LOGISTIC_GOODSADMIN].[Users]    Script Date: 21/08/2023 18.45.23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LOGISTIC_GOODSADMIN].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WindowsAccount] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[HakUser] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Id_ListTransactions]    Script Date: 21/08/2023 18.45.23 ******/
CREATE NONCLUSTERED INDEX [IX_Id_ListTransactions] ON [LOGISTIC_GOODSADMIN].[ItemNames]
(
	[Id_ListTransactions] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [LOGISTIC_GOODSADMIN].[ItemNames]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ItemNames_dbo.ListTransactions_Id_ListTransactions] FOREIGN KEY([Id_ListTransactions])
REFERENCES [LOGISTIC_GOODSADMIN].[ListTransactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [LOGISTIC_GOODSADMIN].[ItemNames] CHECK CONSTRAINT [FK_dbo.ItemNames_dbo.ListTransactions_Id_ListTransactions]
GO
/****** Object:  StoredProcedure [dbo].[getemprecordssp]    Script Date: 21/08/2023 18.45.23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getemprecordssp]
@start datetime,
@end datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from [dbo].[ListTransactions] where Date between @start and @end
END
GO
/****** Object:  StoredProcedure [dbo].[itemnamesssp]    Script Date: 21/08/2023 18.45.23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[itemnamesssp]
@start datetime,
@end datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from [dbo].[ListTransactions] where Date between @start and @end
END
GO
USE [master]
GO
ALTER DATABASE [GOODSAdminContext-20211103141405] SET  READ_WRITE 
GO
