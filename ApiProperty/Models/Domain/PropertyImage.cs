using System;
using System.Collections.Generic;

namespace ApiProperty.Models.Domain;

public partial class PropertyImage
{
    public int IdPropertyImage { get; set; }

    public int? IdProperty { get; set; }

    public byte[]? FileProperty { get; set; }

    public bool? Enable { get; set; }

    public virtual Property? IdPropertyNavigation { get; set; }
}
