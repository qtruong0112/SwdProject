using System;
using System.Collections.Generic;

namespace SwdProject.Models;

public partial class Cart
{
    public long Id { get; set; }

    public int Sum { get; set; }

    public long? UserId { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual User? User { get; set; }
}
