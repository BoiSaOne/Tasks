using System.Text.RegularExpressions;
using System.Text;

namespace MyLibrary
{
    public static class TextHelper
    {

        private static Dictionary<string, int> GetCountUniqueWords(string text)
        {
            Regex regex = new Regex(@"\p{L}+");
            List<string> words = new List<string>();

            words.AddRange(regex.Matches(text).Select(w => w.Value.ToLower()));
            var dictionaryWords = words.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

            return dictionaryWords ?? new Dictionary<string, int>();
        }

        public static Dictionary<string, int> GetCountUniqueWordsThread(string text)
        {
            Regex regex = new Regex(@"\p{L}+");
            List<string> words = new List<string>();

            words.AddRange(regex.Matches(text).Select(w => w.Value.ToLower()));
            var dictionaryWords = words.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

            return dictionaryWords ?? new Dictionary<string, int>();
        }
    }
}