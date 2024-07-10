using JobBoard.Model.Enum;

namespace JobBoard.Model.Attributes
{
    public class CustomFilterAttribute : Attribute
    {
        public string? Property { get; set; } = null;
        public string? SubProperty { get; set; } = null;
        public ComparisonType ComparisonType { get; set; } = ComparisonType.Equal;
    }
}
