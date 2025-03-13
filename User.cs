using System;

//Class to Maitain Users
[Serializable]
class User{
    //Class Variables
    private int userId;
    private String name;
    private List<Book> booksBorrwed;
    //Constructor
    public User(int userId,String name){
        this.userId=userId;
        this.name=name;
        booksBorrwed=new List<Book>();
    }
    //Getter and Setter for userId
    public int UserId{
        get{return userId;}
        set{userId=value;}
    }
    //Getter and Setter for userId
    public String Name{
        get{return name;}
        set{name=value;}
    }
    //Getter and Setter for bookBorrowed
    public List<Book> GetBooksBorrowed(){
        return booksBorrwed;
    }
    public void addBooksBorrowed(Book book){
        booksBorrwed.Add(book);
    }
    public void removeBooksBorrowed(Book book){
        booksBorrwed.Remove(book);
    }
}