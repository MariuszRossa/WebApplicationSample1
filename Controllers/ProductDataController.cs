using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplicationSample1.Models;
using WebApplicationSample1.Models.ProdecureModels;


namespace WebApplicationSample1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductDataController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductDataController(ProductDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetProductsData")]
        public List<Sample1_Get_Product_Data_Results> Get()
        {
            return _context.Product_Data_Results
                .FromSqlRaw("[SAMPLE1_GET_PRODUCT_DATA] ")
                .ToList();
        }

        [HttpGet("GetProductData/{id}")]
        public ActionResult<Sample1_Get_Product_Data_Results> Get(int id)
        {
            Sample1_Get_Product_Data_Results? result = _context.Product_Data_Results
                .FromSqlRaw("[SAMPLE1_GET_PRODUCT_DATA] ")
                .AsEnumerable()
                .FirstOrDefault(p => p.ProductID == id);

            if (result != null)
            {
                return result;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("InsertProductData")]
        public ActionResult PutProduct([FromBody] Sample1_Insert_Product_Data_Result putData)
        {
            try
            {
                _context.Database.ExecuteSqlRaw
                (
                        "[SAMPLE1_INSERT_PRODUCT_DATA] {0},{1},{2},{3},{4},{5},{6}"
                     , putData.Name ?? ""
                     , putData.ProductNumber ?? ""
                     , putData.Color ?? ""
                     , putData.SafetyStockLevel != 0 ? putData.SafetyStockLevel : 1
                     , putData.ReorderPoint
                     , putData.ListPrice
                     , putData.Size ?? ""
                );
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut("DisableProduct/{id}")]
        public ActionResult PutDisableProduct(int id)
        {
            try
            {
                int result = _context.Database.ExecuteSqlRaw
                (
                     "[SAMPLE1_UPDATE_DISABLE_PRODUCT_SELLS] {0}"
                     , id
                );
                if (result > 0)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch(SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update_Price")]
        public ActionResult<Sample1_Update_Product_Price_Results> UpdateProductPrice([FromBody] Sample1_Update_Product_Price_Results putData) 
        {
            try
            {
                int result = _context.Database.ExecuteSqlRaw
                (
                     "[SAMPLE1_UPDATE_PRODUCT_PRICE] {0}, {1}"
                     , putData.ProductiD
                     , putData.ListPrice
                );
                if (result > 0)
                {
                    return Ok();
                }

                return NotFound("Product not found");
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete_Product/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var data = _context.Products
                    .Include(e => e.ProductListPriceHistories)
                    .ToList().Where(p => p.ProductId == id);

                if (data.Count() > 0)
                {
                    _context.Products.RemoveRange(data);
                    _context.SaveChanges();

                    return Ok();
                }

                return NotFound("Product not found");

            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}