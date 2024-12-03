using FinancniInformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class FileService
    {
        private readonly DatabaseService _databaseService;

        public FileService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<File> GetFiles()
        {
            string query = @"SELECT ID_SOUBOR, NAZEV_SOUBORU, DATUM_NAHRANI, POZNAMKA, ID_OSOBA FROM SOUBORY";

            return _databaseService.ExecuteSelect(query, reader => new File
            {
                FileId = reader.GetInt32(0),
                FileName = reader.GetString(1),
                UploadDate = DateOnly.FromDateTime(reader.GetDateTime(2)),
                Note = reader.GetString(3),
                OwnerId = reader.GetInt32(4)
            });
        }

        public void SaveFileToDisk(int idSoubor)
        {
            // Inicializace dialogu pro výběr souboru
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Vyberte místo pro uložení souboru",
                Filter = "Všechny soubory (*.*)|*.*",
                FileName = "NovySoubor"
            };

            // Zobrazení dialogu
            if (saveFileDialog.ShowDialog() == true)
            {
                // Vybraný souborový cestu
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Stažení obsahu souboru z databáze
                    byte[] fileContent = DownloadFile(idSoubor);

                    // Uložení souboru
                    if (fileContent != null && fileContent.Length > 0)
                    {
                        System.IO.File.WriteAllBytes(filePath, fileContent);
                        MessageBox.Show($"Soubor byl úspěšně uložen do: {filePath}", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Soubor nebyl nalezen nebo je prázdný.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Došlo k chybě při ukládání souboru: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Uložení souboru bylo zrušeno uživatelem.", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        public void UploadFile()
        {
            try
            {
                // Dialog pro výběr souboru
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Title = "Vyberte soubor k nahrání",
                    Filter = "Obrázky a PDF soubory (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pdf)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pdf"
                };

                // Pokud uživatel vybere soubor
                if (openFileDialog.ShowDialog() == true)
                {
                    // Získání cesty k souboru
                    string filePath = openFileDialog.FileName;

                    // Převedení souboru na byte[]
                    byte[] fileContent = System.IO.File.ReadAllBytes(filePath);

                    // Informace o souboru
                    string fileName = System.IO.Path.GetFileName(filePath);

                    // Zobrazení zprávy o úspěchu
                    MessageBox.Show($"Soubor '{fileName}' byl načten a je připraven k nahrání do databáze.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Příklad, jak předat data do budoucí procedury (zatím není implementována)
                }
                else
                {
                    MessageBox.Show("Nahrávání souboru bylo zrušeno uživatelem.", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při načítání souboru: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteFile(int fileId)
        {
            try
            {
                _databaseService.ExecuteNonQuery("DELETE FROM SOUBORY WHERE ID_SOUBOR = :idSoubor",
                    command => command.Parameters.Add(new OracleParameter("idSoubor", fileId))
                );
                MessageBox.Show("Soubor byl úspěšně smazán.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při mazání souboru: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private byte[] DownloadFile(int fileId)
        {
            string query = @"SELECT SOUBOR FROM SOUBORY WHERE ID_SOUBOR = :idSoubor";

            return _databaseService.ExecuteSelect(query,
                reader => (byte[])reader["SOUBOR"],
                command => command.Parameters.Add(new OracleParameter("idSoubor", fileId))
            ).FirstOrDefault();
        }

    }
}