using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer
{
    interface IConsole
    {
        void Print(string text);
    }

    class LowerCaseConsole : IConsole
    {
        public void Print(string text)
        {
            Console.WriteLine(text.ToLower());
        }
    }

    class UpperCaseConsole : IConsole
    {
        public void Print(string text)
        {
            Console.WriteLine(text.ToUpper());
        }
    }

    class PrintingService
    {
        IConsole console;

        public PrintingService(IConsole console)
        {
            this.console = console;
        }

        public void OutputText(string text)
        {
            console.Print(text);
        }
    }

    class MyInjectionModule : InjectModule
    {
        public override void Load()
        {
            Bind(typeof(IConsole), typeof(LowerCaseConsole));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InjectModule module = new MyInjectionModule();
            IKernel kernel = new StandardKernel(module);

            PrintingService ps = kernel.Get<PrintingService>();

            Console.ReadLine();
        }
    }
}
