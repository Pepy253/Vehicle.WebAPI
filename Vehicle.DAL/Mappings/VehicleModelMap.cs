using System.Data.Entity.ModelConfiguration;
using Vehicle.Model.Models;

namespace Vehicle.DAL.Mappings
{
    class VehicleModelMap : EntityTypeConfiguration<VehicleModel>
    {
        public VehicleModelMap()
        {
            HasKey(mo => mo.Id);

            Property(mo => mo.MakeId).IsRequired();
            Property(mo => mo.Name).IsRequired();
            Property(mo => mo.Abrv).IsRequired();

            HasIndex(mo => mo.Name).IsUnique();
            HasIndex(mo => mo.MakeId).IsUnique();
            HasIndex(mo => mo.Abrv).IsUnique();

            HasRequired(mo => mo.VehicleMake)
                .WithMany()
                .HasForeignKey(mo => mo.MakeId);

            ToTable("VehicleModel");
        }
    }
}
