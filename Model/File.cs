namespace FinancniInformacniSystemBanky.Model
{
    public class File
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Note { get; set; }
        public FileOwner Owner { get; set; } = new FileOwner(); 
        public string FormattedUploadDate => UploadDate.ToString("dd.MM. yyyy");

        public class FileOwner
        {
            public int OwnerId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string DisplayOwnerInfo => $"{Name} {Surname} ({Role})";
        }

    }
        public class PossibleFileOwner
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string NationalIdNumber{ get; set; }
            public string DisplayPossibleOwnerInfo => $"{Name} {Surname} (r.č.: {NationalIdNumber})";
    }
}