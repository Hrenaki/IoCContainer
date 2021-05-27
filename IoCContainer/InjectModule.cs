using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer
{
    interface ICanBind
    {
        void Bind(Type t1, Type t2);
    }

    internal class Binding
    {
        public Type Target { get; set; }
        public Type Value { get; set; }

        public Binding(Type target, Type value)
        {
            Target = target;
            Value = value;
        }
    }

    public abstract class InjectModule
    {
        internal List<Type> Targets { get; private set; }
        internal List<Type> Values { get; private set; }

        public void Bind(Type t1, Type t2)
        {
            if (Targets == null)
            {
                Targets = new List<Type>();
                Values = new List<Type>();
            }

            if (t2.IsSubclassOf(t1) || t2.GetInterface(t1.Name) != null)
            {
                Targets.Add(t1);
                Values.Add(t2);
            }
            else throw new Exception(t2.FullName + " isnt subclass of " + t1.FullName);
        }

        public abstract void Load();
    }
}
