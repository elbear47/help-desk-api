using System;
using System.Collections.Generic;

namespace HelpDeskApi;

public partial class Ticket
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string DateSubmitted { get; set; } = null!;

    public string? Priority { get; set; }

    public string? Details { get; set; }

    public string SubmittedBy { get; set; } = null!;

    public int? UserId { get; set; }

    public string? ResolvedBy { get; set; }

    public string? ResolutionNote { get; set; }

    public string Active { get; set; } = null!;

    public string IsBookmarked { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual ICollection<UserFavorite> UserFavorites { get; } = new List<UserFavorite>();
}
