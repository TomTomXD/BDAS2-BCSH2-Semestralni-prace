namespace FinancniInformacniSystemBanky.Model
{
    public class File
    {
        public int FileId { get; set; }
        public string FileName{ get; set; }
        public DateOnly UploadDate{ get; set; }
        public string Note { get; set; }
        public int OwnerId { get; set; }

    }
}
