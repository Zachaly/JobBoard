namespace JobBoard.Model.Business
{
    public class UpdateBusinessRequest : IUpdateRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
