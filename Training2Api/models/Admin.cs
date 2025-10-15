using System;
using System.Collections.Generic;

namespace Training2Api.models;

public partial class Admin
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }
}
