using FinancniInformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Data;
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
                Note = reader.IsDBNull(3) ? null : reader.GetString(3),
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


        public void UploadFile(int? fileId, string fileName, DateTime uploadDate, byte[] fileContent, string note, int ownerId)
        {
            try
            {
                _databaseService.ExecuteProcedure("upsert_soubor", command =>
                {
                    command.Parameters.Add("p_id_soubor", OracleDbType.Int32).Value = fileId.HasValue ? (object)fileId.Value : DBNull.Value;
                    command.Parameters.Add("p_nazev_souboru", OracleDbType.Varchar2).Value = fileName;
                    command.Parameters.Add("p_datum_nahrani", OracleDbType.Date).Value = uploadDate;
                    command.Parameters.Add("p_soubor", OracleDbType.Blob).Value = fileContent;
                    command.Parameters.Add("p_poznamka", OracleDbType.Varchar2).Value = note;
                    command.Parameters.Add("p_id_osoba", OracleDbType.Int32).Value = ownerId;
                });

                MessageBox.Show("Soubor byl úspěšně nahrán do databáze.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při nahrávání souboru do databáze: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateFile(int fileId, string fileName, DateTime uploadDate, byte[] fileContent, string note, int ownerId)
        {
            try
            {
                _databaseService.ExecuteProcedure("upsert_soubor", command =>
                {
                    command.Parameters.Add("p_id_soubor", OracleDbType.Int32).Value = fileId;
                    command.Parameters.Add("p_nazev_souboru", OracleDbType.Varchar2).Value = fileName;
                    command.Parameters.Add("p_datum_nahrani", OracleDbType.Date).Value = uploadDate;
                    command.Parameters.Add("p_soubor", OracleDbType.Blob).Value = fileContent;
                    command.Parameters.Add("p_poznamka", OracleDbType.Varchar2).Value = note;
                    command.Parameters.Add("p_id_osoba", OracleDbType.Int32).Value = ownerId;
                });

                MessageBox.Show("Soubor byl úspěšně upraven.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při upravování souboru v databázi: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
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