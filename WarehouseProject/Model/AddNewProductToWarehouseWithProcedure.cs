namespace WarehouseProject.Model;

public class AddNewProductToWarehouseWithProcedure
{
    public int IdWarehouse { get; set; }
    public int IdProduct { get; set; }
    public int IdOrder { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ProcedureProduct> ProcedureProducts { get; set; } = new List<ProcedureProduct>();

}


public class ProcedureProduct
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}