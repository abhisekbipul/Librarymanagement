namespace LibraryManagementConsuming.Models
{
    public class User
    {
        public int UserId { get; set; }  // Primary Key
        public string FirstName { get; set; }  // User's first name
        public string LastName { get; set; }  // User's last name
        public string Email { get; set; }  // User's email address
        public string PhoneNumber { get; set; }  // User's phone number
        public bool IsActive { get; set; }  // User's status (active/inactive)

        // Navigation property for related LibraryCard
        public LibraryCard LibraryCard { get; set; }  // A user can have one library card

        // Navigation property for books issued by the user
        public ICollection<Book> IssuedBooks { get; set; }  // A
    }
}
