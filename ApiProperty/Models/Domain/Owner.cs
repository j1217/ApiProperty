using System;
using System.Collections.Generic;

namespace ApiProperty.Models.Domain;

public partial class Owner
{
    public int IdOwner { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public byte[]? Photo { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
