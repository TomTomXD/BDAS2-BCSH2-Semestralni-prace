namespace FinancniInformacniSystemBanky.Model
{
    public class Session
    {
        private static Session _instance;

        public static Session Instance => _instance ??= new Session();

        public int LoggedInUserId { get; private set; }
        public int LoggedInRoleId { get; private set; }

        public int? EmulatedUserId { get; private set; }
        public int? EmulatedRoleId { get; private set; }

        private Session() { }

        // Nastavení přihlášeného uživatele
        public void SetUser(int userId, int roleId)
        {
            LoggedInUserId = userId;
            LoggedInRoleId = roleId;

            // Ukončí emulaci, pokud nějaká běží
            EmulatedUserId = null;
            EmulatedRoleId = null;
        }

        //Emulace jiného uživatele příprava na emulaci pro admina(BDAS2 požadavek)
        public void EmulateUser(int userId, int roleId)
        {
            EmulatedUserId = userId;
            EmulatedRoleId = roleId;
        }


        // Aktuální kontext uživatele (přihlášený nebo emulovaný)
        public int CurrentUserId => LoggedInUserId;
        public int CurrentRoleId => LoggedInRoleId;
    }
}
