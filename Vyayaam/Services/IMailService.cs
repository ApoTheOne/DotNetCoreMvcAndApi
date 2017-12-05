namespace Vyayaam.Services
{
    public interface IMailService
    {
        void SendMessage(string to, string subject, string body);

        void AlertMessage(string to, string subject, string body);
    }
}