
namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Bindings
{
    public interface IBindingsImporter
    {
        void Import(IBindingsProvider bindingsProvider);
    }
}