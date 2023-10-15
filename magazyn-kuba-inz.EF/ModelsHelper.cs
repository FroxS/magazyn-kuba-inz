using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Models;

namespace Warehouse.EF;

public static class ModelsHelper 
{
    #region Public Methods

    internal static void LoadDefaultEntity<T>(this EntityTypeBuilder<T> mb) where T: BaseEntity
    {
        mb.HasKey(x => x.ID);
        mb.Property(x => x.ID).IsRequired();
    }

    #endregion
}

