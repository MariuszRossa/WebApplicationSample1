using System.ComponentModel;
using System.Drawing;

namespace WebApplicationSample1.Models.ProdecureModels
{
    [DisplayName("Insert_Product")]
    public class Sample1_Insert_Product_Data_Result
    {
        public string? Name { get; set; }
        public string? ProductNumber { get; set; }
        public string? Color { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
    }
}