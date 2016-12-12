namespace PhotographyWorkshops.Data.Interfaces
{
    using Models;

    public interface IUnitOfWork
    {

        IRepository<Len> Lenses { get; }

        IRepository<DSLRCamera> DSLRCameras { get; }

        IRepository<MirrorlessCamera> MirrorlessCameras { get; }

        IRepository<Accessory> Accessories { get; }

        IRepository<Photographer> Photographers { get; }

        IRepository<Workshop> Workshops { get; }

        void Commit();
    }
}
