using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace IoCContainer
{
    interface IKernel : ICanBind
    {
        T Get<T>() where T : class;
    }

    public class StandardKernel : IKernel
    {
        InjectModule module;

        public StandardKernel(InjectModule module)
        {
            this.module = module;
            module.Load();
        }

        public void Bind(Type t1, Type t2)
        {
            module.Bind(t1, t2);
        }

        private object get(Type t)
        {
            var constructors = t.GetConstructors();

            if (constructors != null)
            {
                var suitableConstructor = constructors[0];
                int maxParamCount = 0;
                int curParamCount = 0;
                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters().Select(v => v.ParameterType);
                    if (parameters.Except(module.Targets).Count() == 0 && (curParamCount = parameters.Count()) > maxParamCount)
                    {
                        suitableConstructor = constructor;
                        maxParamCount = curParamCount;
                    }
                }

                var arguments = new object[maxParamCount];
                var actualParameters = suitableConstructor.GetParameters();
                for (int i = 0; i < actualParameters.Length; i++)
                {
                    arguments[i] = get(module.Values[module.Targets.IndexOf(actualParameters[i].ParameterType)]);
                }

                return Activator.CreateInstance(t, arguments);
            }
            else return Activator.CreateInstance(t);
        }

        public T Get<T>() where T : class
        {
            return get(typeof(T)) as T;
        }
    }
}