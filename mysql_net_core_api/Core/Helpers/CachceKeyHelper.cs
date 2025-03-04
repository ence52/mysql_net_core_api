namespace mysql_net_core_api.Core.Helpers
{
    public static class CachceKeyHelper
    {
        public static string GetCacheKey(string keyPrefix,params object[] parameters)
        {
            var paramsString = string.Join("_", parameters.Select(p=>p.ToString()));
            return $"{keyPrefix}_{paramsString}";
        }
    }
}
