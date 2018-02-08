using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings
{
    public class MethodBinding: Binding
    {
        public Func<object> ToMethod { get; set; }
    }
}