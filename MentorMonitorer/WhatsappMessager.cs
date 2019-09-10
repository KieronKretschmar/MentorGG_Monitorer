using System;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MentorMonitorer
{
    /// <summary>
    /// Set auth data as environment variable, 
    /// see https://www.twilio.com/docs/usage/secure-credentials and https://www.twilio.com/blog/2017/01/how-to-set-environment-variables.html
    /// </summary>
    class WhatsappMessager
    {

        // Twilio phone related settings
        public static string PhoneNumberTo;
        public static string PhoneNumberFrom;
        public static string RequestUri;
        public static string AccountSid;
        public static string AuthToken;
       


        static WhatsappMessager()
        {
            // Twilio phone related settings
            PhoneNumberTo = "+491751571271";
            PhoneNumberFrom = "+14155238886";
            AccountSid = System.Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID", EnvironmentVariableTarget.User);
            AuthToken = System.Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN", EnvironmentVariableTarget.User);
            RequestUri = "https://api.twilio.com/2010-04-01/Accounts/AC2b8d4d2a3e95907f2779a9f723248b28/Messages.json";
        }

        public static void SendWhatsAppMessage(string text)
        {
            // Find your Account Sid and Token at twilio.com/console
            TwilioClient.Init(AccountSid, AuthToken);

            var message = MessageResource.Create(
                body: text,
                from: new Twilio.Types.PhoneNumber("whatsapp:" + PhoneNumberFrom),
                to: new Twilio.Types.PhoneNumber("whatsapp:" + PhoneNumberTo)
            );

            Console.WriteLine(message.DateSent);
            Console.WriteLine(message.Status);
        }
    }
}
