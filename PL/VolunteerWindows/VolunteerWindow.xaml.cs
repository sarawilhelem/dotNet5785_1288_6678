using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace PL.VolunteerWindows
{
    public partial class VolunteerWindow : Window, INotifyPropertyChanged
    {
        private volatile DispatcherOperation? _observerOperation = null;
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private BO.Volunteer? _currentVolunteer;
        private BO.Call? _currentVolunteerCall;
        private string? _mapLink;

        private int volunteerId;
        
        public BO.Volunteer? CurrentVolunteer
        {
            get => _currentVolunteer;
            set
            {
                if (_currentVolunteer != value)
                {
                    _currentVolunteer = value;
                    OnPropertyChanged(nameof(CurrentVolunteer));
                    UpdateCurrentVolunteerCall();
                    UpdateMapLink();
                }
            }
        }

        public BO.Call? CurrentVolunteerCall
        {
            get => _currentVolunteerCall;
            set
            {
                if (_currentVolunteerCall != value)
                {
                    _currentVolunteerCall = value;
                    OnPropertyChanged(nameof(CurrentVolunteerCall));
                    UpdateMapLink();
                }
            }
        }

        public string? MapLink
        {
            get => _mapLink;
            set
            {
                if (_mapLink != value)
                {
                    _mapLink = value;
                    OnPropertyChanged(nameof(MapLink));
                }
            }
        }

        public VolunteerWindow()
        {
            InitializeComponent();
            try
            {
                volunteerId = (int)Application.Current.Resources["UserIdKey"];
                LoadVolunteer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadVolunteer()
        {
            CurrentVolunteer = s_bl.Volunteer.Read(volunteerId);
        }

        private void UpdateCurrentVolunteerCall()
        {
            if (CurrentVolunteer?.Call != null)
                CurrentVolunteerCall = s_bl.Call.Read(CurrentVolunteer.Call.CallId);
            else
                CurrentVolunteerCall = null;
        }

        private void UpdateMapLink()
        {
            if (CurrentVolunteer?.Address == null || CurrentVolunteerCall?.Address == null)
            {
                MapLink = null;
                return;
            }

            try
            {
                MapLink = s_bl.Call.GetDirectionsLink(CurrentVolunteer.Address, CurrentVolunteerCall.Address);
            }
            catch
            {
                MapLink = null;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer == null)
                    throw new NullReferenceException("CurrentVolunteer is null");

                int userId = (int)Application.Current.Resources["UserIdKey"];
                s_bl.Volunteer.Update(userId, CurrentVolunteer);
                MessageBox.Show($"Succeed to Update volunteer {CurrentVolunteer.Id} details", "success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnCallsHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new CallsHistory().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show calls history window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnChooseCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new ChooseCall().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show choose call window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnFinishProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer?.Call?.Id == null)
                {
                    MessageBox.Show("Volunteer does not have an assignment to a call");
                    return;
                }
                s_bl.Call.FinishProcess(CurrentVolunteer.Id, CurrentVolunteer.Call.Id);
                MessageBox.Show("Process was finished successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void BtnCancelProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentVolunteer?.Call?.Id == null)
                {
                    MessageBox.Show("Volunteer does not have an assignment to a call");
                    return;
                }
                int userId = (int)Application.Current.Resources["UserIdKey"];
                s_bl.Call.CancelProcess(userId, CurrentVolunteer.Call.Id);
                MessageBox.Show("Process was canceled successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void VolunteerObserver()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    LoadVolunteer();
                });
        }

        private void Window_Loeded(object sender, RoutedEventArgs e)
        {
            s_bl.Volunteer.AddObserver(volunteerId, VolunteerObserver);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(volunteerId, VolunteerObserver);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
