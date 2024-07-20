using JobBoard.Model.Attributes;

namespace JobBoard.Model
{
    public class PagedRequest
    {
        [SkipFilter]
        public int? PageIndex { get; set; }
        [SkipFilter]
        public int? PageSize { get; set; }
        [SkipFilter]
        public bool? SkipPagination { get; set; }
    }
}
