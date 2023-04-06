using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;

Console.WriteLine("Введите путь к файлу");
var pathFile = Console.ReadLine();
Console.WriteLine("Введите путь к файлу для сохранения данных");
var pathSaveFile = Console.ReadLine();

if (pathFile != null && pathSaveFile != null)
{
    Console.Clear();
    var dictionaryWords = await GetCountUniqueWordsAsync(pathFile); 
    await WriteInFileAsync(dictionaryWords, pathSaveFile);
    Console.WriteLine("Данные загрузили в файл");
}
Console.ReadKey();

static async Task<Dictionary<string, int>> GetCountUniqueWordsAsync(string pathFile)
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

static async Task WriteInFileAsync(Dictionary<string, int> words, string pathFile)
{
    int numberSpaces = words.Max(w => w.Key.Length);

    foreach (var item in words)
    {
        using (StreamWriter sw = new StreamWriter(pathFile, true))
        {
            await sw.WriteLineAsync($"{item.Key.PadRight(numberSpaces, ' ')} {item.Value}");
        }
    }
}
