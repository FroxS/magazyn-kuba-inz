﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Models.Attribute;

namespace Warehouse.Models;

[Table("Supplier")]
public class Supplier : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    [FilterColumn]
    public string? Name { get; set; }

    public List<Product>? Products { get; set; }

    public static Supplier Get()
    {
        return new Supplier()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Name = "",
            Lp = 1
        };
    }
}