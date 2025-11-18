using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopppingWithSP.Migrations
{
    /// <inheritdoc />
    public partial class Addsp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'OrderDetailsType')
BEGIN
    CREATE TYPE dbo.OrderDetailsType AS TABLE
    (
        Item NVARCHAR(100),
        Size NVARCHAR(50),
        Price DECIMAL(18,2),
        Description NVARCHAR(250),
ImageUrl NVARCHAR(max)
    );
END
");
            migrationBuilder.Sql(@"
        CREATE OR ALTER PROCEDURE dbo.sp_AddOrderWithDetails
            @CustomerName     NVARCHAR(100),
            @CustomerContact  NVARCHAR(20),
            @CustomerEmail    NVARCHAR(100),
            @CustomerAddress  NVARCHAR(200),
            @OrderStatus      INT,
            @OrderDetails     dbo.OrderDetailsType READONLY
        AS
        BEGIN
            SET NOCOUNT ON;
            BEGIN TRY
                BEGIN TRANSACTION;
                INSERT INTO [Order]
                    (CustomerName, CustomerContact, CustomerEmail, CustomerAddress, OrderDate, OrderStatus)
                VALUES
                    (@CustomerName, @CustomerContact, @CustomerEmail, @CustomerAddress, GETDATE(), @OrderStatus);

                DECLARE @NewOrderId INT = SCOPE_IDENTITY();

                INSERT INTO OrderDetails (OrderId, Item, Size, Price, Description, ImageUrl)
                SELECT @NewOrderId, Item, Size, Price, Description, ImageUrl
                FROM @OrderDetails;

                COMMIT TRANSACTION;
            END TRY
            BEGIN CATCH
                IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
                THROW;
            END CATCH
        END;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS dbo.sp_AddOrderWithDetails;");
            migrationBuilder.Sql(@"DROP TYPE IF EXISTS dbo.OrderDetailsType;");
        }
    }
}
