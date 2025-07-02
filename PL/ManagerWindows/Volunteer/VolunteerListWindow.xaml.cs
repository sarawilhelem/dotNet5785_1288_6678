using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PL.ManagerWindows.Volunteer
{
    /// <summary>
    /// Interaction logic for VolunteerListWindow.xaml
    /// </summary>
    public partial class VolunteerListWindow : Window, INotifyPropertyChanged
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public BO.VolunteerInList? SelectedVolunteer { get; set; }

        public ICommand DeleteVolunteerCommand { get; }

        public static readonly DependencyProperty VolunteerListProperty =
           DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow));

        private string _selectedSortField;
        public string SelectedSortField
        {
            get => _selectedSortField;
            set
            {
                _selectedSortField = value;
                UpdateVolunteersList();
            }
        }

        private bool? _isActiveFilter;
        public bool? IsActiveFilter
        {
            get => _isActiveFilter;
            set
            {
                _isActiveFilter = value;
                UpdateVolunteersList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VolunteerListWindow()
        {
            InitializeComponent();
            DeleteVolunteerCommand = new RelayCommand<BO.VolunteerInList>(DeleteVolunteer);
        }

        /// <summary>
        /// Updates the list of volunteers based on the selected sort field and active status filter.
        /// </summary>
        private void UpdateVolunteersList()
        {
            BO.VolunteerInListFields? sortField = GetSelectedSortField();
            try
            {
                VolunteerList = s_bl!.Volunteer.ReadAll( IsActiveFilter, sortField);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to load volunteers list", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Retrieves the selected sort field as an enum value.
        /// </summary>
        /// <returns>The selected sort field or null if not valid.</returns>
        private BO.VolunteerInListFields? GetSelectedSortField()
        {
            if (string.IsNullOrEmpty(SelectedSortField))
            {
                return null; // Return null if no field is selected
            }

            if (Enum.TryParse(typeof(BO.VolunteerInListFields), SelectedSortField, out var result))
            {
                return (BO.VolunteerInListFields?)result; // Safely return the parsed value
            }
            return null; // Return null if parsing fails
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateVolunteersList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Perform any cleanup if necessary
        }

        private void Volunteer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedVolunteer == null)
            {
                MessageBox.Show("No volunteer selected.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int id = SelectedVolunteer.Id;
            try
            {
                new VolunteerWindow(id).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show volunteer window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Deletes the selected volunteer after confirmation.
        /// </summary>
        /// <param name="volunteer">The volunteer to delete.</param>
        private void DeleteVolunteer(BO.VolunteerInList volunteer)
        {
            try
            {
                if (volunteer == null) throw new ArgumentNullException("No volunteer was accepted for deletion");

                var result = MessageBox.Show("Are you sure you want to delete this item?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int id = volunteer.Id;
                    s_bl.Volunteer.Delete(id);
                    UpdateVolunteersList(); // Refresh the volunteer list after deletion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeVolunteersListSortOrFilter(object sender, SelectionChangedEventArgs e)
        {
            UpdateVolunteersList();
        }
    }
}
