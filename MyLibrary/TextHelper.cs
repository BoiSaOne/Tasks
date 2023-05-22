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

        private static async Task<Dictionary<string, int>> GetCountUniqueWordsAsync(string pathFile)
        {
            List<string> words = new List<string>();
            Regex regex = new Regex(@"\p{L}+");
            StreamReader? reader = null;

            try
            {
                reader = new(pathFile, Encoding.UTF8);
                string? strLine;
                while ((strLine = await reader.ReadLineAsync()) != null)
                {
                    words.AddRange(regex.Matches(strLine).Select(w => w.Value.ToLower()));
                }
            }
            catch (Exception)
            {
                await Console.Out.WriteLineAsync("Ошибка чтение файла");
            }
            finally
            {
                reader?.Close();
            }

            var dictionaryWords = words.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

            return dictionaryWords;
        }
    }
}