using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class PasswordUtils
{
    public static string Hash(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash).ToLower();
    }

    public static string ReadPassword(Func<char, bool> validator)
    {
        var sb = new StringBuilder();
        ConsoleKeyInfo key;

        while (true)
        {
            key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter) break;

            if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
            {
                sb.Length--;
                Console.Write("\b \b");
            }
            else if (validator(key.KeyChar))
            {
                sb.Append(key.KeyChar);
                Console.Write("*");
            }
        }
        Console.WriteLine();
        return sb.ToString();
    }
}
