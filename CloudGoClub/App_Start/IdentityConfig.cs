using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using CloudGoClub.Models;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;

namespace CloudGoClub
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage msg = new MailMessage();
            msg.Subject = message.Subject;
            msg.Body = message.Body;
            msg.From = new MailAddress("mattia.garassini@gmail.com");
            msg.To.Add(message.Destination);
            msg.IsBodyHtml = true;
            return Task.FromResult(SendMessage(msg));
        }

        private IAsyncResult SendMessage(MailMessage message)
        {
            AsyncMethodCaller caller = new AsyncMethodCaller(SendMailInSeperateThread);
            AsyncCallback callbackHandler = new AsyncCallback(AsyncCallback);
            return caller.BeginInvoke(message, callbackHandler, null);
        }

        private delegate void AsyncMethodCaller(MailMessage message);

        private void SendMailInSeperateThread(MailMessage message)
        {
            //try
            //{
                SmtpClient client = new SmtpClient();
                client.Timeout = 20000;
                client.Host = "smtp.gmail.com";
                System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("mattia.garassini@gmail.com", "hongo3chome");
                client.Port = int.Parse("587");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicauthenticationinfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);
                client.Dispose();
                message.Dispose();
            //}
            //catch (Exception e)
            //{
            //    // This is very necessary to catch errors since we are in
            //    // a different context & thread
            //    Elmah.ErrorLog.GetDefault(null).Log(new Error(e));
            //}
        }

        private void AsyncCallback(IAsyncResult ar)
        {
            //try
            //{
                AsyncResult result = (AsyncResult)ar;
                AsyncMethodCaller caller = (AsyncMethodCaller)result.AsyncDelegate;
                caller.EndInvoke(ar);
            //}
            //catch (Exception e)
            //{
            //    Elmah.ErrorLog.GetDefault(null).Log(new Error(e));
            //    Elmah.ErrorLog.GetDefault(null).Log(new Error(new Exception("Emailer - This hacky asynccallback thing is puking, serves you right.")));
            //}
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(ApplicationDbContext context)
            : base(context)
        { }
    }

    public class ApplicationUserValidator : UserValidator<ApplicationUser, int> 
    {
        public ApplicationUserValidator(ApplicationUserManager manager)
            : base(manager)
        { }
    }

    public class ApplicationEmailTokenProvider : EmailTokenProvider<ApplicationUser, int> { }

    public class ApplicationPhoneNumberTokenProvider : PhoneNumberTokenProvider<ApplicationUser, int> { }

    public class ApplicationDataProtectorTokenProvider : DataProtectorTokenProvider<ApplicationUser, int> 
    {
        public ApplicationDataProtectorTokenProvider(Microsoft.Owin.Security.DataProtection.IDataProtector protector)
            : base(protector)
        { }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new ApplicationUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new ApplicationPhoneNumberTokenProvider
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new ApplicationEmailTokenProvider
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new ApplicationDataProtectorTokenProvider(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
