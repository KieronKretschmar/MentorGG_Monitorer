using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MentorMonitorer
{
    class WhatsappMessager
    {
        public static string PhoneNumberTo;
        public static string PhoneNumberFrom;
        public static string RequestUri;
        public static string AccountSid;
        public static string AuthToken;

        static WhatsappMessager()
        {
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
        }
    }
}
