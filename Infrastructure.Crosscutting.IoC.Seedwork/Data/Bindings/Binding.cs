using System;
using Domain.Seedwork.CoreAttributes;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings
{
    public abstract class Binding
    {
        public Type FromType { get; set; }
        public BindingScope? BindingScope { get; set; }
    }
}