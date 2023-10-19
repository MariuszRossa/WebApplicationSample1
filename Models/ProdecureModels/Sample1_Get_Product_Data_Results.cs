using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplicationSample1.Models.ProdecureModels
{
    [DisplayName("Get_Product_Data")]
    public class Sample1_Get_Product_Data_Results
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public string? ProductNumber { get; set; }
        public string? Color { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public DateTime SellStartDate { get; set; }
    }
}
