using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Pi
    {
        static double pi_calculate(double eps)
        {
            double res = 4.0,
                tmp_res = 0.0;
            bool minus = true;
            for (uint i = 3; Math.Abs(res - tmp_res) >= eps; i += 2)
            {
                if (i < 0)
                    throw new Exception("invalid value");
                tmp_res = res;
                if (minus)
                {
                    res -= 4.0 / i;
                }
                else
                {
                    res += 4.0 / i;
                }
                minus = !minus;
            }
            return res;
        }

        public static string PiSpigot(int count)
        {
            // найденные цифры сразу же будем записывать в StringBuilder
            StringBuilder pi = new StringBuilder(count);
            int boxes = count * 10 / 3; // размер массива
            int[] reminders = new int[boxes];
            // инициализируем масив двойками
            for (int i = 0; i < boxes; i++)
            {
                reminders[i] = 2;
            }
            int heldDigits = 0;    // счётчик временно недействительных цифр
            for (int i = 0; i < count; i++)
            {
                int carriedOver = 0;    // перенос на следующий шаг
                int sum = 0;
                for (int j = boxes - 1; j >= 0; j--)
                {
                    reminders[j] *= 10;
                    sum = reminders[j] + carriedOver;
                    int quotient = sum / (j * 2 + 1);   // результат деления суммы на знаменатель
                    reminders[j] = sum % (j * 2 + 1);   // остаток от деления суммы на знаменатель
                    carriedOver = quotient * j;   // j - числитель
                }
                reminders[0] = sum % 10;
                int q = sum / 10;   // новая цифра числа Пи
                                    // регулировка недействительных цифр
                if (q == 9)
                {
                    heldDigits++;
                }
                else if (q == 10)
                {
                    q = 0;
                    for (int k = 1; k <= heldDigits; k++)
                    {
                        int replaced = Convert.ToInt32(pi.ToString(i - k, i - k + 1));
                        if (replaced == 9)
                        {
                            replaced = 0;
                        }
                        else
                        {
                            replaced++;
                        }
                        pi.Remove(i - k, 1);
                        pi.Insert(i - k, replaced);
                    }
                    heldDigits = 1;
                }
                else
                {
                    heldDigits = 1;
                }
                pi.Append(q);   // сохраняем найденную цифру
            }
            if (pi.Length >= 2)
            {
                pi.Insert(1, '.');  // добавляем в строчку точку после 3
            }
            return pi.ToString();
        }
    }
}
