using Microsoft.AspNetCore.Mvc;

namespace MiniMES.Controllers
{
    
    [Route("/Library")]
    public class LibraryController : Controller
    {
        private static List<Book> _books = new List<Book>();
        private static List<User> _users = new List<User>();

        static LibraryController()
        {
            AddBooks();
            AddUsers();
        }

        [HttpGet]
        [Route("/ListBooks")]
        public IActionResult ListBooks()
        {
            if (_books.Count == 0)
            {
                return NoContent();
            }

            string bookList = "";
            foreach (Book book in _books)
            {
                string bookData = $"{book.Id}. {book.Title}.   author: {book.Author}.   availablity: {(book.IsAvailable ? "" : "not")} available";
                bookList += bookData + "\n";
            }
            return Content(bookList);
        }

        [HttpGet]
        [Route("/BookDetails/{id}")]
        public IActionResult BookDetails([FromRoute] int id)
        {
            Book? book = FindBook(id);

            if (book == null)
            {
                return NotFound();
            }

            string bookData = $"{book.Id}. {book.Title}.   author: {book.Author}.   availablity: {(book.IsAvailable ? "" : "not")} available";
            return Content(bookData);

        }

        [HttpPost]
        [Route("/AddBook")]
        public IActionResult AddBook([FromBody] Book book)
        {
            IActionResult? validationIssue = Validate(book);
            if (validationIssue != null)
            {
                return validationIssue;
            }
            
            _books.Add(book);
            Console.WriteLine($"Added book: {book.Title}");
            return Created();
        }

        [HttpPut]
        [Route("/UpdateBook/{id}")]
        public IActionResult UpdateBook([FromRoute] int id, [FromBody] Book updatedBook)
        {
            Book? book = FindBook(id);
            if (book == null)
            {
                Console.WriteLine("No book with this id");
                return NotFound();
            }
            //allows updating to the id a book had before an update
            bool updateToSameId = updatedBook.Id == id;
            IActionResult? validationIssue = Validate(updatedBook,updateToSameId);
            if (validationIssue != null)
            {
                return validationIssue;
            }

            book.Id = updatedBook.Id;
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.IsAvailable = updatedBook.IsAvailable;

            Console.WriteLine($"Book updated to {updatedBook.Title}");
            return Ok("book updated");
        }

        [HttpDelete]
        [Route("/DeleteBook/{id}")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            if (FindBook(id) == null)
            {
                return NotFound();
            }
            
            int i = 0;
            foreach (Book book in _books)
            {
                if (book.Id == id)
                {
                    _books.RemoveAt(i);
                    break;
                }
                i++;
            }
            return Ok("book deleted");
        }

        [HttpPut]
        [Route("/BorrowBook")]
        public IActionResult BorrowBook([FromQuery] int userId, [FromQuery] int bookId)
        {
            Book? book = FindBook(bookId);
            if (book==null)
            {
                Console.WriteLine("not a valid book");
                return NotFound();
            }
            User? user = FindUser(userId);
            if (user == null)
            {
                Console.WriteLine("not a valid user");
                return NotFound();
            }

            if (!book.IsAvailable)
            {
                Console.WriteLine("book is not available");
                return Conflict();
            }
            
            user.BorrowedBooks.Add(book);
            book.IsAvailable = false;
            return Ok($"user {user.Name} borrowed {book.Title}");
        }
        
        [HttpPut]
        [Route("/ReturnBook")]
        public IActionResult ReturnBook([FromQuery] int userId, [FromQuery] int bookId)
        {
            Book? book = FindBook(bookId);
            if (book==null)
            {
                Console.WriteLine("not a valid book");
                return NotFound();
            }
            User? user = FindUser(userId);
            if (user == null)
            {
                Console.WriteLine("not a valid user");
                return NotFound();
            }

            if (!user.BorrowedBooks.Contains(book) )
            {
                Console.WriteLine($"user {user.Name} {user.Surname} does not have this book");
                return Conflict();
            }

            user.BorrowedBooks.Remove(book);
            book.IsAvailable = true; 
            Console.WriteLine($"{user.Name} returned book {book.Title}");
            return Ok($"user {user.Name} returned {book.Title}");
        }
        
        // returns null if book is valid
        private IActionResult? Validate(Book book, bool ignoreOverlappingIds = false)
        {
            if (book.Id < 1)
            {
                Console.WriteLine("Inapropriate ID");
                return BadRequest();
            }

            if (FindBook(book.Id) != null && !ignoreOverlappingIds)
            {
                Console.WriteLine("Book with this id already exists.");
                return Conflict();
            }
            
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                Console.WriteLine("Empty Title");
                return BadRequest();
            }
            
            foreach (char c in book.Title)
            {
                if (char.IsSymbol(c))
                {
                    Console.WriteLine("Title cant contain symbols");
                    return BadRequest();
                }
            }

            if (string.IsNullOrWhiteSpace(book.Author))
            {
                Console.WriteLine("Empty Author");
                return BadRequest();
            }

            foreach (char c in book.Author)
            {
                if (char.IsSymbol(c))
                {
                    Console.WriteLine("Author name cant contain symbols");
                    return BadRequest();
                }
            }
            return null;
        }

        //returns null if book does not exist
        private static Book? FindBook(int id)
        {
            Book? book = null;
            foreach (Book b in _books)
            {
                if (b.Id == id)
                {
                    book = b;
                    break;
                }
            }
            return book;
        }

        private static User? FindUser(int id)
        {
            User? user = null;
            foreach (User b in _users)
            {
                if (b.Id == id)
                {
                    user = b;
                    break;
                }
            }
            return user;
        }
        private static void AddBooks()
        {
            _books.Add(new Book(1, "1984", "George Orwell", true));
            _books.Add(new Book(2, "To Kill a Mockingbird", "Harper Lee", true));
            _books.Add(new Book(3, "The Great Gatsby", "F. Scott Fitzgerald", true));
            _books.Add(new Book(4, "Moby Dick", "Herman Melville", true));
        }
        private static void AddUsers()
        {
            _users.Add(new User(1, "John", "Doe"));
            _users.Add(new User(2, "Jane", "Smith"));
            _users.Add(new User(3, "Sam", "Brown"));
        }
    }
    

    public class Book
    {
        private int id;
        private string title;
        private string author;
        private bool isAvailable;

        public Book(int id, string title, string author, bool isAvailable)
        {
            Title = title;
            Id = id;
            Author = author;
            IsAvailable = isAvailable;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value ?? ""; }
        }
        public string Author
        {
            get { return author; }
            set { author = value ?? ""; }
        }
        public bool IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; }
        }
    }

    public class User
    {
        private int id;
        private string name;
        private string surname;
        private List<Book> borrowedBooks;
        
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set {name = value ?? ""; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value ?? ""; }
        }

        public List<Book> BorrowedBooks
        {
            get { return borrowedBooks; }
            set { borrowedBooks = value; }
        }
        
        public User(int id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
            BorrowedBooks = new List<Book>();
        }
    }
}