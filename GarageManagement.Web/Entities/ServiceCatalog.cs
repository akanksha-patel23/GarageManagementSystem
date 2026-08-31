namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for ServiceCatalogItem
    /// </summary>
    public class ServiceCatalog
    {
        public int ServiceID { get; set; }
        public int CategoryID { get; set; }
        public string ServiceName { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? EstimatedHours { get; set; }

        // Not a DB column — populated by ServiceCatalogBAL from
        // ServiceCategoryBAL's tree for display convenience, avoiding a new SP/join purely for a grid column.
        public string CategoryName { get; set; }
    }
}
