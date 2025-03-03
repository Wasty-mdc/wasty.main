using System.ComponentModel;

namespace wasty.Models
{
    public class Field : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Background { get; set; }
        public bool IsFilterable { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                UpdateFilteredOptions();
            }
        }

        public List<string> AllOptions { get; set; } = new List<string>();
        private List<string> _filteredOptions;
        public List<string> FilteredOptions
        {
            get => _filteredOptions;
            set
            {
                _filteredOptions = value;
                OnPropertyChanged(nameof(FilteredOptions));
            }
        }

        public Field(string name, string icon, string background, bool isFilterable = false, bool isExpanded = false, List<string> options = null)
        {
            Name = name;
            Icon = icon;
            Background = background;
            IsFilterable = isFilterable;
            IsExpanded = isExpanded;
            AllOptions = options ?? new List<string>();
            FilteredOptions = new List<string>(AllOptions);
        }

        private void UpdateFilteredOptions()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                FilteredOptions = new List<string>(AllOptions);
            else
                FilteredOptions = AllOptions.Where(o => o.Contains(FilterText, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
