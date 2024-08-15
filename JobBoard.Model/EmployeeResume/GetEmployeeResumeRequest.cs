namespace JobBoard.Model.EmployeeResume
{
    public class GetEmployeeResumeRequest : PagedRequest
    {
        public long? EmployeeId { get; set; }
    }
}
