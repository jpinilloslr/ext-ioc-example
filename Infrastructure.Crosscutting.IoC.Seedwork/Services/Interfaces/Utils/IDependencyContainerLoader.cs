namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils
{
    public interface IDependencyContainerLoader
    {
        IDependencyContainer LoadAvailable();
    }
}