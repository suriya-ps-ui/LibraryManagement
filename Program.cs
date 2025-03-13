using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
class Program{
    //Hashtable to store books
    static Dictionary<String,Book> booksRegister=new Dictionary<string, Book>();
    //Hashtable to store users
    static Dictionary<int,User> userRegister=new Dictionary<int, User>();
    //Files to save current state to carry over the data.
    static String booksFile="books.json";
    static String userFile="users.json";
    //Method to load data
    static void loadData(){
        try{
            if(File.Exists(booksFile)){
                booksRegister=JsonSerializer.Deserialize<Dictionary<string, Book>>(File.ReadAllText(booksFile));
            }
            if(File.Exists(userFile)){
                userRegister=JsonSerializer.Deserialize<Dictionary<int,User>>(File.ReadAllText(userFile));
            }
        }catch(Exception ex){
            System.Console.WriteLine($"Error while Loading:{ex.Message}");
            userRegister=new Dictionary<int,User>();
            booksRegister=new Dictionary<string,Book>();
        }
    }
    //Method to save data
    static void saveData(){
        try{
            String booksJSON=JsonSerializer.Serialize(booksRegister);
            File.WriteAllText(booksFile,booksJSON);
            String userJSON=JsonSerializer.Serialize(userRegister);
            File.WriteAllText(userFile,userJSON);
        }catch(Exception ex){
            System.Console.WriteLine($"Error while saving:{ex.Message}");
        }
    }
    //Method to Create New User.
    public static void createUser(){
        System.Console.WriteLine("Create New user account.");
        System.Console.WriteLine("Enter UserID:");
        int userId=Convert.ToInt32(Console.ReadLine());
        System.Console.WriteLine("Enter Name:");
        String name=Console.ReadLine();
        if(userRegister.ContainsKey(userId)){
            System.Console.WriteLine("There is existing user with same UserID.");
            return;
        }
        User newUser=new User(userId,name);
        userRegister.Add(userId,newUser);
    }
    //Method to Add New Book
    public static void addBook(){
        System.Console.WriteLine("Add new Book.");
        System.Console.WriteLine("Enter BookID:");
        int bookId=Convert.ToInt32(Console.ReadLine());
        System.Console.WriteLine("Enter Book Name:");
        String bookName=Console.ReadLine();
        if(booksRegister.ContainsKey(bookName)){
            return;
        }
        Book newBook=new Book(bookId,bookName);
        booksRegister.Add(bookName,newBook);
    }
    static void Main(string[] args){
        //Load Previous State
        loadData();
        //Variables Used
        bool flag=true;
        int choice,userId;
        String bookName,bookBorrowChoice;
        Book book;
        User user;
        //Loop until user exits
        while(flag){
            System.Console.WriteLine("\nWelcome to the Library Management System:\n1. Add a Book\n2. View Available Books\n3. Borrow a Book\n4. Return a Book\n5. Exit");
            choice=Convert.ToInt32(Console.ReadLine());
            switch(choice){
                case 1:
                    System.Console.WriteLine("To add new Book enter your UserID");
                    userId=Convert.ToInt32(Console.ReadLine());
                    if(!userRegister.ContainsKey(userId)){
                        createUser();
                    }
                    addBook();
                    break;
                case 2:
                    System.Console.WriteLine("Available Books");
                    foreach(var entry in booksRegister){
                        book=(Book)entry.Value;
                        if(book.IsAvailable){
                            System.Console.WriteLine(book.BookName);
                        }
                    }
                    System.Console.WriteLine("Do you need to Borrow any Book's(Y/N):");
                    bookBorrowChoice=Console.ReadLine();
                    if(bookBorrowChoice.ToUpper()=="Y"){
                        goto case 3; 
                    }
                    break;
                case 3:
                    System.Console.WriteLine("Enter your UserID");
                    userId=Convert.ToInt32(Console.ReadLine());
                    if(!userRegister.ContainsKey(userId)){
                        createUser();
                    }
                    BookBorrowName:
                        System.Console.WriteLine("Enter name of the book you like to borrow:");
                        bookName=Console.ReadLine();
                        if(!booksRegister.ContainsKey(bookName)){
                            System.Console.WriteLine("Enter valid Name.");
                            goto BookBorrowName;
                        }
                    book=(Book)booksRegister[bookName];
                    if(!book.IsAvailable){
                        System.Console.WriteLine("Book is not Available.Do you need some other book(Y/N).");
                        bookBorrowChoice=Console.ReadLine();
                        if(bookBorrowChoice.ToUpper()=="Y"){
                            goto BookBorrowName;
                        }else{
                            break;
                        }

                    }
                    book.IsAvailable=false;
                    user=(User)userRegister[userId];
                    user.addBooksBorrowed(book);
                    System.Console.WriteLine($"Book {book.BookName} is borrowed by {user.Name}");
                    break;
                case 4:
                    System.Console.WriteLine("Enter your UserID");
                    userId=Convert.ToInt32(Console.ReadLine());
                    if(!userRegister.ContainsKey(userId)){
                        System.Console.WriteLine("There is no User with this ID.");
                        break;
                    }
                    BookReturnName:
                        System.Console.WriteLine("Enter the name of book to return.");
                        bookName=Console.ReadLine();
                        if(!booksRegister.ContainsKey(bookName)){
                            System.Console.WriteLine("Enter valid Name.");
                            goto BookReturnName;
                        }
                    book=(Book)booksRegister[bookName];
                    user=(User)userRegister[userId];
                    bool bookBorrowCheck=false;
                    foreach(Book borrowedBooks in user.GetBooksBorrowed()){
                        if(book==borrowedBooks){
                            bookBorrowCheck=true;
                            break;
                        }
                    }
                    if(!bookBorrowCheck||book.IsAvailable){
                        System.Console.WriteLine("This is book has not been borrowed by the user.");
                        break;
                    }
                    book.IsAvailable=true;
                    user.removeBooksBorrowed(book);
                    System.Console.WriteLine("Book has been Returned.");
                    break;
                case 5:
                    //Save Data to use next time.
                    saveData();
                    flag=false;
                    break;
                default:
                    System.Console.WriteLine("Wrong Option Enter Again");
                    break;
            }
        }
    }
}