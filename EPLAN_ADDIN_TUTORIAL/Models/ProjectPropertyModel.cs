namespace EPLAN_API_TUTORIAL.Models
{
    public class ProjectPropertyModel
    {
        public int PropertyId { get; set; }
        public int? Index { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        public string DisplayId => Index.HasValue ? $"<{PropertyId} {Index}>" : $"<{PropertyId}>";
    }
}
