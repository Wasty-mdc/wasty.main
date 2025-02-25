namespace wasty.Models
{
    public class Field
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Background { get; set; }
        public bool IsFilterable { get; set; }

        public Field(string name, string icon, string background, bool isFilterable = false)
        {
            Name = name;
            Icon = icon;
            Background = background;
            IsFilterable = isFilterable;
        }
    }
}
