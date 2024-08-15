using PostRecommender.Contracts;

namespace PostRecommenders.Models;

public class Follower
{
    private readonly HashSet<Post> recommends = [];
    public readonly HashSet<string> _interests = [];
    public string Address { get;}
    public string Title { get;}
    public PageType LikedPageType { get; set; }

    public void AddInterests(List<string> interests) => _interests.UnionWith(interests);
    public void AddRecommends(List<string> recommends) => _interests.UnionWith(recommends);
    // public void RemoveInterest(string interest) => _interests.Remove(interest);

    public Follower(string address, string title)
    {
        Address = address;
        Title = title;
    }
}