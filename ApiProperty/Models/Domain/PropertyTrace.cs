using System;
using System.Collections.Generic;

namespace ApiProperty.Models.Domain;

public partial class PropertyTrace
{
    public int IdPropertyTrace { get; set; }

    public DateTime? DataSale { get; set; }

    public string? Name { get; set; }

    public decimal? Value { get; set; }

    public decimal? Tax { get; set; }

    public int? IdProperty { get; set; }

    public virtual Property? IdPropertyNavigation { get; set; }
}
