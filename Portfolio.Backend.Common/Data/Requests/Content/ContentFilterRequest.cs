using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common;

public class ContentFilterRequest
{
    [Required]
    public int Page { get; set; }
    [Required]
    public int Size { get; set; }
    public IEnumerable<int>? IdList { get; set; }
    public string? PlaceSearchString { get; set; }
    public string? LocationSearchString { get; set; }
    public string? PayloadSearchString { get; set; }
    public string? CreatedAtSearchString { get; set; }
    public string? UpdatedAtSearchString { get; set; }
}
