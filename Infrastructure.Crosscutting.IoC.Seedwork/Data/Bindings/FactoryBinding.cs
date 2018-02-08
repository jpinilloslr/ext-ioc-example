using System.Reflection;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings
{
    public class FactoryBinding: Binding
    {
        public MethodInfo FactoryMethod { get; set; }
    }
}