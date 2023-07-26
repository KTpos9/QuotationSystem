namespace QuotationSystem.Data.Repositories
{
    public interface IConfigRepository
    {
        string GetDefaultPassowrd();
        string GetConfigById(string id);
    }
}