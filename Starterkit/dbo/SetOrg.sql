USE [queentouch_db]
GO

/****** Object:  StoredProcedure [dbo].[Set_Org]    Script Date: 03-01-2024 17:19:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE      PROCEDURE [dbo].[Set_Org]
 @id INT
,@name VARCHAR(150)    
,@contactName VARCHAR(150)    
,@email VARCHAR(100)
,@mobile VARCHAR(50)
,@address VARCHAR(200)
,@code VARCHAR(10)
,@website varchar(255)
,@mid int
,@tokenid varchar(255)
,@dashboard_url varchar(255)
AS
BEGIN
SET NOCOUNT ON;

SET @id=LTRIM(RTRIM(ISNULL(@id,0)))
SET @name=LTRIM(RTRIM(ISNULL(@name,'')))    
SET @contactName=LTRIM(RTRIM(ISNULL(@contactName,'')))    
SET @email=LTRIM(RTRIM(ISNULL(@email,'')))    
SET @mobile=LTRIM(RTRIM(ISNULL(@mobile,'')))    
SET @address=LTRIM(RTRIM(ISNULL(@address,'')))
SET @code=LTRIM(RTRIM(ISNULL(@code,'')))
SET @website=LTRIM(RTRIM(ISNULL(@website,'')))
SET @mid=LTRIM(RTRIM(ISNULL(@mid,'')))
SET @tokenid=LTRIM(RTRIM(ISNULL(@tokenid,'')))
SET @dashboard_url=LTRIM(RTRIM(ISNULL(@dashboard_url,'')))

DECLARE @response TABLE (response VARCHAR(100), id INT)    

DECLARE @resp VARCHAR(100)    
DECLARE @output VARCHAR(100)     
    
EXEC Validate_Org @id, @name, @code, @mobile,@tokenid, @output OUTPUT

IF @output='OK'    
BEGIN    
	IF @id=-1    
	BEGIN   
		INSERT INTO organizations (name, contactName, email, mobile, address, code, isActive, createdAt, org_website,org_wallet,org_pass,mstr_id,token_id,dashboard_url)
		VALUES (@name, @contactName, @email, @mobile, @address, @code, 1, GETDATE() ,@website ,  0.00,'password',@mid,@tokenid ,@dashboard_url)

		SET @id = SCOPE_IDENTITY()
		SET @resp = 'OK'
	END
	ELSE
	BEGIN
		UPDATE organizations
		SET name=@name, contactName=@contactName, email=@email, mobile=@mobile, address=@address, code=@code, updatedAt=GETDATE()
		WHERE id=@id

		SET @Resp='OK'
	END
END
ELSE
BEGIN
	SET @id=0    
	SET @resp=@output
END
INSERT INTO @response SELECT @resp,@id    
SELECT response,id FROM @response   

END
GO


