using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty MaxYearsProperty =
            DependencyProperty.Register("MaxYears", typeof(TimeSpan), typeof(MainWindow), new PropertyMetadata(TimeSpan.Zero));


        public TimeSpan MaxYears
        {
            get { return (TimeSpan)GetValue(MaxYearsProperty); }
            set { SetValue(MaxYearsProperty, value); }
        }

        private void AddOneMinuteByClick()
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Minute);
        }
        private void AddOneHourByClick()
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Hour);
        }
        private void AddOneDayByClick()
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Day);
        }
        private void AddOneMonthByClick()
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Month);
        }
        private void AddOneYearByClick()
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.Year);
        }
        private void UpdateRiskRange()
        {
            s_bl.Admin.SetRiskRange(MaxYears);
        }
        private void clockObserver()
        {
            CurrentTime = s_bl.Admin.GetClock();

        }
        private void configObserver()
        {
            MaxYears = s_bl.Admin.GetRiskRange();

        }
      private void CloseWindow()
        {
            s_bl.Admin.RemoveClockObserver(clockObserver);
            s_bl.Admin.RemoveConfigObserver(configObserver);
            this.Close();
        }
        private void CallsListClick(object sender, RoutedEventArgs e)
        { new Call.CallListWindow().Show(); }
        private void VolunteersListClick(object sender, RoutedEventArgs e)
        { new Volunteer.VolunteerListWindow().Show(); }

        private void InitializationDBClick(object sender, RoutedEventArgs e)
        {
            
            if (MessageBox.Show("Are you sure you want to initialize the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                
                Mouse.OverrideCursor = Cursors.Wait;

                try
                {
                    
                    s_bl.Admin.InitializeDB();

                    
                    CloseAllWindowsExceptMain();
                }
                catch (Exception ex)
                {
                    // Handle any potential errors
                    MessageBox.Show($"An error occurred while initializing the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // Restore the cursor to default
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void ResetDBClick(object sender, RoutedEventArgs e)
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


        public MainWindow()
        {
            CurrentTime = s_bl.Admin.GetClock();
            s_bl.Admin.AddClockObserver(clockObserver);
            s_bl.Admin.AddConfigObserver(configObserver);
            InitializeComponent();
            this.DataContext = this;

        }
    }
}