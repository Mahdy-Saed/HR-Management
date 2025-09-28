using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.RequestDtos
{
    public class RequestResponceDto
    {


           public int? Id { get; set; }
            public string? Title { get; set; }

            public RequestType? Type { get; set; }

            public string? Description { get; set; }

            public RequestStatus? Status { get; set; }= RequestStatus.Pending;

            public DateTime? Request_Date { get; set; } = DateTime.Now;


        
        public enum RequestType
        {
            Leave,
            Resignation,
            Promotion,
            Transfer,
            Other
        }



        public enum RequestStatus
        {
            Approved,
            Pending,
            Rejected

        }
    




}
}
