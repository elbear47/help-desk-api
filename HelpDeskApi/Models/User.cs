using System;
using System.Collections.Generic;

namespace HelpDeskApi;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();

    public virtual ICollection<UserFavorite> UserFavorites { get; } = new List<UserFavorite>();
}
