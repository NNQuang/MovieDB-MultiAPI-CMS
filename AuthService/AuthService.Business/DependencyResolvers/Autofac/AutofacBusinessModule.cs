using Autofac;
using AuthService.Core.Utilities.Security.Tokens;
using AuthService.Core.Utilities.Security.Tokens.JWT;
using AuthService.Data.UnitOfWork.Abstract;
using AuthService.Data.UnitOfWork.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Business.Abstract;
using AuthService.Business.Concrete;

namespace AuthService.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
        }
    }
}
