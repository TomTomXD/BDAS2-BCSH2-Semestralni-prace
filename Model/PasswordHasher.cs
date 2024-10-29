using System;
using System.Security.Cryptography;
using System.Text;

namespace InformacniSystemBanky.Model
{
    public class PasswordHasher
    {
        private static readonly char[] SaltChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        // Metoda pro generování náhodné soli obsahující písmena a čísla
        public static string GenerateSalt(int size = 16)
        {
            var saltBytes = new byte[size];
            RandomNumberGenerator.Fill(saltBytes);
            var saltChars = new char[size];

            for (int i = 0; i < size; i++)
            {
                saltChars[i] = SaltChars[saltBytes[i] % SaltChars.Length];
            }

            return new string(saltChars);
        }

        // Metoda pro hashování hesla se solí
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Metoda pro ověření hesla
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var hashOfEnteredPassword = HashPassword(enteredPassword, storedSalt);
            return hashOfEnteredPassword == storedHash;
        }
    }
}