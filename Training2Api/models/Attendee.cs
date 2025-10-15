using System;
using System.Collections.Generic;

namespace Training2Api.models;

public partial class Attendee
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string TicketType { get; set; } = null!;

    public DateTime RegisteredAtUtc { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}
