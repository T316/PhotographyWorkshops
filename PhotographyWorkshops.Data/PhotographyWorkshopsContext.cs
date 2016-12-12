namespace PhotographyWorkshops.Data
{
    using Models;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class PhotographyWorkshopsContext : DbContext
    {
        public PhotographyWorkshopsContext()
            : base("name=PhotographyWorkshopsContext")
        {
        }

        public DbSet<Len> Lenses { get; set; }

        public DbSet<DSLRCamera> DSLRCameras { get; set; }

        public DbSet<MirrorlessCamera> MirrorlessCameras { get; set; }

        public DbSet<Camera> Cameras { get; set; }

        public DbSet<Accessory> Accessories { get; set; }

        public DbSet<Photographer> Photographers { get; set; }

        public DbSet<Workshop> Workshops { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Len>().Property(l => l.MaxAperture).HasPrecision(18, 1);

            modelBuilder.Entity<DSLRCamera>().ToTable("DSLRCamera");
            modelBuilder.Entity<MirrorlessCamera>().ToTable("MirrorlessCamera");

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Workshop>()
               .HasMany<Photographer>(p => p.Participant)
               .WithMany(w => w.WorkshopsParticipate)
               .Map(cs =>
               {
                   cs.MapLeftKey("WorkshopId");
                   cs.MapRightKey("ParticipantId");
                   cs.ToTable("WorkshopParticipants");
               });

        }
    }
}