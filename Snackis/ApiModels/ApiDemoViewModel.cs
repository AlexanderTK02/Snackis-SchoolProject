namespace Snackis.ApiModels;

public class ApiDemoViewModel
{
    public List<ApiCategory>? Categories { get; set; }
    public List<ApiPost>? Posts { get; set; }
    public int? SelectedCategoryId { get; set; }
    public string ApiBaseUrl { get; set; } = string.Empty;
}
