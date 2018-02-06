using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Seedwork.CoreAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BindingAttribute : Attribute
    {
        private readonly List<Type> _abstractions; 

        public bool IsSelfBindable { get; set; }

        public BindingScope Scope { get; set; }

        public Type[] SuperTypes
        {
            get { return _abstractions.ToArray(); }
            set
            {
                _abstractions.Clear();
                _abstractions.AddRange(value);
            }
        }

        public Type SuperType
        {
            get { return _abstractions.Single(); }
            set
            {
                _abstractions.Clear();
                _abstractions.Add(value);
            }
        }

        public bool Active { get; set; }

        public BindingAttribute()
        {
            Active = true;
            Scope = BindingScope.Singleton;
            _abstractions = new List<Type>();
        }
    }
}