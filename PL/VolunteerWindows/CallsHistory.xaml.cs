using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace PL.VolunteerWindows
{
    /// <summary>
    /// Interaction logic for CallsHistory.xaml
    /// </summary>
    public partial class CallsHistory : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        static int volunteerId;

        public IEnumerable<BO.ClosedCallInList> CallList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        private BO.ClosedCallInList? _selectedCall;
        public BO.ClosedCallInList? SelectedCall
        {
            get => _selectedCall;
            set
            {
                if (_selectedCall != value)
                {
                    _selectedCall = value;
                    OnPropertyChanged(nameof(SelectedCall));
                }
            }
        }

        public static readonly DependencyProperty CallListProperty =
          DependencyProperty.Register(
               "CallList",
               typeof(IEnumerable<BO.ClosedCallInList>),
               typeof(CallsHistory)
          );

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

        private string _selectedCallType;
        public string SelectedCallType
        {
            get => _selectedCallType;
            set
            {
                _selectedCallType = value;
                UpdateCallsList();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CallsHistory()
        {
            InitializeComponent();
            volunteerId = (int)Application.Current.Resources["UserIdKey"];

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Call.AddObserver(UpdateCallsList);
            UpdateCallsList(); // Load initial call list
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Call.RemoveObserver(UpdateCallsList);
        }
        private void UpdateCallsList()
        {
            var selectedId = SelectedCall?.Id;
            BO.ClosedCallInListFields? sortField = GetSelectedSortField();
            BO.CallType? callType = GetSelectedCallType();
            try
            {
                CallList = s_bl!.Call.ReadAllVolunteerClosedCalls(volunteerId, callType, sortField);

                if (selectedId != null)
                    SelectedCall = CallList?.FirstOrDefault(c => c.Id == selectedId);
                else
                    SelectedCall = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to load calls list", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private BO.ClosedCallInListFields? GetSelectedSortField()
        {
            if (string.IsNullOrEmpty(SelectedSortField))
            {
                return null;
            }

            if (Enum.TryParse(typeof(BO.ClosedCallInListFields), SelectedSortField, out var result))
            {
                return (BO.ClosedCallInListFields?)result;
            }
            return null;
        }

        private BO.CallType? GetSelectedCallType()
        {
            if (string.IsNullOrEmpty(SelectedCallType))
            {
                return null;
            }

            if (Enum.TryParse(typeof(BO.CallType), SelectedCallType, out var result))
            {
                if (result is BO.CallType.All)
                    return null;
                return (BO.CallType?)result;
            }

            return null;
        }

        private void ChangeCallsListSortOrFilter(object sender, SelectionChangedEventArgs e)
        {
            UpdateCallsList();
        }
    }
}
