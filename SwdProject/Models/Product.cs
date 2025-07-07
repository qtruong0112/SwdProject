using System;
using System.Collections.Generic;

namespace SwdProject.Models;

public partial class Product
{
    public long Id { get; set; }

    public string DetailDesc { get; set; } = null!;

    public string? Factory { get; set; }

    public string? Image { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public long Quantity { get; set; }

    public string ShortDesc { get; set; } = null!;

    public long Sold { get; set; }

    public string? Target { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
