using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using FinancniInformacniSystemBanky.DatabaseLayer;
using InformacniSystemBanky.Model;

public class RegisterCreatePasswordViewModel : INotifyPropertyChanged
{
    private string _password;
    private readonly Window _registrationCreatePassword;
    private readonly UserRegistrationViewModel _userRegistrationViewModel;
    private readonly UserService _userService;
    public ICommand GoNextCommand { get; }
    public ICommand GoBackCommand { get; }

    public RegisterCreatePasswordViewModel(Window registerCreatePassword, UserRegistrationViewModel userRegistrationViewModel)
    {
        _registrationCreatePassword = registerCreatePassword;
        _userRegistrationViewModel = userRegistrationViewModel;
        _userService = new UserService();
        GoNextCommand = new RelayCommand(GoNext);
        GoBackCommand = new RelayCommand(GoBack);
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    private string _passwordAgain;
    public string PasswordAgain
    {
        get => _passwordAgain;
        set
        {
            _passwordAgain = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    private void GoNext()
    {
        if (!string.Equals(Password, PasswordAgain, StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("Hesla se neshodují. Prosím, zkontrolujte je a zkuste to znovu.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        else
        {
            try
            {
                var salt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.HashPassword(Password, salt);
                var saltBase64 = Convert.ToBase64String(salt);

                _userService.RegisterNewUser(
                    _userRegistrationViewModel.FirstName,
                    _userRegistrationViewModel.LastName,
                    _userRegistrationViewModel.DateOfBirth,
                    _userRegistrationViewModel.RodneCislo,
                    _userRegistrationViewModel.PhoneNumber,
                    _userRegistrationViewModel.Email,
                    "K",
                    2,
                    _userRegistrationViewModel.AddressStreet,
                    _userRegistrationViewModel.AddressHouseNumber,
                    _userRegistrationViewModel.AddressCity,
                    int.Parse(_userRegistrationViewModel.AddressZipCode),
                    hashedPassword,
                    saltBase64
                );

                MessageBox.Show("Hesla jsou v pořádku, probíhá registrace.", "Registrace", MessageBoxButton.OK, MessageBoxImage.Information);
                _registrationCreatePassword.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při registraci: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void GoBack()
    {
        var registerAddressView = new RegisterAddressView(_userRegistrationViewModel);
        registerAddressView.Show();
        _registrationCreatePassword.Close();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
