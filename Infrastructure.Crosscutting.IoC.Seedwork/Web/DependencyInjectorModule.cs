using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.Dependencies;
using System.Web.UI;
using Domain.Seedwork.CoreAttributes;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Web
{
    public class DependencyInjectorModule : IHttpModule
    {
        private IDependencyScope _pageDependencyScope;

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
            InjectHttpModules(context);
        }

        public void Dispose()
        {
        }

        private void InjectHttpModules(HttpApplication context)
        {
            for (var i = 0; i < context.Modules.Count; i++)
            {
                var currentModule = context.Modules.Get(i);
                InjectPropertiesOn(currentModule, Bootstrapper.DependencyContainer);
            }
        }

        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var currentPage = HttpContext.Current.Handler as Page;
            if (currentPage != null)
            {
                currentPage.InitComplete += OnPageInitComplete;
                currentPage.Init += OnPageInit;
                currentPage.Unload += OnPageUnload;
            }
        }

        private void OnPageUnload(object sender, EventArgs eventArgs)
        {
            DisposePageDependencyScope();
        }

        private void OnPageInit(object sender, EventArgs eventArgs)
        {
            CreatePageDependencyScope();
        }

        private void CreatePageDependencyScope()
        {
            _pageDependencyScope = Bootstrapper.DependencyContainer.BeginScope();
        }

        private void DisposePageDependencyScope()
        {
            _pageDependencyScope?.Dispose();
            _pageDependencyScope = null;
        }

        private void OnPageInitComplete(object sender, EventArgs e)
        {
            var currentPage = (Page)sender;
            Inject(currentPage, _pageDependencyScope);

            if(currentPage.Master != null)
            {
                Inject(currentPage.Master, _pageDependencyScope);
            }

            foreach (var c in GetControlTree(currentPage))
            {
                Inject(c, _pageDependencyScope);
            }
        }

        private IEnumerable<Control> GetControlTree(Control root)
        {
            foreach (Control child in root.Controls)
            {
                yield return child;
                foreach (var c in GetControlTree(child))
                {
                    yield return c;
                }
            }
        }

        private void Inject(Control control, IDependencyScope dependencyResolver)
        {
            InjectPropertiesOn(control, dependencyResolver);
        }

        private void InjectPropertiesOn(object currentPage, IDependencyScope dependencyResolver)
        {
            var properties = currentPage.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(DependencyAttribute), false);
                if (attributes.Length > 0)
                {
                    var valueToInject = dependencyResolver.GetService(property.PropertyType);
                    property.SetValue(currentPage, valueToInject, null);
                }
            }
        }
    }
}