using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Bindings;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Bindings
{
    public class BindingsImporter: IBindingsImporter
    {
        private readonly IDependencyContainer _dependencyContainer;

        public BindingsImporter(IDependencyContainer dependencyContainer)
        {
            _dependencyContainer = dependencyContainer;
        }

        public void Import(IBindingsProvider bindingsProvider)
        {
            var bindings = bindingsProvider.GetBindings();

            foreach (var binding in bindings)
            {
                _dependencyContainer.Bind(binding);
            }
        }
    }
}