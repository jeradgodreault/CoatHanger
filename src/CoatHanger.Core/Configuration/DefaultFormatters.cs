namespace CoatHanger.Core.Configuration
{

    /// <summary>
    /// Controls how the author appears in the coathanger file. 
    /// </summary>
    public interface IAuthorFormatter
    {
        string GetFormattedAuthor(string username);
    }

    public class DefaultAuthorFormatter : IAuthorFormatter
    {
        public string GetFormattedAuthor(string username) => username;
    }

    /// <summary>
    /// Controls how the release version appears in the coathanger file. 
    /// </summary>
    public interface IReleaseVersionFormatter
    {
        string GetFormattedVersion(string version);
    }

    public class DefaultReleaseVersionFormatter : IReleaseVersionFormatter
    {
        public string GetFormattedVersion(string version) => version;
    }

}
