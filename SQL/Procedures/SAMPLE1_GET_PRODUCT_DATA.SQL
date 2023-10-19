--USE AdventureWorks2017 DATABASE
CREATE PROCEDURE SAMPLE1_GET_PRODUCT_DATA
AS
BEGIN
    SELECT 
	    PR.[ProductID]
        ,[Name]
        ,[ProductNumber]
        ,ISNULL([Color], '') [Color]
        ,[SafetyStockLevel]
        ,[ReorderPoint]
        ,[ListPrice]
        ,ISNULL([Size], '') [Size]
        ,[SellStartDate]
    FROM 
        [AdventureWorks2017].[Production].[Product] PR
        LEFT JOIN [AdventureWorks2017].[Production].[ProductProductPhoto] PP_ID ON PP_ID.[ProductID] = PR.ProductID
        LEFT JOIN [AdventureWorks2017].[Production].[ProductPhoto] PP ON PP_ID.ProductPhotoID = PP.ProductPhotoID
	WHERE 
		(
			PR.SellEndDate IS NULL OR
			PR.SellEndDate <= GETDATE()
		)
END