using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SimpleInjector.Lifestyles;
using SimpleInjector;
using LoginDemo.Servcices;
using LoginDemo.Servcices.Interfaces;
using RestSharp;
using LoginDemo.Domain.Model;

namespace LoginDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container container;
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    Bootstrap();

        //    var login = container.GetInstance(typeof(Login));

        //    base.OnStartup(e);
        //}

        private void ApplicationStartup(Object sender, StartupEventArgs e)
        {
            Bootstrap();
            var login = container.GetInstance(typeof(Login));
        }

        private static void Bootstrap()
        {
            // Create the container as usual.
            container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Options.EnableAutoVerification = false;

            container.Register<IRequest, FacebookWebRequest>();
            container.Register<IWebScrapingService, WebScrapingService>();
            container.Register<IDocumentHelper, DocumentHelper>();
            container.Collection.Register(new List<RestResponseCookie>());
            container.Register(() => new Login(
                                new WebScrapingService(new FacebookWebRequest(
                                    new DocumentHelper(), 
                                    new List<RestResponseCookie>(),
                                    new Credential()
                                ))
                ));

            // Optionally verify the container.
            //container.Verify();
        }
    }
}
