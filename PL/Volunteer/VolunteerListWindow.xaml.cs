using BO;
using PL.Call;
using System;
using System.Collections.Generic;
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

        public IEnumerable<BO.VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerInList));

        public BO.VolunteerInListFields SelectedSortField { get; set; } = BO.VolunteerInListFields.None;
        public BO.CallInList? SelectedVolunteer
        {
            get;
            set;
        }

        public VolunteerListWindow()
        {
            InitializeComponent();
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
            VolunteerList = (SelectedSortField == BO.VolunteerInListFields.None) ?
                s_bl?.Volunteer.ReadAll()! :
                s_bl?.Volunteer.ReadAll(null,SelectedSortField)!;
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
            if (sender is ListView listView)
            {
                if (SelectedVolunteer != null)
                {
                    int? id = (listView.SelectedItem as BO.VolunteerInList)?.Id;
                    new VolunteerWindow(id ?? 0).Show();
                }

            }
        }
        public ICommand DeleteCommand => new RelayCommand<BO.CallInList>(DeleteVolunteer);

        private void DeleteVolunteer(BO.VolunteerInList item)
        {
            var result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את המתנדב?", "אישור מחיקה", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Volunteer.Delete((int)item.Id);
                }
                catch
                {
                    var failedErase = MessageBox.Show("המחיקה נכשלה");
                }
            }
        }
    }
}
