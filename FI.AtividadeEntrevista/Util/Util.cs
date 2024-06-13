using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Util
{
    public class Util
    {
        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11)
                return false;

            // Elimina CPFs inválidos conhecidos
            switch (cpf)
            {
                case "00000000000":
                case "11111111111":
                case "22222222222":
                case "33333333333":
                case "44444444444":
                case "55555555555":
                case "66666666666":
                case "77777777777":
                case "88888888888":
                case "99999999999":
                    return false;
            }

            // Valida primeiro dígito verificador
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            // Valida segundo dígito verificador
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}
