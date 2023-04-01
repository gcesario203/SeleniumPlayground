namespace Lib.Crawler.Contracts
{
    public interface ICrawlerDriver
    {
        public void Crawl();

        public void Dispose();
    }
}