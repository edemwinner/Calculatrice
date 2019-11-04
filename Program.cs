using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Preparatifs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Entrez une expression: ");
            string valeurEtree = Console.ReadLine();
            Expression expr = new Expression(valeurEtree);
            var regex = new Regex("(\\+|-|\\*|/)");
            Console.WriteLine($"La valeur de l'expression est: {expr.doubleValue}");
        }
    }

    public class Expression
    {
        public string stringValue { get; set; }
        public double doubleValue
        {
            get
            {
                return getValue(stringValue);
            }
        }
        public Expression(string expression)
        {
            stringValue = expression;
        }
        private static double getValue(string stringValue)
        {
            double result;
            try
            {

                if (stringValue.Contains('('))//une parenthèse ouvrante a été trouvée, on cherche donc sa parenthèse fermante
                {
                    var indexDebut = stringValue.LastIndexOf('(');
                    var indexFin = stringValue.Substring(indexDebut).IndexOf(')');
                    var regroupement = stringValue.Substring(indexDebut + 1, indexFin - 1);
                    var tempResult = getValue(regroupement);
                    var remainExpression = stringValue.Substring(0, indexDebut) + tempResult.ToString() + stringValue.Substring(indexDebut + indexFin + 1);
                    return getValue(remainExpression);

                }
                if (stringValue.Contains('*'))
                {
                    return getValue(stringValue.EvaluerSelonOperateur('*'));
                }
                if (stringValue.Contains('/'))
                {
                    return getValue(stringValue.EvaluerSelonOperateur('/'));
                }
                if (stringValue.Contains('-'))
                {
                    var index = stringValue.IndexOf('-');
                    if (stringValue.StartsWith('-'))//cas d'un nomnre négatif
                    {
                        if (double.TryParse(stringValue, out result))
                            return result;
                        else if (stringValue.Contains('+')) // un nombre négatif est additionné à un nomre positif
                        {
                            var partieNegative = stringValue.Substring(0, stringValue.IndexOf('+'));
                            var partiePositive = stringValue.Substring(stringValue.IndexOf('+') + 1);
                            stringValue = partiePositive + partieNegative;
                        }
                        else if (stringValue.Contains('-'))// soustraction d'un nombre à un nombre négatif
                        {
                            var indexSoustraction = 1 + stringValue.Substring(1).IndexOf('-');
                            stringValue = stringValue.Remove(0, 1); //on ignore temporairement le signe négatif
                            stringValue = stringValue.Insert(indexSoustraction, "+"); // la soustraction devient une addition
                            stringValue = stringValue.Remove(indexSoustraction - 1, 1); // on supprime le signe de soustraction
                            return -getValue(stringValue);// on retourne l'opposé de l'addition résultante
                        }
                    }
                    else if (stringValue[index - 1] == '+') //cas d'addition d'un nombre à un nombre négatif:3+-5 par exemple
                        stringValue = stringValue.Remove(index - 1, 1);//l'opération revient à une simple soustraction
                    return getValue(stringValue.EvaluerSelonOperateur('-'));
                }
                if (stringValue.Contains('+'))
                {
                    return getValue(stringValue.EvaluerSelonOperateur('+'));
                }

                if (double.TryParse(stringValue, out result))
                    return result;
                return double.NaN;
            }
            catch (Exception e)
            {
                return double.NaN;
            }
        }
    }

}
