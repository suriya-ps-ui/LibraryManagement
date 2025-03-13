using System;

//Book Class to maitain book's
[Serializable]
class Book{
    //Class Variables
    private int bookId;
    private String bookName;
    private bool isAvailable;

    //Constructor
    public Book(int bookId,String bookName){
        this.bookId=bookId;
        this.bookName=bookName;
        isAvailable=true;
    }
    //Setters and Getters for bookID
    public int BookId{
        get{return bookId;}
        set{bookId=value;}
    }
    //Setters and Getters for bookName
    public String BookName{
        get{return bookName;}
        set{bookName=value;}
    }
    //Setters and Getters for isAvailable
    public bool IsAvailable{
        get{return isAvailable;}
        set{isAvailable=value;}
    }
}