namespace BlogRazor.Constants
{
    /// <summary>
    /// Api urls constants.
    /// </summary>
    public class ApiUrls
    {
#if DEBUG        
        /// <summary>
        /// The shop API URL.
        /// </summary>
        public const string BlogApiUrl = "http://localhost:54676";
#else
        public const string BlogApiUrl = "http://localhost:54676";
#endif
    }
}
