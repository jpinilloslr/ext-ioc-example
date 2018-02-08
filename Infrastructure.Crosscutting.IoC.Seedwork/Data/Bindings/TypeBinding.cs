using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings
{
    public class TypeBinding: Binding
    {
        public Type ToType { get; set; }
    }
}