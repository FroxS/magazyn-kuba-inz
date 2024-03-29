﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Models.Attribute;
using Warehouse.Models.Enums;

namespace Warehouse.Models;

[Table("ItemState")]
public class ItemState : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    [FilterColumn]
    public string? Name { get; set; }

    public EState State { get; set; }

    public List<StorageItem>? Items { get; set; }

    public static ItemState Get()
    {
        return new ItemState()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Name = "",
            Lp = 1
        };
    }
}