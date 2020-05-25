namespace Application
{
    public class PolSystem
    {
        public string ApiUrl { get; }
        public string AppUrl { get; }

        public PolSystem(string apiUrl, string appUrl)
        {
            ApiUrl = apiUrl;
            AppUrl = appUrl;
        }
    }
}