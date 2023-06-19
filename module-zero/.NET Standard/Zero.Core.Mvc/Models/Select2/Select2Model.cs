using Newtonsoft.Json;

namespace Zero.Core.Mvc.Models.Select2
{
    public class Select2Model
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "selected")]
        public bool Selected { get; set; }

        [JsonProperty(PropertyName = "disabled")]
        public bool Disabled { get; set; }
    }
}