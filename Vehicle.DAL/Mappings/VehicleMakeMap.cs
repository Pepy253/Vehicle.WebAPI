using System.Data.Entity.ModelConfiguration;
using Vehicle.Model.Entities;

namespace Vehicle.DAL.Mappings
{
    public class VehicleMakeMap : EntityTypeConfiguration<VehicleMake>
    {
        public VehicleMakeMap()
        {
            HasKey(ma => ma.Id);

            Property(ma => ma.Name).HasMaxLength(50);
            Property(ma => ma.Name).IsRequired();
            Property(ma => ma.Abrv).HasMaxLength(50);
            Property(ma => ma.Abrv).IsRequired();

            HasIndex(ma => ma.Name).IsUnique();
            HasIndex(ma => ma.Abrv).IsUnique();

            ToTable("VehicleMake");
        }
    }
}
