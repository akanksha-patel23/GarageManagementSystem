namespace GarageManagement.Api.DTOs
{
    public class JobCardStatusHistoryDto
    {
        public int HistoryID { get; set; }
        public int JobCardID { get; set; }
        public string Status { get; set; }
        public DateTime ChangedDate { get; set; }
        public string Remarks { get; set; }
    }
}
