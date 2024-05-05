using WarehouseProject.Model;

namespace WarehouseProject.Repository;

public interface IProductRepository
{


    
    Task<IEnumerable<Product>> GetAllProducts();
  

    Task<bool> DoesProductExists(int id);
    
    Task<bool> DoesWarehouseExists(int id);
    

    Task<bool> DoesProductExistsInTabelOrder(int IdProduct, int Amount);
    
   

    Task<bool> HasOrderBeenRealized(int idOrder);
    


      Task<int> AddProductToWarehouseAndReturnID(ProductWarehouse productWarehouse);


      Task AddProductWithProcedure(int IdProduct , int IdWarehouse , int Amount , DateTime CreatedAt);



}