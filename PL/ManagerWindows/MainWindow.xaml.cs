using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PL.ManagerWindows
{
    public partial class MainWindow : Window
    {
        private volatile DispatcherOperation? _clockObserverOperation = null;
        private volatile DispatcherOperation? _configObserverOperation = null;

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private Call.CallListWindow callListWindow;
        private Volunteer.VolunteerListWindow? volunteerListWindow;

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty RiskRangeProperty =
            DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(MainWindow));

        public TimeSpan RiskRange
        {
            get { return (TimeSpan)GetValue(RiskRangeProperty); }
            set { SetValue(RiskRangeProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(MainWindow), new PropertyMetadata(1));

        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty IsSimulatorRunningProperty =
            DependencyProperty.Register("IsSimulatorRunning", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public bool IsSimulatorRunning
        {
            get { return (bool)GetValue(IsSimulatorRunningProperty); }
            set { SetValue(IsSimulatorRunningProperty, value); }
        }
        private void ToggleSimulator(object sender, RoutedEventArgs e)
        {
            if (IsSimulatorRunning)
            {
                s_bl.Admin.StopSimulator();
                IsSimulatorRunning = false;
            }
            else
            {
                s_bl.Admin.StartSimulator(Interval);
                IsSimulatorRunning = true;
            }
        }




        private void AddMinute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Minute);
        }

        private void AddHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Hour);
        }

        private void AddDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Day);
        }

        private void AddMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Month);
        }

        private void AddYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Year);
        }

        private void UpdateRiskRange(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetRiskRange(RiskRange);
        }

        private void ClockObserver()
        {
            if (_clockObserverOperation is null || _clockObserverOperation.Status == DispatcherOperationStatus.Completed)
                _clockObserverOperation = Dispatcher.BeginInvoke(() =>
                {
                    CurrentTime = s_bl.Admin.GetClock();
                });
        }

        private void ConfigObserver()
        {
            if (_configObserverOperation is null || _configObserverOperation.Status == DispatcherOperationStatus.Completed)
                _configObserverOperation = Dispatcher.BeginInvoke(() =>
                {
                    RiskRange = s_bl.Admin.GetRiskRange();
                });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (IsSimulatorRunning)
            {
                s_bl.Admin.StopSimulator(); // Ensure simulator is stopped
            }
            s_bl.Admin.RemoveClockObserver(ClockObserver);
            s_bl.Admin.RemoveConfigObserver(ConfigObserver);
            this.Close();
        }

        private void CallsList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (callListWindow == null || !callListWindow.IsVisible)
                {
                    callListWindow = new Call.CallListWindow();
                    callListWindow.Closed += (s, args) => callListWindow = null; // Reset reference when closed
                    callListWindow.Show();
                }
                else
                {
                    callListWindow.Activate(); // Bring the existing window to the front
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show calls list window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VolunteersList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (volunteerListWindow == null || !volunteerListWindow.IsVisible)
                {
                    volunteerListWindow = new Volunteer.VolunteerListWindow();
                    volunteerListWindow.Closed += (s, args) => volunteerListWindow = null;
                    volunteerListWindow.Show();
                }
                else
                {
                    volunteerListWindow.Activate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show volunteer list window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializationDB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to initialize the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                try
                {
                    s_bl.Admin.InitializationDB();
                    CloseAllWindowsExceptMain();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while initializing the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void ResetDB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                try
                {
                    s_bl.Admin.ResetDB();
                    CloseAllWindowsExceptMain();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while resetting the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void CloseAllWindowsExceptMain()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this) // Keep the main window open
                {
                    window.Close();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            RiskRange = s_bl.Admin.GetRiskRange();
            s_bl.Admin.AddClockObserver(ClockObserver);
            s_bl.Admin.AddConfigObserver(ConfigObserver);
            this.DataContext = this;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
