using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PL.ManagerWindows.Volunteer;

/// <summary>
/// Interaction logic for VolunteerWindow.xaml
/// </summary>
public partial class VolunteerWindow : Window
{
    /// <summary>
    /// Represents the observer operation for updating the volunteer data.
    /// </summary>
    private volatile DispatcherOperation? _observerOperation = null;

    /// <summary>
    /// Reference to the business logic layer.
    /// </summary>
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Gets or sets the current volunteer being managed in the window.
    /// </summary>
    public BO.Volunteer? CurrentVolunteer
    {
        get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
        set { SetValue(CurrentVolunteerProperty, value); }
    }

    /// <summary>
    /// Dependency property for the current volunteer.
    /// </summary>
    public static readonly DependencyProperty CurrentVolunteerProperty =
        DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the text displayed on the button (e.g., "ADD" or "UPDATE").
    /// </summary>
    public string ButtonText
    {
        get { return (string)GetValue(ButtonTextProperty); }
        set { SetValue(ButtonTextProperty, value); }
    }

    /// <summary>
    /// Dependency property for the button text.
    /// </summary>
    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register("ButtonText", typeof(string), typeof(VolunteerWindow), new PropertyMetadata(null));

    /// <summary>
    /// Initializes a new instance of the <see cref="VolunteerWindow"/> class.
    /// </summary>
    /// <param name="id">The ID of the volunteer to manage. If 0, a new volunteer is created.</param>
    public VolunteerWindow(int id = 0)
    {
        try
        {
            ButtonText = id == 0 ? "ADD" : "UPDATE";
            InitializeComponent();
            CurrentVolunteer = (id != 0) ?
                s_bl.Volunteer.Read(id)! :
                new BO.Volunteer();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Handles the click event for the Add/Update button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (CurrentVolunteer == null)
                throw new NullReferenceException("currentVolunteer is null");
            else if (ButtonText == "ADD")
                s_bl.Volunteer.Create(CurrentVolunteer);
            else
            {
                int userId = (int)Application.Current.Resources["UserIdKey"];
                s_bl.Volunteer.Update(userId, CurrentVolunteer);
            };
            MessageBox.Show($"Succeed to {ButtonText} volunteer {CurrentVolunteer.Id}", "success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Updates the current volunteer's data by observing changes.
    /// </summary>
    private void VolunteerObserver()
    {
        if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
            _observerOperation = Dispatcher.BeginInvoke(() =>
            {
                int id = CurrentVolunteer!.Id;
                CurrentVolunteer = null;
                CurrentVolunteer = s_bl.Volunteer.Read(id);
            });
    }

    /// <summary>
    /// Validates that the input in the text box is numeric.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data containing the input text.</param>
    private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Regex to check if the input is a number
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    /// <summary>
    /// Adds the VolunteerObserver to the observers list when the window is loaded.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, VolunteerObserver);
    }

    /// <summary>
    /// Removes the VolunteerObserver from the observers list when the window is closed.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void Window_Closed(object sender, EventArgs e)
    {
        s_bl.Volunteer.RemoveObserver(VolunteerObserver);
    }
}
