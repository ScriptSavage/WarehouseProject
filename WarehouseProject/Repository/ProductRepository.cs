using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseProject.Model;

namespace WarehouseProject.Repository;

public class ProductRepository : IProductRepository
{

    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<IEnumerable<Product>> GetAllProducts()
    {

        var query = "SELECT * FROM Product";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;

       
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        var produkty = new List<Product>();
        
        var IdProductOrdnial = reader.GetOrdinal("IdProduct");


        while (await reader.ReadAsync())
        {
            var grade = new Product()
            {
                IdProduct = reader.GetInt32(IdProductOrdnial)
            };
            produkty.Add(grade);
        }

        return produkty;

    }

    public  async Task<bool> DoesProductExists(int id)
    {
        var query = "SELECT 1 FROM Product WHERE IdProduct = @ID";

       await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
       await using SqlCommand command = new SqlCommand();

       command.Connection = connection;
       command.CommandText = query;
       command.Parameters.AddWithValue("@ID", id);
       await connection.OpenAsync();

       var res = await command.ExecuteScalarAsync();

       return res is not null;


    }

    public  async Task<bool> DoesWarehouseExists(int id)
    {
        var query = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("ID", id);
        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }
    
    public async Task<bool> DoesProductExistsInTabelOrder(int IdProduct, int Amount)
    {
        var qurey = "SELECT 1 FROM \"Order\" WHERE IdOrder = @IdProduct AND  Amount = @Amount";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = qurey;
        await connection.OpenAsync();
        command.Parameters.AddWithValue("@IdProduct", IdProduct);
        command.Parameters.AddWithValue("@Amount", Amount);

        var result = await command.ExecuteScalarAsync();

        return result is not null;
    }
    
    public async Task<bool> HasOrderBeenRealized(int idOrder)
    {
        
        var query = "SELECT 1 FROM Product_Warehouse Where IdProduct = @IdProduct";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdProduct", idOrder);
        await connection.OpenAsync();
        
        var res =  await command.ExecuteScalarAsync();

        return res is not null;
    }

   

    public async Task<int> AddProductToWarehouseAndReturnID(ProductWarehouse productWarehouse)
    {
        var query = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                    "VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Amount*@Price, @CreatedAt); " +
                    "SELECT CAST(@@IDENTITY AS INT) AS IdProductWarehouse; UPDATE \"Order\" SET FulfilledAt = @Data WHERE IdOrder = @IdOrder;";
        
        
        //var update = "UPDATE \"Order\" SET FulfilledAt = @Data WHERE IdOrder = @IdOrderV2;";


        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        //command.CommandText = update;
        command.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        command.Parameters.AddWithValue("IdOrder", productWarehouse.IdOrder);
        command.Parameters.AddWithValue("Amount", productWarehouse.Amount);
        command.Parameters.AddWithValue("@Price", productWarehouse.Price);
        command.Parameters.AddWithValue("@CreatedAt",productWarehouse.CreatedAt);
        command.Parameters.AddWithValue("@Data", productWarehouse.CreatedAt);
       // command.Parameters.AddWithValue("@IdOrderV2", productWarehouse.IdOrder);
        
        await connection.OpenAsync();
        
        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task AddProductWithProcedure(int IdProduct , int IdWarehouse , int Amount , DateTime CreatedAt)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand("AddProductToWarehouse", connection);
        command.Connection = connection;
        await connection.OpenAsync();
        command.CommandType = CommandType.StoredProcedure;
        
        command.Parameters.AddWithValue("IdProduct", IdProduct);
        command.Parameters.AddWithValue("Amount", Amount);
        command.Parameters.AddWithValue("IdWarehouse", IdWarehouse);
        command.Parameters.AddWithValue("CreatedAt", CreatedAt);
        
    }
}