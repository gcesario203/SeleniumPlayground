namespace Application.Crawler.Contracts
{
    public interface ICrawlerDriver
    {
        public void Crawl();

        public void Dispose();
    }
}