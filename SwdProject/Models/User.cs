﻿using System;
using System.Collections.Generic;

namespace SwdProject.Models;

public partial class User
{
    public long Id { get; set; }

    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public long? RoleId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
