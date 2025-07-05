using System;
using System.Windows;
using System.Windows.Controls;

namespace PL;

/// <summary>
/// Interaction logic for EnterSystemWindow.xaml
/// </summary>
public partial class EnterSystemWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public new string? Name
    {
        get { return (string?)GetValue(NameProperty); }
        set { SetValue(NameProperty, value); }
    }

    public static new readonly DependencyProperty NameProperty =
        DependencyProperty.Register("Name", typeof(string), typeof(EnterSystemWindow), new PropertyMetadata(null));

    public string Password { get; private set; }

    public EnterSystemWindow()
    {
        InitializeComponent();
        Name = "";
        Password = "";
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = ((PasswordBox)sender).Password;
    }

    public void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Name))
                throw new NullReferenceException("No name inputted");

            var volunteer = s_bl.Volunteer.EnterSystem(Name, Password);

            Application.Current.Resources["UserIdKey"] = volunteer.Id;
            if (volunteer.Role == BO.Role.Manager)
                new ManagerWindows.MainWindow().Show();
            else if (volunteer.Role == BO.Role.Volunteer)
                new VolunteerWindows.VolunteerWindow().Show();
            else
                MessageBox.Show("Volunteer's role does not found", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            Name = "";
            Password = "";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


}
