using PostRecommender.Contracts;

namespace PostRecommenders.Models;

internal class Customer
{
    public decimal Wallet { get; set; }
    public PageType Type { get; init; }
    public string Title { get; init; }
    public int FollowerCount { get; set; }
    public readonly List<Post> Posts = [];
    
    public Customer(PageType type, string title, int followerCount)
    {
        Wallet = 1000M;
        Type = type;
        Title = title;
        FollowerCount = followerCount;
    }
    
    public void AddPost(Post post) => Posts.Add(post);
    
}
