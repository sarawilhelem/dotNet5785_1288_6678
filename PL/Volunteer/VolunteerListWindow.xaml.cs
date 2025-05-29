using BO;
using PL.Call;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PL.Volunteer
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
        public BO.VolunteerInList? SelectedVolunteer
        {
            get;
            set;
        }
        public ICommand DeleteVolunteerCommand { get; }


        public VolunteerListWindow()
        {
            InitializeComponent();
            DeleteVolunteerCommand = new RelayCommand<BO.VolunteerInList>(DeleteVolunteer);

        }

        /// <summary>
        /// Handles the selection change event to update the volunteer list based on the selected sort field.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>
        private void ChangeVolunteersListSort(object sender, SelectionChangedEventArgs e)
        {
            UpdateVolunteersList();
        }

        /// <summary>
        /// Update the volunteer list from bl
        /// </summary>
        private void UpdateVolunteersList()
        {
            VolunteerList.Clear(); // Clear the old items
            var volunteers = (SelectedSortField == BO.VolunteerInListFields.None) ?
                s_bl?.Volunteer.ReadAll()! :
                s_bl?.Volunteer.ReadAll(null, SelectedSortField)!;

            foreach (var volunteer in volunteers)
            {
                VolunteerList.Add(volunteer); // Add new items
            }
        }

        /// <summary>
        /// update the volunteer list
        /// </summary>
        private void VolunteerListObserver()
        {
            UpdateVolunteersList();
        }

        /// <summary>
        /// add the volunteerListObserver to the observers list
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(VolunteerListObserver);
        }

        /// <summary>
        /// remove the volunteerListObserver from the observers list
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(VolunteerListObserver);
        }

        private void AddVolunteer_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerWindow().Show();
        }

        private void Volunteer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            int id = SelectedVolunteer?.Id
        ?? throw new ArgumentNullException("No volunteer was accepted for deletion");
            new VolunteerWindow(id).Show();

        }
        private void DeleteVolunteer(BO.VolunteerInList volunteer)
        {
            try
            {
                if (volunteer == null)
                    throw new ArgumentNullException("No volunteer was accepted for deletion");

                var result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את המתנדב?", "אישור מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question);

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