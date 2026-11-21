namespace VetClinic.Web.Models.Entities;

/// <summary>
/// Personal access token for the MCP endpoint. The plaintext token is shown to the user once
/// and never stored; only a keyed HMAC-SHA256 hash (32 bytes) is persisted.
/// </summary>
public class McpAccessToken
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string TokenPrefix { get; set; } = string.Empty;
    public byte[] TokenHash { get; set; } = [];
    public string Scopes { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? LastUsedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
}
