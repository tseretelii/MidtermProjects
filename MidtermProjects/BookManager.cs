using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public class Book
    {
        public Book(string title, string author, DateOnly releaseYear)
        {
            Title = title;
            Author = author;
            ReleaseYear = releaseYear;
        }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public DateOnly ReleaseYear { get; private set; }
    }
    public class BookManager
    {
        public static List<Book> Books { get; private set; }
        private static string _path = "C:\\Users\\User\\Desktop\\BookFolder";
        private static DirectoryInfo _directory = new DirectoryInfo(_path);

        public static void AddBook(string title, string author, DateOnly releaseYear)
        {
            SaveToFile(new Book(title, author, releaseYear));
        }

        public static void GetAllBooks()
        {
            Books = new List<Book>();
            FileInfo[] fileInfos = _directory.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                using (FileStream fs = fileInfo.OpenRead())
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        Books.Add(JsonSerializer.Deserialize<Book>(sr.ReadToEnd()));
                    }
                }
            }
            Books.ForEach(x => Console.WriteLine($"\"{x.Title}\" By {x.Author}, Published in {x.ReleaseYear}"));
        }

        public static void GetBookByName(string title)
        {
            FileInfo[] fileInfos = _directory.GetFiles($"{title}.txt");
            if (fileInfos.Count() < 1)
            {
                Console.WriteLine($"No book found titled \"{title}\"");
                return;
            }
            foreach (FileInfo fileInfo in fileInfos)
            {
                using (FileStream fs = fileInfo.OpenRead())
                {
                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        Book x = JsonSerializer.Deserialize<Book>(streamReader.ReadToEnd());
                        Console.WriteLine($"\"{x.Title}\" By {x.Author}, Published in {x.ReleaseYear}");
                    }
                }
            }
        }

        private static void SaveToFile(Book book)
        {
            if (!_directory.Exists)
            {
                _directory.Create();
            }
            string filePath = _directory.FullName + "\\" + $"{book.Title}.txt";

            if (_directory.GetFiles($"{book.Title} - {book.Author}.txt").Count() > 0)
            {
                Console.WriteLine("This book is allready added");
                return;
            }
            else if (_directory.GetFiles($"{book.Title}.txt").Count() > 0)
            {
                Console.WriteLine("Book With this title allready exists so we have to add the author to the file name as well");
                filePath = _directory.FullName + "\\" + $"{book.Title} - {book.Author}.txt";
            }

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    string serializedBook = JsonSerializer.Serialize(book);
                    streamWriter.WriteLine(serializedBook);
                    Console.WriteLine("Book Added!");
                }
            }
        }
    }
}
