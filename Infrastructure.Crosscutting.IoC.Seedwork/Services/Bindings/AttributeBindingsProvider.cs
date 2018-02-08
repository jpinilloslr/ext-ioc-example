using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Seedwork.CoreAttributes;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings;
using Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Bindings;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Bindings;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Utils;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Bindings
{
    public class AttributeBindingsProvider : IBindingsProvider
    {
        private List<Binding> _bindings;
        private readonly IAssembliesClassExplorer _assembliesClassExplorer;

        public static IBindingsProvider Create()
        {
            var assembliesEnumerator    = new AssembliesEnumerator();
            var assembliesClassExplorer = new AssembliesClassExplorer(assembliesEnumerator);

            return new AttributeBindingsProvider(assembliesClassExplorer);
        }

        public AttributeBindingsProvider(IAssembliesClassExplorer assembliesClassExplorer)
        {
            _assembliesClassExplorer = assembliesClassExplorer;
        }

        public IEnumerable<Binding> GetBindings()
        {
            _bindings = new List<Binding>();

            _assembliesClassExplorer.Explore(CheckForBindings);

            return _bindings;
        }

        private void CheckForBindings(Type type)
        {
            CheckForClassBindings(type);
            CheckForFactoryBinding(type);
        }

        private void CheckForClassBindings(Type type)
        {
            var attribute = GetBindingAttribute(type);

            if(null != attribute && attribute.Active)
            {
                AddBinding(attribute, type);
            }
        }

        private BindingAttribute GetBindingAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(BindingAttribute), true);

            return (BindingAttribute)attributes.FirstOrDefault();
        }

        private void AddBinding(BindingAttribute attribute, Type type)
        {
            if(attribute.SuperTypes.Any())
            {
                AddExplicitBindings(attribute, type);
            }
            else
            {
                AddImplicitBindings(attribute, type);
            }
        }

        private void AddExplicitBindings(BindingAttribute attribute, Type type)
        {
            foreach (var abstraction in attribute.SuperTypes)
            {
                if(type.GetInterfaces().Any(t => t == abstraction))
                {
                    AddExplicitBinding(attribute, type, abstraction);
                }
                else
                {
                    throw new InvalidTypeForSuperTypeException(type.FullName, abstraction.FullName);
                }
            }
        }

        private void AddExplicitBinding(BindingAttribute attribute, Type type, Type abstraction)
        {
            _bindings.Add(new TypeBinding
            {
                FromType = abstraction,
                ToType = type,
                BindingScope = attribute.Scope
            });    
        }

        private void AddImplicitBindings(BindingAttribute attribute, Type type)
        {
            if(attribute.IsSelfBindable)
            {
                AddSelfBindableImplicitBinding(attribute, type);
            }
            else
            {
                AddNotSelfBindableImplicitBinding(attribute, type);
            }
        }

        private void AddSelfBindableImplicitBinding(BindingAttribute attribute, Type type)
        {
            _bindings.Add(new TypeBinding
            {
                FromType = type,
                ToType = type,
                BindingScope = attribute.Scope
            });
        }

        private void AddNotSelfBindableImplicitBinding(BindingAttribute attribute, Type type)
        {
            if (!type.GetInterfaces().Any())
            {
                throw new ImplicitBindingWithoutSuperTypeException(type.FullName);
            }

            if (type.GetInterfaces().Count() > 1)
            {
                throw new AmbiguousImplicitBindingException(type.FullName);
            }

            _bindings.Add(new TypeBinding
            {
                FromType = type.GetInterfaces().First(),
                ToType = type,
                BindingScope = attribute.Scope
            });
        }

        private void CheckForFactoryBinding(Type type)
        {
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var attribute = GetBindingAttribute(method);
                if (null != attribute && attribute.Active)
                {
                    AddFactoryBinding(method, attribute);
                }
            }
        }

        private BindingAttribute GetBindingAttribute(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(typeof(BindingAttribute), true);

            return (BindingAttribute)attributes.FirstOrDefault();
        }

        private void AddFactoryBinding(MethodInfo method, BindingAttribute attribute)
        {
            if(attribute.SuperTypes.Any())
            {
                AddExplicitFactoryBinding(method, attribute);
            }
            else
            {
                AddImplicitFactoryBinding(method, attribute);
            }
        }

        private void AddExplicitFactoryBinding(MethodInfo method, BindingAttribute attribute)
        {
            var returnType = method.ReturnType;
            foreach (var abstraction in attribute.SuperTypes)
            {
                if(returnType != abstraction && !abstraction.IsSubclassOf(returnType))
                {
                    throw new InvalidTypeForSuperTypeFactoryBindingException(method.Name, 
                        returnType.FullName, abstraction.FullName);
                }

                _bindings.Add(new FactoryBinding
                {
                    FactoryMethod = method,
                    FromType = abstraction,
                    BindingScope = attribute.Scope
                });
            }
        }

        private void AddImplicitFactoryBinding(MethodInfo method, BindingAttribute attribute)
        {
            if(method.ReturnType.Name == "Void")
            {
                throw new InvalidReturnTypeInFactoryBindingException(method.Name);
            }

            _bindings.Add(new FactoryBinding
            {
                FactoryMethod = method,
                FromType = method.ReturnType,
                BindingScope = attribute.Scope
            });
        }
    }
}