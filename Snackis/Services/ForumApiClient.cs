using Snackis.ApiModels;
using System.Net.Http.Json;

namespace Snackis.Services;

public class ForumApiClient : IForumApiClient
{
    private readonly HttpClient _http;

    public ForumApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ApiCategory>?> GetCategoriesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<ApiCategory>>($"api/categories");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<List<ApiPost>?> GetPostsAsync(int categoryId)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<ApiPost>>($"api/categories/{categoryId}/posts");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<List<ApiPost>?> GetAllPostsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<ApiPost>>("api/posts");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
