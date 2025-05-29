using BO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallListWindow.xaml
    /// </summary>
    public partial class CallListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.CallInList> CallList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }
        public BO.CallInList? SelectedCall
        {
            get;
            set;
        }


        public static readonly DependencyProperty CallListProperty =
           DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow));
        public BO.CallType SelectedCallType { get; set; } = BO.CallType.All;
        public ICommand DeleteCallCommand { get; }
        public CallListWindow()
        {
            InitializeComponent();
            DeleteCallCommand = new RelayCommand<BO.CallInList>(DeleteCall);
        }
        /// <summary>
        /// Handles the selection change event to update the call list based on the selected filter.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>
        private void ChangeCallsListFilter(object sender, SelectionChangedEventArgs e)
        {
            UpdateCallsList();
        }
        /// <summary>
        /// Update the call list from bl
        /// </summary>
        private void UpdateCallsList()
        {
            CallList = (SelectedCallType == BO.CallType.All) ?
                s_bl?.Call.ReadAll()! :
                s_bl?.Call.ReadAll(BO.CallInListFields.CallType, SelectedCallType, null)!;
        }

        /// <summary>
        /// update the call list
        /// </summary>
        private void CallListObserver()
        {
            UpdateCallsList();
        }

        /// <summary>
        /// add the callListObserver to the observers list
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Call.AddObserver(CallListObserver);
        }

        /// <summary>
        /// remove the callListObserver from the observers list
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Call.RemoveObserver(CallListObserver);
        }
        private void AddCall_Click(object sender, RoutedEventArgs e)
        {
            new CallWindow().Show();
        }

        private void Call_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int id = SelectedCall?.CallId ??
                throw new ArgumentNullException("No call was accepted for deletion");
            new CallWindow(id).Show();
        }

        private void DeleteCall(BO.CallInList call)
        {
            try
            {
                if (call == null)
                    throw new ArgumentNullException("No call was accepted for deletion");

                var result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את הפריט?", "אישור מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int id = call.CallId;
                    s_bl.Call.Delete(id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




    }
}
