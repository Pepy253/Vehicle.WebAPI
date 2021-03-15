using System.Data.Entity.ModelConfiguration;
using Vehicle.Model.Entities;

namespace Vehicle.DAL.Mappings
{
    class VehicleModelMap : EntityTypeConfiguration<VehicleModel>
    {
        public VehicleModelMap()
        {
            HasKey(mo => mo.Id);
            
            Property(mo => mo.Name).HasMaxLength(50);
            Property(mo => mo.Name).IsRequired();
            Property(mo => mo.Abrv).HasMaxLength(50);
            Property(mo => mo.Abrv).IsRequired();

            HasIndex(mo => mo.Name).IsUnique();            
            HasIndex(mo => mo.Abrv).IsUnique();

            ToTable("VehicleModel");
        }
    }
}
