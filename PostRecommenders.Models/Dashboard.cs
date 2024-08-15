using PostRecommender.Contracts;

namespace PostRecommenders.Models;

public class Dashboard : IInstagramPageService
{
    private decimal _balance = 0;
    private readonly List<Post> _posts = [];
    private readonly List<Customer> _customers = [];
    private readonly List<Follower> _followers = [];
    private readonly List<Recommendation> _recommendations = [];

    public void RegisterCustomerPage(RegisterCustomerPageDto customerPageDto) =>
        _customers.Add(new Customer(customerPageDto.PageType, customerPageDto.Title, customerPageDto.FollowerCount));

    public List<ShowCustomerPageDto> ShowCustomersPage() => _customers.Select((_, index) => new ShowCustomerPageDto
    {
        Id = index + 1,
        Title = _.Title,
        PageType = _.Type,
        FollowerCount = _.FollowerCount,
        WalletBalance = _.Wallet,
    }).ToList();

    public void UpdateCustomerFollowerCount(UpdateFollowerCountDto followerCountDto) =>
        _customers[followerCountDto.CustomerId - 1].FollowerCount = followerCountDto.NewFollowerCount;

    public void RechargeCustomerWallet(WalletRechargeDto walletRechargeDto) =>
        _customers[walletRechargeDto.CustomerId - 1].Wallet += walletRechargeDto.Amount;

    public void RegisterFollower(RegisterFollowerDto followerDto) =>
        _followers.Add(new Follower(followerDto.PageAddress, followerDto.Title));

    public List<ShowFollowerDto> ShowFollowers() => _followers.Select((_, index) => new ShowFollowerDto
    {
        FollowerId = index + 1,
        PageAddress = _.Address,
        Title = _.Title
    }).ToList();


    public void RegisterFollowerLikedPost(RegisterFollowerLikedPostDto followerLikedPostDto)
    {
        _followers[followerLikedPostDto.FollowerId - 1].AddInterests(followerLikedPostDto.PostHashtags);
        _followers[followerLikedPostDto.FollowerId - 1].LikedPageType |= followerLikedPostDto.LikedPageType;
    }

    public void RegisterCustomerPost(RegisterCustomerPostDto customerPostDto)
    {
        if (customerPostDto.LikeCount < 5) throw new Exception("Likes should be 5 or more");
        if (customerPostDto.Hashtags.Count == 0) throw new Exception("Post should have a tag or more");
        _customers[customerPostDto.CustomerId - 1].AddPost(new Post(customerPostDto.LikeCount,
            customerPostDto.PostAddress, customerPostDto.Hashtags));
    }

    public void RecommendCustomerPosts(RecommendCustomerPostsDto recommendCustomerPostsDto)
    {
        var customer = _customers[recommendCustomerPostsDto.CustomerId - 1];
        decimal payPerRecommend = customer.Type == PageType.Personal ? 10 : 100;
        _followers.ForEach(follower =>
        {
            if (follower.LikedPageType != customer.Type) return;
            var posts = customer.Posts.Where(p => p._tags.Any(c => follower._interests.ToList().Contains(c))).ToList();
            posts.ForEach(post =>
            {
                if (customer.Wallet >= payPerRecommend)
                {
                    _recommendations.Add(new Recommendation
                    {
                        Follower = follower,
                        Post = post,
                    });
                    customer.Wallet -= payPerRecommend;
                    _balance += payPerRecommend;
                }
            });
        });
    }

    public List<RecommendationDto> ShowCustomerRecommendations(RecommendationRequestDto recommendationRequestDto) =>
        _customers[recommendationRequestDto.CustomerId - 1].Posts.Select(_ => new RecommendationDto()
        {
            Hashtags = _._tags
        }).ToList();

    // var customer = _customers[recommendationRequestDto.CustomerId - 1];
    // var dto = new List<RecommendationDto>();
    // customer.Posts.ForEach(post => dto.Add(
    //     new RecommendationDto()
    //     {
    //         Hashtags = post._tags
    //     }
    //     ));
    // return dto;

    public ShowTotalIncomeDto ShowTotalIncome()
    {
        return new ShowTotalIncomeDto
        {
            TotalIncome = _balance
        };
    }
}