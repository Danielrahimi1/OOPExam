namespace PostRecommenders.Models;

internal class Post
{
    public readonly List<string> _tags = [];
    public string Url { get; set; } = "";
    public int LikeCount { get; set; } = 0;

    public void AddTag(List<string> tags) => _tags.AddRange(tags);

    public Post(int likeCount, string url, List<string> tags)
    {
        LikeCount = likeCount;
        Url = url;
        _tags.AddRange(tags);
    }
}