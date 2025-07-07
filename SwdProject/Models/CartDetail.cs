using System;
using System.Collections.Generic;

namespace SwdProject.Models;

public partial class CartDetail
{
    public long Id { get; set; }

    public double Price { get; set; }

    public long Quantity { get; set; }

    public long? CartId { get; set; }

    public long? ProductId { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Product? Product { get; set; }
}
