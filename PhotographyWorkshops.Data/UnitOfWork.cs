namespace PhotographyWorkshops.Data
{
    using Interfaces;
    using Models;

    public class UnitOfWork : IUnitOfWork
    {
        private PhotographyWorkshopsContext context;
        private IRepository<Len> lenses;
        private IRepository<Accessory> accessories;
        private IRepository<DSLRCamera> dslrameras;
        private IRepository<MirrorlessCamera> mirrorlessCameras;
        private IRepository<Photographer> photographers;
        private IRepository<Workshop> workshops;
        private IRepository<Camera> cameras;

        public UnitOfWork()
        {
            this.context = new PhotographyWorkshopsContext();
        }

        public IRepository<Len> Lenses => this.lenses ?? (this.lenses = new Repository<Len>(this.context.Lenses));

        public IRepository<Accessory> Accessories => this.accessories ?? (this.accessories = new Repository<Accessory>(this.context.Accessories));

        public IRepository<DSLRCamera> DSLRCameras => this.dslrameras ?? (this.dslrameras = new Repository<DSLRCamera>(this.context.DSLRCameras));

        public IRepository<MirrorlessCamera> MirrorlessCameras => this.mirrorlessCameras ?? (this.mirrorlessCameras = new Repository<MirrorlessCamera>(this.context.MirrorlessCameras));

        public IRepository<Photographer> Photographers => this.photographers ?? (this.photographers = new Repository<Photographer>(this.context.Photographers));

        public IRepository<Workshop> Workshops => this.workshops ?? (this.workshops = new Repository<Workshop>(this.context.Workshops));

        public IRepository<Camera> Cameras => this.cameras ?? (this.cameras = new Repository<Camera>(this.context.Cameras));

        public void Commit()
        {
            this.context.SaveChanges();
        }
    }
}
