namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for Part
    /// </summary>
    public class Part
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public int StockQty { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
