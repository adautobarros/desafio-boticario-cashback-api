using System.Text.RegularExpressions;

namespace Boticario.Cashback.Dominio.Extensions
{
    public static class StringExtensions
    {
        public static string CampoObrigatorio(this string campo)
        {
            return string.Format("O campo {0} é obrigatório", campo);
        }

        public static string SomenteNumeros(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return new Regex("\\D").Replace(value, "");
        }
    }
}
