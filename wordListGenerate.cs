using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class wordListGenerate
    {
        private List<char> Chars { get; set; }
        private int MinLength { get; set; }
        private int MaxLength { get; set; }

        public wordListGenerate(List<char> chars, int min, int max)
        {
            this.Chars = chars;
            this.MinLength = min;
            this.MaxLength = max;
        }

        public wordListGenerate(char[] chars, int min, int max)
        {
            Chars = new List<char>();
            foreach (var ch in chars)
                Chars.Add(ch);
            MinLength = min;
            MaxLength = max;
        }

        private void Replace(ref string s, int position, int charPosition) => Replace(ref s, position, Chars[charPosition]);
        private void Replace(ref string s, int position, char ch) => Replace(ref s, position, ch.ToString());
        private void Replace(ref string s, int position, string ch)
        {
            try
            {
                s = s.Remove(position, 1).Insert(position, ch);
                Console.WriteLine(s);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Merge(ref string s, int pos, char symbol, int counter)
        {
            if (pos > Chars.Count-1)
                pos--;
            if (Chars.IndexOf(symbol) == -1)
                throw new Exception("The symbol " + symbol + " is not contained in chars list.");
            else if(counter++ == Chars.Count - 1)
            {
                counter = 0;
                return;
            }
            Replace(ref s, pos, symbol);

        }

        public void Generate()
        {
            string s = "";
            for (int i = 0; i < MinLength; i++)
                s += Chars[0];
            //Merge(ref s, s.Length - 1, Chars[Chars.Count-1], 0);
            foreach (var ch in Chars)
                Merge(ref s, s.Length - 1, ch, 0);
        }
    }
}
