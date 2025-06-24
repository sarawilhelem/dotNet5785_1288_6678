using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PL.ManagerWindows.Volunteer
{
    /// <summary>
    /// Interaction logic for VolunteerListWindow.xaml
    /// </summary>
    public partial class VolunteerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private ObservableCollection<BO.VolunteerInList> _volunteerList = new ObservableCollection<BO.VolunteerInList>();
        public ObservableCollection<BO.VolunteerInList> VolunteerList
        {
            get { return _volunteerList; }
            set { _volunteerList = value; }
        }

        public BO.VolunteerInListFields SelectedSortField { get; set; } = BO.VolunteerInListFields.None;
        public BO.VolunteerInList? SelectedVolunteer { get; set; }
        public ICommand DeleteVolunteerCommand { get; }

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

        public VolunteerListWindow()
        {
            InitializeComponent();
            DeleteVolunteerCommand = new RelayCommand<BO.VolunteerInList>(DeleteVolunteer);
        }

        private void ChangeVolunteersListSortOrFilter(object sender, SelectionChangedEventArgs e)
        {
            UpdateVolunteersList();
        }

        private void UpdateVolunteersList()
        {
            VolunteerList.Clear(); // Clear the old items

            // Check if IsActiveFilter is null, true, or false
            var volunteers = (SelectedSortField == BO.VolunteerInListFields.None) ?
                s_bl?.Volunteer.ReadAll(IsActiveFilter) :
                s_bl?.Volunteer.ReadAll(IsActiveFilter, SelectedSortField)!;

            foreach (var volunteer in volunteers!)
            {
                VolunteerList.Add(volunteer); // Add new items
            }
        }

        private void VolunteerListObserver()
        {
            UpdateVolunteersList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(VolunteerListObserver);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(VolunteerListObserver);
        }

        private void AddVolunteer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new VolunteerWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show volunteer window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Volunteer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int id = SelectedVolunteer?.Id 
                ?? throw new ArgumentNullException("No volunteer was accepted");
            try
            {
                new VolunteerWindow(id).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show volunteer window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteVolunteer(BO.VolunteerInList volunteer)
        {
            try
            {
                if (volunteer == null)
                    throw new ArgumentNullException("No volunteer was accepted for deletion");

                var result = MessageBox.Show("Are you sure you want to delete the volunteer?", "Deleting Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int id = volunteer.Id;
                    s_bl.Volunteer.Delete(id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}