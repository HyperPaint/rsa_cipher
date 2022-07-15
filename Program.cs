using System;
using System.Numerics;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите сообщение");
            string message = Console.ReadLine();
            // генерация ключей
            int p, q;
            do
            {
                Console.WriteLine("Введите простое число p (101)");
                p = Convert.ToInt32(Console.ReadLine());
            } while (!isSimple(p));
            do
            {
                Console.WriteLine("Введите простое число q (113)");
                q = Convert.ToInt32(Console.ReadLine());
            } while (!isSimple(q));
            // модуль
            int n = p * q;
            // функция эйлера
            int f_Euler = (p - 1) * (q - 1);
            // открытая экспонента
            int e = getSimple(f_Euler);
            // секретная экспонента
            int d = 1;
            while (d * e % f_Euler != 1)
            {
                d++;
            }
            // публикация ключей
            Console.WriteLine($"Пара ({e}, {n}) - открытый ключ");
            Console.WriteLine($"Пара ({d}, {n}) - закрытый ключ");
            // шифрование
            int[] e_message = ek(message, (e, n));
            foreach (var item in e_message)
            {
                Console.Write(item.ToString() + " ");
            }
            Console.WriteLine();
            // расшифрование
            string d_message = dk(e_message, (d, n));
            Console.WriteLine(d_message);
            Console.ReadKey();
        }

        private static int getSimple(int number, int a_ = 2)
        {
            int a = a_;
            for (int i = 2; i <= number; i++)
            {
                // если имеют общие делители
                if ((number % i == 0) && (a % i == 0))
                {
                    // проверить число, большее на 1
                    a = getSimple(number, a + 1);
                    break;
                }
            }
            return a;
        }

        private static bool isSimple(int number)
        {
            if (number < 2)
            {
                return false;
            }
            if (number == 2)
            {
                return true;
            }

            for (int i = 2; i < number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static int[] ek(string text, (int, int) key)
        {
            int[] result = new int[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                result[i] = Convert.ToInt32((BigInteger.Pow(text[i], key.Item1) % key.Item2).ToString());
            }
            return result;
        }

        private static string dk(int[] text, (int, int) key)
        {
            string result = "";
            foreach (var item in text)
            {
                result += (char)(BigInteger.Pow(item, key.Item1) % key.Item2);
            }
            return result;
        }
    }
}