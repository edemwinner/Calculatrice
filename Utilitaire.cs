using System;
using System.Collections.Generic;
using System.Text;

namespace Preparatifs
{
    public static class Utilitaire
    {
        public static bool EstOperateur(this char c)
        {
            return c == '*' || c == '/' || c == '+' || c == '-';
        }
        public static string EvaluerSelonOperateur(this string expression, char operateur)
        {
            bool operationtionNegative = false;
            int index = expression.IndexOf(operateur); // on recherche les opérandes
            if ((operateur == '*' || operateur == '/' || operateur == '-') && expression[index + 1] == '-')//multiplication ou division avec un nombre négatif sans les parenthèses
            {
                operationtionNegative = true;
                expression = expression.Remove(index + 1, 1); //on ignore temporairement le signe négatif
            }
            
            var operande1Builder = new StringBuilder();
            var operande2Builder = new StringBuilder();
            var leftRemainBuilder = new StringBuilder();
            var rightRemainRemainBuilder = new StringBuilder();
            bool rechercherOperateur = true;
            for (var i = index - 1; i >= 0; i--)
            {
                var caract = expression[i];
                if (caract.EstOperateur() || !rechercherOperateur)
                {
                    rechercherOperateur = false;
                    leftRemainBuilder.Insert(0, caract);
                }
                else
                {
                    operande1Builder.Insert(0, caract);
                }
            }
            rechercherOperateur = true;
            for (var i = index + 1; i < expression.Length; i++)
            {
                var caract = expression[i];
                if (caract.EstOperateur() || !rechercherOperateur)
                {
                    rechercherOperateur = false;
                    rightRemainRemainBuilder.Append(caract);
                }
                else
                {
                    operande2Builder.Append(caract);
                }
            }
            string operande1 = operande1Builder.ToString();
            string operande2 = operande2Builder.ToString();
            double tempResult = 0;
            switch (operateur)
            {
                case '*':
                    tempResult = double.Parse(operande1) * double.Parse(operande2);
                    if (operationtionNegative)
                        tempResult = -tempResult; //on reconsidère le signe négatif initialement ignoré
                    break;
                case '/':
                    tempResult = double.Parse(operande1) / double.Parse(operande2);
                    if (operationtionNegative)
                        tempResult = -tempResult;//on reconsidère le signe négatif initialement ignoré
                    break;
                case '+':
                    tempResult = double.Parse(operande1) + double.Parse(operande2);
                    break;
                case '-':
                    tempResult =  double.Parse(operande1) - double.Parse(operande2);
                    if (operationtionNegative)
                        tempResult = -tempResult;//on reconsidère le signe négatif initialement ignoré
                    break;
            }

            var resultExpression = leftRemainBuilder.ToString() + tempResult.ToString() + rightRemainRemainBuilder.ToString();
            return resultExpression;
        }

        public static string EvaluerRegroupement(this string expression)
        {
            var indexDebut = expression.LastIndexOf('(');
            var indexFin = expression.IndexOf(')');
            var regroupement = expression.Substring(indexDebut + 1, indexFin - indexDebut - 1);

            return regroupement;
        }
    }
}
