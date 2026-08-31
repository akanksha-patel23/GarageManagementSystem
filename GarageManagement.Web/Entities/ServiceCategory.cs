namespace GarageManagement.Web.Entities
{
    /// <summary>
    /// Entity class for ServiceCategory
    /// </summary>
    public class ServiceCategory
    {
        //public int CategoryId { get; set; }
        //public string CategoryName { get; set; }
        //public string Description { get; set; }

        public int CategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string CategoryName { get; set; }
        public int Depth { get; set; }
    }
}
