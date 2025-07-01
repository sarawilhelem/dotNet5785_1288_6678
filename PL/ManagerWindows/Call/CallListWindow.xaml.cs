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

namespace PL.ManagerWindows.Call
{
    public partial class CallListWindow : Window, INotifyPropertyChanged
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private volatile DispatcherOperation? _observerOperation = null; //stage 7


        public IEnumerable<BO.CallInList> CallList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public BO.CallInList? SelectedCall { get; set; }

        public ICommand DeleteCallCommand { get; }
        public ICommand CancelAssignmentCommand { get; }

        public static readonly DependencyProperty CallListProperty =
           DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow));

        private string _selectedFilterField;
        public string SelectedFilterField
        {
            get => _selectedFilterField;
            set
            {
                _selectedFilterField = value;
                UpdateFilterValueControl();
            }
        }

        private string _selectedSortField;
        public string SelectedSortField
        {
            get => _selectedSortField;
            set
            {
                _selectedSortField = value;
                UpdateCallsList();
            }
        }

        private object _currentFilterValueControl;
        public object CurrentFilterValueControl
        {
            get => _currentFilterValueControl;
            set
            {
                _currentFilterValueControl = value;
                OnPropertyChanged(nameof(CurrentFilterValueControl)); // הוספת קריאה להודעה על שינוי
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CallListWindow()
        {
            InitializeComponent();
            DeleteCallCommand = new RelayCommand<BO.CallInList>(DeleteCall);
            CancelAssignmentCommand = new RelayCommand<BO.CallInList>(CancelAssignment);
            SelectedFilterField = nameof(FilterFields.CallId);
        }

        private void ListObserver()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    UpdateCallsList();
                });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Call.AddObserver(ListObserver);
            UpdateCallsList(); 
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Call.RemoveObserver(UpdateCallsList);
        }

        private void UpdateFilterValueControl()
        {
            if (string.IsNullOrEmpty(SelectedFilterField)) return;

            switch (SelectedFilterField)
            {
                case nameof(FilterFields.CallId):
                case nameof(FilterFields.LastVolunteerName):
                    var textBox = new TextBox();
                    textBox.Height = 20;
                    textBox.LostFocus += (s, e) => UpdateCallsList();
                    CurrentFilterValueControl = textBox;
                    break;
                case nameof(FilterFields.CallType):
                    var comboBox1 = new ComboBox();
                    comboBox1.SetBinding(ComboBox.ItemsSourceProperty, new Binding
                    {
                        Source = Application.Current.Resources["CallTypeCollectionKey"]
                    });
                    comboBox1.SelectedItem = null;
                    comboBox1.SelectionChanged += (s, e) => UpdateCallsList();
                    comboBox1.VerticalContentAlignment = VerticalAlignment.Center;
                    CurrentFilterValueControl = comboBox1;
                    break;

                case nameof(FilterFields.Status):
                    var comboBox2 = new ComboBox();
                    comboBox2.SetBinding(ComboBox.ItemsSourceProperty, new Binding
                    {
                        Source = Application.Current.Resources["FinishCallTypeCollectionKey"]
                    });
                    comboBox2.SelectedItem = null;
                    comboBox2.SelectionChanged += (s, e) => UpdateCallsList();
                    comboBox2.VerticalContentAlignment = VerticalAlignment.Center;
                    CurrentFilterValueControl = comboBox2;
                    break;

            }
        }

        private void UpdateCallsList()
        {
            BO.CallInListFields? sortField = GetSelectedSortField();
            BO.CallInListFields? filterField = GetSelectedFilterField();
            var filterValue = GetFilterValue();
            try
            {
                CallList = s_bl!.Call.ReadAll(filterField, filterValue, sortField);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Failed to load calls list", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private BO.CallInListFields? GetSelectedSortField()
        {
            if (string.IsNullOrEmpty(SelectedSortField))
            {
                return null; // Return null if no field is selected
            }

            // Try parsing only if SelectedFilterField has a valid value
            if (Enum.TryParse(typeof(BO.CallInListFields), SelectedSortField, out var result))
            {
                return (BO.CallInListFields?)result; // Safely return the parsed value
            }
            return null; // Replace with actual logic to return selected sort field
        }

        private BO.CallInListFields? GetSelectedFilterField()
        {
            if (string.IsNullOrEmpty(SelectedFilterField))
            {
                return null; // Return null if no field is selected
            }

            // Try parsing only if SelectedFilterField has a valid value
            if (Enum.TryParse(typeof(BO.CallInListFields), SelectedFilterField, out var result))
            {
                return (BO.CallInListFields?)result; // Safely return the parsed value
            }

            return null; // Return null if parsing fails
        }


        private object? GetFilterValue()
        {
            if (CurrentFilterValueControl is TextBox filterControl)
            {
                return filterControl.Text; // Text value from the TextBox
            }
            else if (CurrentFilterValueControl is ComboBox comboBox)
            {
                var selectedValue = comboBox.SelectedItem;

                // Check if the selected value is the "All" value for CallType or FinishCallType
                if (SelectedFilterField == nameof(FilterFields.CallType) &&
                    selectedValue is BO.CallType callType && callType == BO.CallType.All)
                {
                    return null; // Return null for "All" CallType
                }
                else if (SelectedFilterField == nameof(FilterFields.Status) &&
                    selectedValue is BO.FinishCallType finishCallType && finishCallType == BO.FinishCallType.All)
                {
                    return null; // Return null for "All" FinishCallType
                }

                return selectedValue; // Selected value from ComboBox
            }
            return null; // Default case if no control is set
        }
        private void AddCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new CallWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show call window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Call_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedCall == null)
            {
                MessageBox.Show("No call selected.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int id = SelectedCall.CallId;
            try
            {
                new CallWindow(id).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show call window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCall(BO.CallInList call)
        {
            try
            {
                if (call == null) throw new ArgumentNullException("No call was accepted for deletion");

                var result = MessageBox.Show("Are you sure you want to delete this item?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int id = call.CallId;
                    s_bl.Call.Delete(id);
                    UpdateCallsList(); // Refresh the call list after deletion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelAssignment(BO.CallInList call)
        {
            try
            {
                if (call is null) 
                    throw new ArgumentNullException("No call was accepted to cancel its assignment");

                var result = MessageBox.Show("Are you sure you want to cancel assinment for this call?", "Cancel assignment Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int userId = (int)Application.Current.Resources["UserIdKey"];
                    if (call.Id is null)
                    {
                        MessageBox.Show("This call does not have an assignment", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    s_bl.Call.CancelProcess(userId, call.Id.Value);
                    UpdateCallsList(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
