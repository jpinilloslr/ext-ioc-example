namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Interceptors
{
    public interface IInterceptorsImporter
    {
        void Import(IInterceptorsProvider interceptorsProvider);
    }
}