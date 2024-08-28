using System;
using System.Text.RegularExpressions;

namespace LdapApplication.Model
{
    public static class Validator
    {
        // Regex para validar nomes (somente texto)
        private static readonly Regex NameRegex = new Regex("^[\\p{L}\\s]+$", RegexOptions.Compiled);

        // Regex para validar login (somente texto)
        private static readonly Regex LoginRegex = new Regex("^[a-zA-Z0-9]+$", RegexOptions.Compiled);

        // Regex para validar telefone (somente números)
        private static readonly Regex PhoneRegex = new Regex("^[0-9]+$", RegexOptions.Compiled);


        public static bool ValidateFullName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return NameRegex.IsMatch(name);
        }


        public static bool ValidateLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return false;

            return LoginRegex.IsMatch(login);
        }

        public static bool ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return PhoneRegex.IsMatch(phone);
        }
    }
}
