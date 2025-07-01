using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PL.ManagerWindows.Call
{
    /// <summary>
    /// Interaction logic for CallWindow.xaml
    /// </summary>
    public partial class CallWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private volatile DispatcherOperation? _observerOperation = null; //stage 7

        public BO.Call? CurrentCall
        {
            get { return (BO.Call?)GetValue(CurrentCallProperty); }
            set { SetValue(CurrentCallProperty, value); }
        }

        public static readonly DependencyProperty CurrentCallProperty =
            DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(CallWindow), new PropertyMetadata(null));


        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(CallWindow), new PropertyMetadata(null));

        public CallWindow(int id = 0)
        {
            try
            {
                ButtonText = id == 0 ? "ADD" : "UPDATE";
                InitializeComponent();
                CurrentCall = (id != 0) ?
                    s_bl.Call.Read(id)! :
                    new BO.Call() { MaxCloseTime = s_bl.Admin.GetClock() };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (CurrentCall == null)
                    throw new NullReferenceException("currentCall is null");
                else if (ButtonText == "ADD")
                {
                    s_bl.Call.Create(CurrentCall);
                    MessageBox.Show($"Succeed to ADD Call", "success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Call.Update(CurrentCall);
                    MessageBox.Show($"Succeed to UPDATE Call {CurrentCall.Id}", "success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        /// <summary>
        /// update the call
        /// </summary>
        private void CallObserver()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    int id = CurrentCall!.Id;
                    CurrentCall = null;
                    CurrentCall = s_bl.Call.Read(id);
                });
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// add the callObserver to the observers list
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Call.AddObserver(CurrentCall!.Id, CallObserver);
        }

        /// <summary>
        /// remove the callListObserver from the observers list
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data that contains the new selection state.</param>

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Call.RemoveObserver(CallObserver);
        }
    }
}
