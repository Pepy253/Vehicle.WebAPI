using System.Data.Entity.ModelConfiguration;
using Vehicle.Model.Models;

namespace Vehicle.DAL.Mappings
{
    public class VehicleMakeMap : EntityTypeConfiguration<VehicleMake>
    {
        public VehicleMakeMap()
        {
            HasKey(ma => ma.Id);

            Property(ma => ma.Name).IsRequired();
            Property(ma => ma.Abrv).IsRequired();

            HasIndex(ma => ma.Name).IsUnique();
            HasIndex(ma => ma.Abrv).IsUnique();

            ToTable("VehicleMake");
        }
    }
}
