namespace BgB_TeachingAssistant.ViewModels
{
    public class PageDescriptor : IPageDescriptor
    {
        public string Name { get; set; }
        public Type ViewModelType { get; set; }
        public string Icon { get; set; }  // Optional, can be a path to an icon or other descriptor
    }
}