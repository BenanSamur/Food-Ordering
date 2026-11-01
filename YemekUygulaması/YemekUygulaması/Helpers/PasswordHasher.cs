using System;
using System.Security.Cryptography;
using System.Text;

namespace YemekUygulaması.Helpers
{
    /// <summary>
    /// Şifreleri PBKDF2 (SHA-256) ile hash'ler ve doğrular.
    /// Saklanan biçim: PBKDF2$iterasyon$tuz(base64)$hash(base64)
    /// Eski kayıtlar düz metin olarak saklandığı için doğrulama sırasında
    /// onlar da kabul edilir ve needsUpgrade = true döner; çağıran taraf
    /// başarılı girişten sonra şifreyi yeniden hash'leyip kaydetmelidir.
    /// </summary>
    public static class PasswordHasher
    {
        private const string Prefix = "PBKDF2";
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static string Hash(string password)
        {
            if (password == null) throw new ArgumentNullException("password");

            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = Derive(password, salt, Iterations, HashSize);
            return string.Join("$", Prefix, Iterations.ToString(), Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool Verify(string password, string stored, out bool needsUpgrade)
        {
            needsUpgrade = false;
            if (password == null || string.IsNullOrEmpty(stored)) return false;

            var parts = stored.Split('$');
            if (parts.Length == 4 && parts[0] == Prefix)
            {
                int iterations;
                if (!int.TryParse(parts[1], out iterations) || iterations < 1) return false;

                byte[] salt;
                byte[] expected;
                try
                {
                    salt = Convert.FromBase64String(parts[2]);
                    expected = Convert.FromBase64String(parts[3]);
                }
                catch (FormatException)
                {
                    return false;
                }

                if (expected.Length == 0) return false;

                byte[] actual;
                try
                {
                    actual = Derive(password, salt, iterations, expected.Length);
                }
                catch (ArgumentException)
                {
                    return false;
                }

                var ok = FixedTimeEquals(actual, expected);
                needsUpgrade = ok && iterations < Iterations;
                return ok;
            }

            // Eski (düz metin) kayıt
            var legacyOk = FixedTimeEquals(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(stored));
            needsUpgrade = legacyOk;
            return legacyOk;
        }

        private static byte[] Derive(string password, byte[] salt, int iterations, int length)
        {
            using (var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return kdf.GetBytes(length);
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            var len = Math.Min(a.Length, b.Length);
            for (var i = 0; i < len; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}
