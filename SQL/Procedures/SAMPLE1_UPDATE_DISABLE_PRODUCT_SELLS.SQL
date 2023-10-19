--USE AdventureWorks2017 DATABAS
CREATE PROCEDURE SAMPLE1_UPDATE_DISABLE_PRODUCT_SELLS
(
	@ProductiD INT
)
AS
BEGIN
	IF
	(
		EXISTS
		(
			SELECT 1 
			FROM [AdventureWorks2017].[Production].[Product] 
			WHERE ProductID = @ProductiD
		)
	)
	BEGIN 
		DECLARE @DATE DATETIME = GETDATE()

		UPDATE [AdventureWorks2017].[Production].[Product] 
		SET SellEndDate = @DATE
		WHERE ProductID = @ProductiD
	END
END