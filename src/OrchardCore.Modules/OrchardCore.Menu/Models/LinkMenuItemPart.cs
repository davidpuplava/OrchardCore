using OrchardCore.ContentManagement;

namespace OrchardCore.Menu.Models
{
    public class LinkMenuItemPart : ContentPart
    {
        /// <summary>
        /// The url of the link to create.
        /// </summary>
        public string Url { get; set; }
    }
}
