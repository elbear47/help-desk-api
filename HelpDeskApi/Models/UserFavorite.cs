using System;
using System.Collections.Generic;

namespace HelpDeskApi;

public partial class UserFavorite
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TicketId { get; set; }

    public virtual Ticket? Ticket { get; set; }

    public virtual User? User { get; set; }
}
