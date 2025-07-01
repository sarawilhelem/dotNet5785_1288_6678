using PL.ManagerWindows.Call;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace PL.VolunteerWindows
{
    public partial class ChooseCall : Window, INotifyPropertyChanged
    {
        private volatile DispatcherOperation? _observerOperation = null;
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        static int volunteerId;

        public IEnumerable<BO.OpenCallInList> CallList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        private BO.OpenCallInList? _selectedCall;
        public BO.OpenCallInList? SelectedCall
        {
            get => _selectedCall;
            set
            {
                if (_selectedCall != value)
                {
                    _selectedCall = value;
                    OnPropertyChanged(nameof(SelectedCall));
                    UpdateMapLink();
                }
            }
        }

        private string? _mapLink;
        public string? MapLink
        {
            get => _mapLink;
            set
            {
                if (_mapLink != value)
                {
                    _mapLink = value;
                    OnPropertyChanged(nameof(MapLink));
                    OnPropertyChanged(nameof(MapLinkVisibility));
                }
            }
        }

        public Visibility MapLinkVisibility =>
            string.IsNullOrEmpty(MapLink) ? Visibility.Collapsed : Visibility.Visible;

        public ICommand ChooseCallCommand { get; }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register(
                "CallList",
                typeof(IEnumerable<BO.OpenCallInList>),
                typeof(ChooseCall)
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

        public ChooseCall()
        {
            InitializeComponent();
            ChooseCallCommand = new RelayCommand<BO.OpenCallInList>(AssignCallVolunteer);
            volunteerId = (int)Application.Current.Resources["UserIdKey"];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Call.AddObserver(UpdateCallsList);
            UpdateCallsList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Call.RemoveObserver(UpdateCallsList);
        }

        private void UpdateCallsList()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    var selectedId = SelectedCall?.Id;
                    BO.OpenCallInListFields? sortField = GetSelectedSortField();
                    BO.CallType? callType = GetSelectedCallType();
                    try
                    {
                        CallList = s_bl!.Call.ReadAllVolunteerOpenCalls(volunteerId, callType, sortField);

                        if (selectedId != null)
                            SelectedCall = CallList?.FirstOrDefault(c => c.Id == selectedId);
                        else
                            SelectedCall = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Failed to load calls list", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
        }

        private void UpdateMapLink()
        {
            if (SelectedCall == null)
            {
                MapLink = null;
                return;
            }

            try
            {
                var volunteer = s_bl.Volunteer.Read(volunteerId);
                MapLink = s_bl.Call.GetDirectionsLink(volunteer?.Address, SelectedCall.Address);
            }
            catch
            {
                MapLink = null;
            }
        }

        private BO.OpenCallInListFields? GetSelectedSortField()
        {
            if (string.IsNullOrEmpty(SelectedSortField))
                return null;

            if (Enum.TryParse(typeof(BO.OpenCallInListFields), SelectedSortField, out var result))
                return (BO.OpenCallInListFields?)result;

            return null;
        }

        private BO.CallType? GetSelectedCallType()
        {
            if (string.IsNullOrEmpty(SelectedCallType))
                return null;

            if (Enum.TryParse(typeof(BO.CallType), SelectedCallType, out var result))
            {
                if (result is BO.CallType.All)
                    return null;
                return (BO.CallType?)result;
            }

            return null;
        }

        private void AssignCallVolunteer(BO.OpenCallInList call)
        {
            try
            {
                s_bl.Call.ChooseCall(volunteerId, call.Id);
                MessageBox.Show("Succeed to choose the call", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeCallsListSortOrFilter(object sender, SelectionChangedEventArgs e)
        {
            UpdateCallsList();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}