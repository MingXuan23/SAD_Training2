using System;
using System.Collections.Generic;

namespace Training2Api.models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
}
