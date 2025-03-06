using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Utils
{
    public class Utils
    {
        public static string RemoveLastWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            string[] words = text.Trim().Split(' ');
            if (words.Length <= 1)
            {
                return string.Empty;
            }

            return string.Join(' ', words, 0, words.Length - 1);
        }
    }
}