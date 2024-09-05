using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Module02Exercise01.Model;
using Microsoft.Maui.Graphics;

namespace Module02Exercise01.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _allEmployees;
        private ObservableCollection<Employee> _displayedEmployees;
        private bool _isShowingManagers;
        private string _searchText;

        public ObservableCollection<Employee> DisplayedEmployees
        {
            get => _displayedEmployees;
            set
            {
                if (_displayedEmployees != value)
                {
                    _displayedEmployees = value;
                    OnPropertyChanged(nameof(DisplayedEmployees));
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterEmployees();
                }
            }
        }

        public ICommand DisplayManagerCommand { get; set; }

        public string ButtonText => _isShowingManagers ? "Show All Employees" : "Show Managers";

        public Color ButtonColor => _isShowingManagers ? Colors.Red : Colors.Green;

        public string ImageSource => _isShowingManagers ? "manager.png" : "employee.png";

        public string HeadingText => _isShowingManagers ? "Managers" : "Employees";

        public EmployeeViewModel()
        {
            _allEmployees = new ObservableCollection<Employee>
            {
                new Employee { FirstName = "Jerico", LastName = "Daag", Position = "Developer", Department = "IT", IsActive = true, ImagePath = "john.png" },
                new Employee { FirstName = "Elmalia", LastName = "Diaz", Position = "Manager", Department = "HR", IsActive = true, ImagePath = "jane.png" },
                new Employee { FirstName = "Joshua", LastName = "Daguio", Position = "Developer", Department = "IT", IsActive = true, ImagePath = "robert.png" },
                new Employee { FirstName = "Anne", LastName = "Canlas", Position = "Manager", Department = "Finance", IsActive = true, ImagePath = "emily.png" },
                new Employee { FirstName = "Paul", LastName = "Yumang", Position = "Analyst", Department = "Marketing", IsActive = true, ImagePath = "michael.png" }
            };

            DisplayedEmployees = new ObservableCollection<Employee>(_allEmployees);
            _isShowingManagers = false;

            DisplayManagerCommand = new Command(DisplayManager);
        }

        private void FilterEmployees()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                DisplayedEmployees = new ObservableCollection<Employee>(_allEmployees);
            }
            else
            {
                var filteredEmployees = _allEmployees
                    .Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(SearchText.ToLower()))
                    .ToList();
                DisplayedEmployees = new ObservableCollection<Employee>(filteredEmployees);
            }
        }

        private void DisplayManager()
        {
            if (_isShowingManagers)
            {
                // Show all employees
                DisplayedEmployees = new ObservableCollection<Employee>(_allEmployees);
            }
            else
            {
                // Show only managers
                var managers = _allEmployees.Where(e => e.Position == "Manager").ToList();
                DisplayedEmployees = new ObservableCollection<Employee>(managers);
            }
            _isShowingManagers = !_isShowingManagers;
            OnPropertyChanged(nameof(ButtonText));
            OnPropertyChanged(nameof(ButtonColor));
            OnPropertyChanged(nameof(ImageSource));
            OnPropertyChanged(nameof(HeadingText));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}