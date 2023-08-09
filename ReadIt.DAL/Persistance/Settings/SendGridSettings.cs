namespace ReadIt.DAL.Persistance.Settings
{
    public record SendGridSettings
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string EmailName { get; set; }
    }
}
