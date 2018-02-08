using System.Web.Http.Dependencies;
using Domain.Seedwork.CoreAttributes;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Utils
{
    public class DependencyContainerResolver
    {
        [Binding(SuperType = typeof(IDependencyResolver), Scope = BindingScope.Transient)]
        public IDependencyResolver GetDependencyResolver()
        {
            return Bootstrapper.DependencyContainer;
        }
    }
}