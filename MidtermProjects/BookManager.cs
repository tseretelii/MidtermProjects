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
            Title = title.ToLower();
            Author = author.ToLower();
            ReleaseYear = releaseYear;
        }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public DateOnly ReleaseYear { get; private set; }
    }
    public static class BookManager
    {
        public static List<Book> Books { get; private set; }
        private static string _path = "C:\\Users\\User\\Desktop\\BookFolder";
        private static DirectoryInfo _directory = new DirectoryInfo(_path);

        public static void UserInterface()
        {
            Console.WriteLine("Wellcome To The Book Manager\nHere you can:");
            while (true)
            {
                Console.WriteLine("\n1. Add a book\n2. Search a books by title\n3. Get all books\n4. exit\n(enter the digit)");
                int choice = ValidateInput<int>(errorMessage:"Invalid Input Try Again", range:5);

                if (choice == 1)
                {
                    Console.Write("\nPleas provide this information: ");
                    Console.Write("\nTitle - ");
                    string title = ValidateInput<string>("Must Not Be Empty Try Again");

                    Console.Write("\nAuthor - ");
                    string author = ValidateInput<string>("Must Not Be Empty Try Again");

                    Console.Write("\nDate of publication (enter in this format \"YYYY-MM-DD\") - ");
                    DateOnly dateOnly = ValidateInput<DateOnly>("Invalid Input Try Again");
                    AddBook(title, author,dateOnly);
                }
                else if (choice == 2)
                {
                    Console.Write("Title - ");
                    string title = ValidateInput<string>("Must Not Be Empty Try Again");
                    GetBookByName(title);
                }
                else if (choice == 3)
                    GetAllBooks();
                else
                    break;
            }
        }
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
            bool condition = false;
            FileInfo[] fileInfos = _directory.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.Contains(title.ToLower()))
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        using (StreamReader streamReader = new StreamReader(fs))
                        {
                            condition = true;
                            Book x = JsonSerializer.Deserialize<Book>(streamReader.ReadToEnd());
                            Console.WriteLine($"\"{x.Title}\" By {x.Author}, Published in {x.ReleaseYear}");
                        }
                    }
            }
            if ( !condition )
                Console.WriteLine("No Books Found");
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

        private static T ValidateInput<T>(string errorMessage, int range = 0)
        {
            if (typeof(T) == typeof(int))
            {
                int value;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out value) && value > 0 && value < range)
                        return (T)(object)value;
                    Console.WriteLine(errorMessage);
                }
            }
            else if (typeof(T) == typeof(string))
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                        return (T)(object)input;
                    Console.WriteLine(errorMessage);
                }
            }
            else if(typeof(T) == typeof(DateOnly))
            {
                DateOnly dateOnly = new DateOnly();
                while (true)
                {
                    if (DateOnly.TryParse(Console.ReadLine(), out dateOnly))
                        return(T)(object)dateOnly;
                    Console.WriteLine(errorMessage);
                }
            }
            else
            {
                return (T)(object)"invalid type";
            }
        }
    }
}
