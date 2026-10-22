using System.ComponentModel.DataAnnotations.Schema;

namespace Snackis.Api.Models;

[Table("AspNetUsers")]
public class ForumUser
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
}