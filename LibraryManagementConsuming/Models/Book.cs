namespace LibraryManagementConsuming.Models
{
    public class Book
    {
        public int BookId { get; set; }  // Primary key
        public string Title { get; set; }
        public string Author { get; set; }

        // Foreign key to User table (for issued user)
        public int? IssuedToUserId { get; set; }  // Nullable because book may not be issued

        // Navigation property for the user
        public User IssuedToUser { get; set; }  // Navigation to the User who issued the book

        // Issued status
        public bool IsIssued { get; set; }  
    }
}
