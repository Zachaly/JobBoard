namespace JobBoard.Domain.Entity
{
    public class EmployeeResume : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public long EmployeeId { get; set; }
        public EmployeeAccount Employee { get; set; }
    }
}
