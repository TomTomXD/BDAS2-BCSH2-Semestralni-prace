using System;
using System.Security.Cryptography;
using System.Text;

namespace InformacniSystemBanky.Model
{
    public class PasswordHasher
    {
        private const int SaltSize = 16; // Velikost soli v bajtech
        private const int HashSize = 32; // Velikost výsledného hashe v bajtech
        private const int Iterations = 300000; // Počet iterací pro PBKDF2

        // Metoda pro generování náhodné soli
        public static byte[] GenerateSalt(int size = SaltSize)
        {
            var salt = new byte[size];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        // Metoda pro hashování hesla se solí pomocí PBKDF2
        public static string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hash = pbkdf2.GetBytes(HashSize);

                // Spojení soli a hashe do jednoho pole pro uložení
                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                // Vrácení výsledku jako Base64 řetězec
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Metoda pro ověření hesla
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Převod uloženého hashe ze stringu do byte[]
            var hashBytes = Convert.FromBase64String(storedHash);

            // Extrahování soli z uloženého hashe
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Vypočítání hashe pro zadané heslo se získanou solí
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hash = pbkdf2.GetBytes(HashSize);

                // Porovnání vypočítaného hashe s uloženým hashem
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i]) return false;
                }
                return true;
            }
        }
    }
}
