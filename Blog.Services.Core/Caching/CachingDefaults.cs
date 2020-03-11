namespace Blog.Services.Core.Caching
{
    public static class CachingDefaults
    {
        public static int CacheTime => 60;
        public static string RedisDataProtectionKey => "Blog.DataProtectionKeys";
    }
}
