namespace Eleicoes.Constants
{
    public static class ConfigConstants
    {
        public const string CRAWL_CITY_URL = "https://resultados.tse.jus.br/oficial/app/index.html#/eleicao;e=e545/dados-de-urna/boletim-de-urna";
        public const string CRAWL_URL_TEMPLATE = "https://resultados.tse.jus.br/oficial/app/index.html#/eleicao;e=e545;uf={UF};ufbu={UF};mubu={MU};zn={ZN};se={SE}/dados-de-urna/rdv";
        public const string DRIVER_PATH = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application";
    }
}