namespace LibraryManagementConsuming.Models
{
    public class LibraryCard
    {
        public int LibraryCardId { get; set; }
        public string CardNumber { get; set; }
        public int UserId { get; set; }  // Foreign key to the User table
        public DateTime IssuedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation property for the user
        public User User { get; set; }
    }
}
