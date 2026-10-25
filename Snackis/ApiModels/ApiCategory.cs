namespace Snackis.ApiModels;

public class ApiCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
}
