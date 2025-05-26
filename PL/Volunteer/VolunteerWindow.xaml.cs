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

namespace PL.Volunteer;

/// <summary>
/// Interaction logic for VolunteerWindow.xaml
/// </summary>
public partial class VolunteerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Volunteer? CurrentVolunteer
    {
        get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
        set { SetValue(CurrentVolunteerProperty, value); }
    }

    public static readonly DependencyProperty CurrentVolunteerProperty =
        DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));


    public string ButtonText
    {
        get { return (string)GetValue(ButtonTextProperty); }
        set { SetValue(ButtonTextProperty, value); }
    }

    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register("ButtonText", typeof(string), typeof(VolunteerWindow), new PropertyMetadata(null));

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

    private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        
        try
        {
            if (CurrentVolunteer == null)
                throw new NullReferenceException("currentVolunteer is null");
            else if (ButtonText == "ADD")
                s_bl.Volunteer.Create(CurrentVolunteer);
            else
                s_bl.Volunteer.Update(CurrentVolunteer.Id, CurrentVolunteer);
            MessageBox.Show($"Succeed to {ButtonText} volunteer {CurrentVolunteer.Id}", "success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    /// <summary>
    /// update the Volunteer
    /// </summary>
    private void VolunteerObserver()
    {
        int id = CurrentVolunteer!.Id;
        CurrentVolunteer = null;
        CurrentVolunteer = s_bl.Volunteer.Read(id);
    }

    /// <summary>
    /// add the VolunteerObserver to the observers list
    /// </summary>
    /// <param name="sender">The source of the event</param>
    /// <param name="e">The event data that contains the new selection state.</param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, VolunteerObserver);
    }

    /// <summary>
    /// remove the VolunteerListObserver from the observers list
    /// </summary>
    /// <param name="sender">The source of the event</param>
    /// <param name="e">The event data that contains the new selection state.</param>

    private void Window_Closed(object sender, EventArgs e)
    {
        s_bl.Volunteer.RemoveObserver(VolunteerObserver);
    }
}
