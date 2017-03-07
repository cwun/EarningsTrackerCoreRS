/*
 *  1) Create Database 
 */
DECLARE @DbName NVARCHAR(128) = N'EarningsTracker'


IF NOT EXISTS (	SELECT	name  
				FROM	master.dbo.sysdatabases  
				WHERE	('[' + name + ']' = @DbName 
				OR		name = @DbName))
BEGIN
	CREATE DATABASE EarningsTracker;
END
GO

USE [EarningsTracker]
GO

/*
 *  2) Drop Foreign Key Constraints
 */
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_income_office_id]') AND parent_object_id = OBJECT_ID(N'[dbo].[income]'))
	ALTER TABLE [dbo].[income] DROP CONSTRAINT [FK_income_office_id]
GO

 /*
 *  3) Drop Tables
 */
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[office]') AND type in (N'U'))
	DROP TABLE [dbo].[office]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[income]') AND type in (N'U'))
	DROP TABLE [dbo].[income]
GO


 /*
 *  4) Create Tables
 */
 SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[office](
	[office_id] [int] IDENTITY(1,1) NOT NULL,
	[office] [nvarchar](100) NOT NULL,
	[updated_utc] [datetime2](7) NOT NULL
 CONSTRAINT [PK_office_id] PRIMARY KEY CLUSTERED 
(
	[office_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[income](
	[income_id] [int] IDENTITY(1,1) NOT NULL,
	[office_id] [int] NOT NULL,
	[year] [int] NOT NULL,
	[revenue] [money] NOT NULL,
	[profit] [money] NOT NULL,
	[updated_utc] [datetime2](7) NOT NULL
 CONSTRAINT [PK_income_id] PRIMARY KEY CLUSTERED 
(
	[income_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


 /*
 *  5) Create Foreign Keys
 */
ALTER TABLE [dbo].[income]  WITH CHECK ADD  CONSTRAINT [FK_income_office_id] FOREIGN KEY([office_id])
REFERENCES [dbo].[office] ([office_id])
GO

ALTER TABLE [dbo].[income] CHECK CONSTRAINT [FK_income_office_id]
GO

SET ANSI_PADDING OFF
GO

 /*
 *  6) Create Functions
 */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetYearlyIncomePerOffice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fnGetYearlyIncomePerOffice]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION dbo.fnGetYearlyIncomePerOffice 
(	
	@OfficeId	INT 
	,@Year		INT 
)
RETURNS @Result TABLE 
(
	Id			INT
	,Name		NVARCHAR(100)
	,Revenue	MONEY
	,Profit		MONEY
)
AS
BEGIN
        INSERT INTO  @Result 
			(Id, Name, Revenue, Profit)
		SELECT		i.office_id, o.office, i.revenue, i.profit
		FROM		dbo.income i
		INNER JOIN	dbo.office o
			ON		i.office_id = o.office_id
		WHERE		i.office_id = @OfficeId
		AND			i.[year] = @Year
        
        RETURN 
END

GO


 /*
 *  7) Create Stored Procedures
 */

IF OBJECT_ID('dbo.GetTotalRevenuePerOffice','P') IS NOT NULL
	DROP PROCEDURE dbo.GetTotalRevenuePerOffice
GO

CREATE PROCEDURE [dbo].[GetTotalRevenuePerOffice]
	@Office  [NVARCHAR](100)
AS
	SET NOCOUNT ON;

	DECLARE		@Revenue		MONEY
				,@TotalRevenue	MONEY

	SELECT		@Revenue = SUM(revenue)
	FROM		dbo.income i
	INNER JOIN	dbo.office o
		ON		i.office_id = o.office_id
	WHERE		o.office = @Office

	SELECT		@TotalRevenue = SUM(revenue)
	FROM		dbo.income i
	INNER JOIN	dbo.office o
		ON		i.office_id = o.office_id

	SELECT      office_id AS [Id] 
			    ,office AS [Name] 
				,(@Revenue * 100 / @TotalRevenue) AS [Percent]
				,@Revenue AS [Revenue]
				,@TotalRevenue - @Revenue AS [Others]
	FROM        dbo.office
	WHERE		office = @Office
GO


IF OBJECT_ID('dbo.GetYearlyProfitPerOffice','P') IS NOT NULL
	DROP PROCEDURE dbo.GetYearlyProfitPerOffice
GO

CREATE PROCEDURE [dbo].[GetYearlyProfitPerOffice]
	@Office  [NVARCHAR](100)
AS
	SET NOCOUNT ON;

	SELECT		profit AS [YearlyProfit]
	FROM		dbo.income i
	INNER JOIN	dbo.office o
		ON		i.office_id = o.office_id
	WHERE		o.office = @Office
	ORDER BY	i.[year]

GO

IF OBJECT_ID('dbo.GetYearlyRevenuePerOffice','P') IS NOT NULL
	DROP PROCEDURE dbo.GetYearlyRevenuePerOffice
GO

CREATE PROCEDURE [dbo].[GetYearlyRevenuePerOffice]
	@Office  [NVARCHAR](100)
AS
	SET NOCOUNT ON;

	SELECT		revenue AS [YearlyRevenue]
	FROM		dbo.income i
	INNER JOIN	dbo.office o
		ON		i.office_id = o.office_id
	WHERE		o.office = @Office
	ORDER BY	i.[year]

GO


IF OBJECT_ID('dbo.GetDashboardSetting','P') IS NOT NULL
	DROP PROCEDURE dbo.GetDashboardSetting
GO

CREATE PROCEDURE [dbo].[GetDashboardSetting]
AS
	SET NOCOUNT ON;

	EXEC [dbo].[GetTotalRevenuePerOffice] 'Dallas'
	EXEC [dbo].[GetYearlyProfitPerOffice] 'Dallas'
	EXEC [dbo].[GetYearlyRevenuePerOffice] 'Dallas'

	EXEC [dbo].[GetTotalRevenuePerOffice] 'Seattle'
	EXEC [dbo].[GetYearlyProfitPerOffice] 'Seattle'
	EXEC [dbo].[GetYearlyRevenuePerOffice] 'Seattle'
	
	EXEC [dbo].[GetTotalRevenuePerOffice] 'Boston'
	EXEC [dbo].[GetYearlyProfitPerOffice] 'Boston'
	EXEC [dbo].[GetYearlyRevenuePerOffice] 'Boston'
	
GO

IF OBJECT_ID('dbo.GetIncomePerOffice','P') IS NOT NULL
	DROP PROCEDURE dbo.GetIncomePerOffice
GO

CREATE PROCEDURE [dbo].[GetIncomePerOffice]
	@Id		INT
AS
	SET NOCOUNT ON;

	DECLARE @MyTable TABLE (
		Id				INT NOT NULL
		,Name			NVARCHAR(100) NOT NULL
		,Revenue2013	MONEY
		,Profit2013		MONEY
		,Revenue2014	MONEY
		,Profit2014		MONEY
		,Revenue2015	MONEY
		,Profit2015		MONEY
		,Revenue2016	MONEY
		,Profit2016		MONEY
		,Revenue2017	MONEY
		,Profit2017		MONEY 
	)
	
	/* 2013 */
	INSERT INTO @MyTable 
		(Id, Name, Revenue2013, Profit2013)
	SELECT * FROM [dbo].[fnGetYearlyIncomePerOffice] (@Id, 2013)

	/* 2014 */
	UPDATE		t
	SET			Revenue2014 = i.Revenue
				,Profit2014 = i.Profit
	FROM		@MyTable t
	INNER JOIN	[dbo].[fnGetYearlyIncomePerOffice] (@Id, 2014) AS i
		ON		t.id = i.id

	/* 2015 */
	UPDATE		t
	SET			Revenue2015 = i.Revenue
				,Profit2015 = i.Profit
	FROM		@MyTable t
	INNER JOIN	[dbo].[fnGetYearlyIncomePerOffice] (@Id, 2015) AS i
		ON		t.id = i.id

	/* 2016 */
	UPDATE		t
	SET			Revenue2016 = i.Revenue
				,Profit2016 = i.Profit
	FROM		@MyTable t
	INNER JOIN	[dbo].[fnGetYearlyIncomePerOffice] (@Id, 2016) AS i
		ON		t.id = i.id

	/* 2017 */
	UPDATE		t
	SET			Revenue2017 = i.Revenue
				,Profit2017 = i.Profit
	FROM		@MyTable t
	INNER JOIN	[dbo].[fnGetYearlyIncomePerOffice] (@Id, 2017) AS i
		ON		t.id = i.id

	SELECT * FROM @MyTable

GO

IF OBJECT_ID('dbo.UpdateYearlyIncomePerOffice','P') IS NOT NULL
	DROP PROCEDURE dbo.UpdateYearlyIncomePerOffice
GO

CREATE PROCEDURE [dbo].[UpdateYearlyIncomePerOffice]
	@OfficeId		INT
	,@Year			INT
	,@Revenue		MONEY
	,@Profit		MONEY
	,@UpdatedUTC	DATETIME2
AS
	SET NOCOUNT ON;

	UPDATE	dbo.income
	SET		revenue = @Revenue
			,profit = @Profit
			,updated_utc = @UpdatedUTC
	WHERE	office_id = @OfficeId
	AND		[year] = @Year

GO

IF OBJECT_ID('dbo.UpdateIncomePerOffice','P') IS NOT NULL
	DROP PROCEDURE dbo.UpdateIncomePerOffice
GO

CREATE PROCEDURE [dbo].[UpdateIncomePerOffice]
	@Id				INT
	,@Revenue2013	MONEY
	,@Profit2013	MONEY
	,@Revenue2014	MONEY
	,@Profit2014	MONEY
	,@Revenue2015	MONEY
	,@Profit2015	MONEY
	,@Revenue2016	MONEY
	,@Profit2016	MONEY
	,@Revenue2017	MONEY
	,@Profit2017	MONEY
AS
	SET NOCOUNT ON;

	DECLARE @Today AS DateTime2 = SYSUTCDATETIME()

	EXEC [dbo].[UpdateYearlyIncomePerOffice] @Id, 2013, @Revenue2013, @Profit2013, @Today

	EXEC [dbo].[UpdateYearlyIncomePerOffice] @Id, 2014, @Revenue2014, @Profit2014, @Today

	EXEC [dbo].[UpdateYearlyIncomePerOffice] @Id, 2015, @Revenue2015, @Profit2015, @Today

	EXEC [dbo].[UpdateYearlyIncomePerOffice] @Id, 2016, @Revenue2016, @Profit2016, @Today

	EXEC [dbo].[UpdateYearlyIncomePerOffice] @Id, 2017, @Revenue2017, @Profit2017, @Today

	SELECT COUNT(1)
	FROM	dbo.income
	WHERE	office_id = @Id
	AND		updated_utc = @Today

GO

 /*
 *  8) Populate Data
 */
 DECLARE @TodayUtc AS DATETIME2 = SYSUTCDATETIME()
		,@DallasId INT
		,@SeattleId INT
		,@BostonId INT

INSERT INTO dbo.office (office, updated_utc) VALUES ('Dallas', @TodayUtc)
SET @DallasId = SCOPE_IDENTITY()

INSERT INTO dbo.office (office, updated_utc) VALUES ('Seattle', @TodayUtc)
SET @SeattleId = SCOPE_IDENTITY()

INSERT INTO dbo.office (office, updated_utc) VALUES ('Boston', @TodayUtc)
SET @BostonId = SCOPE_IDENTITY()

/* Dallas Office */
INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@DallasId, 2013, 4500, 1500, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@DallasId, 2014, 3800, 3000, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@DallasId, 2015, 1500, 1000, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@DallasId, 2016, 1700, 1000, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@DallasId, 2017, 700, 300, @TodayUtc)

/* Seattle Office */
INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@SeattleId, 2013, 600, 200, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@SeattleId, 2014, 1600, 900, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@SeattleId, 2015, 3500, 500, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@SeattleId, 2016, 5000, 1000, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@SeattleId, 2017, 2800, 2000, @TodayUtc)

/* Boston Office */
INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@BostonId, 2013, 2000, 1500, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@BostonId, 2014, 2200, 170, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@BostonId, 2015, 2400, 190, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@BostonId, 2016, 2600, 2100, @TodayUtc)

INSERT INTO dbo.income(office_id, year, revenue, profit, updated_utc)
VALUES (@BostonId, 2017, 2800, 2300, @TodayUtc)




