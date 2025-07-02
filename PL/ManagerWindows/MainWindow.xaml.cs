using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PL.ManagerWindows
{
    /// <summary>
    /// Represents the main window of the application, managing simulator controls, 
    /// database operations, and navigation to other windows.
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile DispatcherOperation? _clockObserverOperation = null;
        private volatile DispatcherOperation? _configObserverOperation = null;
        private volatile DispatcherOperation? _callsStatusesCount = null;
        private const int defaultInternal = 10000;
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private Call.CallListWindow callListWindow;
        private Volunteer.VolunteerListWindow? volunteerListWindow;

        /// <summary>
        /// Dependency property for the current time displayed in the application.
        /// </summary>
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));

        /// <summary>
        /// Gets or sets the current time displayed in the application, synchronized with the simulator clock.
        /// </summary>
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        /// <summary>
        /// Dependency property for the risk range used in the simulator.
        /// </summary>
        public static readonly DependencyProperty RiskRangeProperty =
            DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(MainWindow));

        /// <summary>
        /// Gets or sets the risk range used for monitoring and simulation purposes.
        /// </summary>
        public TimeSpan RiskRange
        {
            get { return (TimeSpan)GetValue(RiskRangeProperty); }
            set { SetValue(RiskRangeProperty, value); }
        }

        /// <summary>
        /// Dependency property for the interval in milliseconds for the simulator's operations.
        /// </summary>
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(MainWindow), new PropertyMetadata(defaultInternal));

        /// <summary>
        /// Gets or sets the interval in milliseconds for the simulator's operations.
        /// </summary>
        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// Dependency property indicating whether the simulator is currently running.
        /// </summary>
        public static readonly DependencyProperty IsSimulatorRunningProperty =
            DependencyProperty.Register("IsSimulatorRunning", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the simulator is currently running.
        /// </summary>
        public bool IsSimulatorRunning
        {
            get { return (bool)GetValue(IsSimulatorRunningProperty); }
            set { SetValue(IsSimulatorRunningProperty, value); }
        }

        public static readonly DependencyProperty CallsStatusesProperty =
    DependencyProperty.Register("CallsStatuses", typeof(IEnumerable<Tuple<string, int>>), typeof(MainWindow), new PropertyMetadata(null));

        public IEnumerable<Tuple<string, int>> CallsStatuses
        {
            get { return (IEnumerable<Tuple<string, int>>)GetValue(CallsStatusesProperty); }
            set { SetValue(CallsStatusesProperty, value); }
        }

        /// <summary>
        /// Toggles the simulator's state between running and stopped.
        /// </summary>
        private async void ToggleSimulator(object sender, RoutedEventArgs e)
        {
            if (IsSimulatorRunning)
            {
                await s_bl.Admin.StopSimulator();
                IsSimulatorRunning = false;
            }
            else
            {
                s_bl.Admin.StartSimulator(Interval);
                IsSimulatorRunning = true;
            }
        }

        private void CallsStatusesObserver()
        {
            if (_callsStatusesCount is null || _callsStatusesCount.Status == DispatcherOperationStatus.Completed)
                _callsStatusesCount = Dispatcher.BeginInvoke(() =>
                {
                    CallsStatuses = s_bl.Call.GetCountsGroupByStatus();
                });
        }

        /// <summary>
        /// Advances the simulator clock by one minute.
        /// </summary>
        private void AddMinute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Minute);
        }

        /// <summary>
        /// Advances the simulator clock by one hour.
        /// </summary>
        private void AddHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Hour);
        }

        /// <summary>
        /// Advances the simulator clock by one day.
        /// </summary>
        private void AddDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Day);
        }

        /// <summary>
        /// Advances the simulator clock by one month.
        /// </summary>
        private void AddMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Month);
        }

        /// <summary>
        /// Advances the simulator clock by one year.
        /// </summary>
        private void AddYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Year);
        }

        /// <summary>
        /// Updates the risk range in the simulator configuration.
        /// </summary>
        private void UpdateRiskRange(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetRiskRange(RiskRange);
        }

        /// <summary>
        /// Observes and updates the current time from the simulator.
        /// </summary>
        private void ClockObserver()
        {
            if (_clockObserverOperation is null || _clockObserverOperation.Status == DispatcherOperationStatus.Completed)
                _clockObserverOperation = Dispatcher.BeginInvoke(() =>
                {
                    CurrentTime = s_bl.Admin.GetClock();
                });
        }

        /// <summary>
        /// Observes and updates the risk range from the simulator configuration.
        /// </summary>
        private void ConfigObserver()
        {
            if (_configObserverOperation is null || _configObserverOperation.Status == DispatcherOperationStatus.Completed)
                _configObserverOperation = Dispatcher.BeginInvoke(() =>
                {
                    RiskRange = s_bl.Admin.GetRiskRange();
                });
        }

        /// <summary>
        /// Handles the window's closing event, ensuring proper cleanup of resources.
        /// </summary>
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

        /// <summary>
        /// Opens the calls list window or activates it if already open.
        /// </summary>
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
                else if (IsSimulatorRunning == true)
                    new Call.CallListWindow().Show();
                else
                    callListWindow.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to show calls list window", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Opens the volunteers list window or activates it if already open.
        /// </summary>
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
                else if (IsSimulatorRunning == true)
                    new Volunteer.VolunteerListWindow().Show();
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

        /// <summary>
        /// Initializes the database and closes all other windows except the main window.
        /// </summary>
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

        /// <summary>
        /// Resets the database and closes all other windows except the main window.
        /// </summary>
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

        /// <summary>
        /// Closes all open windows except the main window.
        /// </summary>
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

        /// <summary>
        /// Handles the window's loaded event, initializing data and observers.
        /// </summary>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            RiskRange = s_bl.Admin.GetRiskRange();

            var statusCountsArray = s_bl.Call.GetCountsGroupByStatus(); // שהשורה הזו מחזירה int[]
            var statusCounts = new List<Tuple<string, int>>();

            for (int i = 0; i < statusCountsArray.Length; i++)
            {
                if (statusCountsArray[i] > 0) // אם הכמות גדולה מ-0
                {
                    statusCounts.Add(new Tuple<string, int>
                    (
                        ((BO.FinishCallType)i).ToString(),  // המרת הסטטוס לשם
                        statusCountsArray[i]
                    ));
                }
            }

            CallsStatuses = statusCounts; // CallsStatuses צריך להיות מסוג IEnumerable<Tuple<string, int>>
            s_bl.Admin.AddClockObserver(ClockObserver);
            s_bl.Admin.AddConfigObserver(ConfigObserver);
            s_bl.Call.AddObserver(CallsStatusesObserver);
            this.DataContext = this;
        }

        public MainWindow()
        {
            InitializeComponent();
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Tuple<string, int> selectedStatus)
            {
                // כאן תוכל לפתוח את חלון הקריאות בהתאם לסטטוס שנבחר
                var callsWindow = new Call.CallListWindow(); // או whatever חלון שצריך
                callsWindow.LoadCallsByStatus(selectedStatus.Item1); // פעולה לפתוח חלון על פי הסטטוס
                callsWindow.Show();
            }
        }
    }
}
