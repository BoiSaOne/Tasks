using System.Reflection;

const string DLL_NAME = "MyLibrary.dll";
const string TYPE_NAME = "MyLibrary.TextHelper";
const string METHOD_NAME = "GetCountUniqueWords";

Console.WriteLine("Enter the path to the file");
var pathFile = Console.ReadLine();

try
{
    Assembly assembly = Assembly.LoadFrom(DLL_NAME);
    var textHelperType = assembly.GetType(TYPE_NAME, true, true);
    var methodInfo = textHelperType?.GetMethod(METHOD_NAME, BindingFlags.NonPublic | BindingFlags.Static);

    if (methodInfo != null)
    {
        string text = await GetTextFromFileAsync(pathFile);

        Console.WriteLine("Enter the path to the file to save the data");
        var pathSaveFile = Console.ReadLine();

        if (pathSaveFile != null)
        {
            Console.Clear();
            var dictionaryWordsObject = methodInfo.Invoke(null, new object[] { text });
            if (dictionaryWordsObject is Dictionary<string, int> dictionaryWords)
            {
                await WriteInFileAsync(dictionaryWords, pathSaveFile);
                Console.WriteLine("The data was uploaded to a file");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.ReadKey();

static async Task<string> GetTextFromFileAsync(string? pathFile)
{
    if (pathFile == null)
    {
        throw new ArgumentNullException(nameof(pathFile));
    }

    using (StreamReader reader = new StreamReader(pathFile))
    {
        return await reader.ReadToEndAsync();
    }
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
