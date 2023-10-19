--USE AdventureWorks2017 DATABASE
CREATE PROCEDURE SAMPLE1_INSERT_PRODUCT_DATA 
(
	@Name NVARCHAR(512)
	,@ProductNumber NVARCHAR(512)
	,@Color VARCHAR(128)
	,@SafetyStockLevel SMALLINT
	,@ReorderPoint SMALLINT
	,@ListPrice MONEY
	,@Size NVARCHAR(5)
)
AS
BEGIN

BEGIN TRY  
	BEGIN TRANSACTION Insert_Product
		DECLARE @DATE DATETIME= GETDATE()
		DECLARE @ProductiDTable TABLE (ID INT)
		DECLARE @ProductiD INT

		--INSERT PRODUCT
		INSERT INTO [AdventureWorks2017].[Production].[Product] 
		(
			[Name]
			,[ProductNumber]
			,[Color]
			,[SafetyStockLevel]
			,[ReorderPoint]
			,[ListPrice]
			,[Size]
			,[SellStartDate]
			,[ModifiedDate]
			,[StandardCost]
			,[DaysToManufacture]
		) 
		OUTPUT INSERTED.ProductID
			INTO @ProductiDTable
		VALUES 
		(
			@Name
			,@ProductNumber
			,@Color
			,@SafetyStockLevel
			,@ReorderPoint
			,@ListPrice
			,@Size
			,@DATE
			,@DATE
			,0
			,0
		)
		SELECT @ProductiD = (SELECT TOP 1 ID FROM @ProductiDTable)

		--CHECK AND UPDATE LAST HISTORY DATA - USEFULL IN PRODUCT PRICE UPDATE
		IF 
		(
			EXISTS 
			(
				SELECT 1 
				FROM [AdventureWorks2017].[Production].[ProductListPriceHistory] 
				WHERE ProductID = @ProductiD AND EndDate IS NULL 
			)
		)
		BEGIN 
			UPDATE [AdventureWorks2017].[Production].[ProductListPriceHistory] 
			SET EndDate = @DATE
			WHERE ProductID = @ProductiD AND EndDate IS NULL 
		END

		--INSERT PRICE HISTORY
		INSERT INTO [AdventureWorks2017].[Production].[ProductListPriceHistory] 
		(
			[ProductID]
			,[StartDate]
			,[ListPrice]
			,[ModifiedDate]
		)
		VALUES
		(
			@ProductiD
			,@DATE
			,@ListPrice
			,@DATE
		)

	COMMIT TRANSACTION Insert_Product 
END TRY  

BEGIN CATCH  
	IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION 
			
			DECLARE @ERROR NVARCHAR(1024) = (SELECT ERROR_MESSAGE())
			RAISERROR (@ERROR, 16, 1)
		END
	END CATCH  
END