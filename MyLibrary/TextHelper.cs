using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MyLibrary
{
    public static class TextHelper
    {

        private static Dictionary<string, int> GetCountUniqueWords(string text)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Regex regex = new Regex(@"\p{L}+");
            List<string> words = new List<string>();

            words.AddRange(regex.Matches(text).Select(w => w.Value.ToLower()));
            var dictionaryWords = words.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);
            
            stopwatch.Stop();
            Console.WriteLine($"Method execution time \"{nameof(GetCountUniqueWords)}\": {stopwatch.ElapsedMilliseconds} ms");

            return dictionaryWords ?? new Dictionary<string, int>();
        }

        public static Dictionary<string, int> GetCountUniqueWordsThread(string text)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
           
            object locker = new();
            Regex regex = new Regex(@"\p{L}+");
            List<string> list = new();
            
            var textLines = text.Split(Environment.NewLine);
            
            foreach (string line in textLines)
            {
                Thread thread = new Thread(item =>
                {
                    if (item is string line)
                    {
                        List<string> words = new();
                        words.AddRange(regex.Matches(line).Select(w => w.Value.ToLower()));
                        lock (locker)
                        {
                            list.AddRange(words);
                        }
                    }
                });
                thread.Start(line);
                thread.Join();
            }

            var dictionaryWords = list.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

            stopwatch.Stop();
            Console.WriteLine($"Method execution time \"{nameof(GetCountUniqueWordsThread)}\": {stopwatch.ElapsedMilliseconds} ms");

            return dictionaryWords ?? new Dictionary<string, int>();
        }

        public static Dictionary<string, int> GetCountUniqueWordsTask(string text)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            object locker = new();
            Regex regex = new Regex(@"\p{L}+");
            List<string> list = new();
            List<Task> tasks = new();

            var textLines = text.Split(Environment.NewLine);

            foreach (string line in textLines)
            {
                tasks.Add(new Task(() =>
                {
                    List<string> words = new();
                    words.AddRange(regex.Matches(line).Select(w => w.Value.ToLower()));
                    lock (locker)
                    {
                        list.AddRange(words);
                    }
                }));
                tasks[tasks.Count - 1].Start();
            }

            Task.WaitAll(tasks.ToArray());
            var dictionaryWords = list.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

            stopwatch.Stop();
            Console.WriteLine($"Method execution time \"{nameof(GetCountUniqueWordsTask)}\": {stopwatch.ElapsedMilliseconds} ms");

            return dictionaryWords ?? new Dictionary<string, int>();
        }

        public static Dictionary<string, int> GetCountUniqueWordsParallel(string text)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            object locker = new();
            Regex regex = new Regex(@"\p{L}+");
            List<string> list = new();

            var textLines = text.Split(Environment.NewLine);

            Parallel.ForEach(textLines, (string line) =>
            {
                List<string> words = new();
                words.AddRange(regex.Matches(line).Select(w => w.Value.ToLower()));
                lock (locker)
                {
                    list.AddRange(words);
                }
            });
            var dictionaryWords = list.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

            stopwatch.Stop();
            Console.WriteLine($"Method execution time \"{nameof(GetCountUniqueWordsParallel)}\": {stopwatch.ElapsedMilliseconds} ms");

            return dictionaryWords ?? new Dictionary<string, int>();
        }

        public static Dictionary<string, int> GetCountUniqueWordsThreadPool(string text)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            object locker = new();
            Regex regex = new Regex(@"\p{L}+");
            List<string> list = new();

            var textLines = text.Split(Environment.NewLine);
            using (var countdownEvent = new CountdownEvent(textLines.Length))
            {
                foreach (var line in textLines)
                {
                    ThreadPool.QueueUserWorkItem((state) =>
                    {
                        List<string> words = new();
                        words.AddRange(regex.Matches(line).Select(w => w.Value.ToLower()));
                        lock (locker)
                        {
                            list.AddRange(words);
                        }
                        countdownEvent.Signal();
                    });
                }
                countdownEvent.Wait();
            }

            var dictionaryWords = list.GroupBy(m => m).ToDictionary(w => w.Key, c => c.Count())
                .OrderByDescending(m => m.Value).ToDictionary(m => m.Key, m => m.Value);

            stopwatch.Stop();
            Console.WriteLine($"Method execution time \"{nameof(GetCountUniqueWordsThreadPool)}\": {stopwatch.ElapsedMilliseconds} ms");

            return dictionaryWords ?? new Dictionary<string, int>();
        }
    }
}