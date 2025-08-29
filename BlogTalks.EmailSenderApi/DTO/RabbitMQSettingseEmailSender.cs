namespace BlogTalks.EmailSenderApi.DTO
{
    public class RabbitMQSettingseEmailSender
    {
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangeName { get; set; }
        public string ExchhangeType { get; set; }
        public string QueueName  { get; set; }
        public string RouteKey { get; set; }
    }
}
