using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace magazyn_kuba_inz.EF;

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

