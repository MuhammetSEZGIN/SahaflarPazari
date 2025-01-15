using System;
using System.Web;
using Infrastructure.Repositories;
using Domain.Interfaces;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using SahaflarPazari.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Infrastructure.Identity;
using Microsoft.Owin.Security;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SahaflarPazari.App_Start.NinjectWebCommon), "Start")]
namespace SahaflarPazari.App_Start
{
    public static class NinjectWebCommon
    {
        public static readonly Bootstrapper bootstrapper = new Bootstrapper();


        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
        private static void RegisterServices(IKernel kernel) {
            kernel.Bind<Infrastructure.Data.SahaflarPazari>()
                 .ToSelf()
                 .InRequestScope(); // web isteği boyunca aynı context

            // identity
            kernel.Bind<IUserStore<ApplicationUser>>()
            .To<UserStore<ApplicationUser>>()
            .InRequestScope()
            .WithConstructorArgument("context", ctx => ctx.Kernel.Get<Infrastructure.Data.SahaflarPazari>());

            kernel.Bind<ApplicationUserManager>()
                  .ToSelf()
                  .InRequestScope();

            kernel.Bind<ApplicationSignInManager>()
                  .ToSelf()
                  .InRequestScope();

            kernel.Bind<IAuthenticationManager>()
                  .ToMethod(ctx => HttpContext.Current.GetOwinContext().Authentication)
                  .InRequestScope();

            // Repositories
            kernel.Bind<IBookImagesRepository>()
                  .To<BookImagesRepository>()
                  .InRequestScope();

            kernel.Bind<IBookCategoryRepository>().
                To<BookCategoryRepository>().
                InRequestScope();

            kernel.Bind<IPublisherRepository>().
                To<PublisherRepository>().
                InRequestScope();


            kernel.Bind<IBookRepository>()
                  .To<BookRepository>()
                  .InRequestScope();

            kernel.Bind<IAddressRepository>()
                  .To<AddressRepository>()
                  .InRequestScope();

            kernel.Bind<IOrderRepository>()
                  .To<OrderRepository>()
                  .InRequestScope();

            kernel.Bind<IComplaintRepository>()
                  .To<ComplaintRepository>()
                  .InRequestScope();

            kernel.Bind<IShoppingCartRepository>()
                  .To<ShoppingCartRepository>()
                  .InRequestScope();

            kernel.Bind<IWishlistRepository>()
                  .To<WishlistRepository>()
                  .InRequestScope();


            // UnitOfWork
            kernel.Bind<IUnitOfWork>()
                  .To<UnitOfWork>()
                  .InRequestScope();
        }
    }

}