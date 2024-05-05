using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Model;
using WarehouseProject.Repository;

namespace WarehouseProject.Controllers;

[Route("/api[controller]")]
[ApiController]
public class ProductController : ControllerBase
{

    private readonly IProductRepository _productRepository;


    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    [HttpPut]
    public async Task<IActionResult> AddProductToWarehouse(ProductWarehouse productWarehouse)
    {

        if (!await _productRepository.DoesProductExists(productWarehouse.IdProduct))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        if (!await _productRepository.DoesWarehouseExists(productWarehouse.IdWarehouse))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        if (!await _productRepository.HasOrderBeenRealized(productWarehouse.IdOrder))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        if (!await _productRepository.DoesProductExistsInTabelOrder(productWarehouse.IdProduct, productWarehouse.Amount))
        {
            return StatusCode(StatusCodes.Status418ImATeapot);
        }
        

        var result = await _productRepository.AddProductToWarehouseAndReturnID(productWarehouse);
        return Ok(result);
    }


    [HttpPut]
    [Route("Dodaj prze Proc")]
    public async Task<IActionResult> AddProductViaProcedure(int IdProduct , int IdWarehouse , int Amount , DateTime CreatedAt)
    {
        return Created("Wstawiono product do magazynu", _productRepository.AddProductWithProcedure(IdProduct,IdWarehouse,Amount,CreatedAt));
    }
    




}