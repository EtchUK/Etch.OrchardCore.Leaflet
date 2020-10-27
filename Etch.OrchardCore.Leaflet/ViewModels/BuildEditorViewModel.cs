namespace Etch.OrchardCore.Leaflet.ViewModels
{
    public class BuildEditorViewModel
    {
        public string ContentTypesName { get; set; }
        public dynamic EditorShape { get; set; }
        public bool FlowMetadata { get; set; }
        public string Id { get; set; } 
        public string ParentContentType { get; set; }
        public string PartName { get; set; }
        public string Prefix { get; set; }
        public string PrefixesName { get; set; }
        public string TargetId { get; set; }
    }
}
