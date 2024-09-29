using Company.G02.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Company.G02.PL.Mapping
{
    public interface ISmsService
    {
        public MessageResource SendSms(SmsMessage sms);
    }
}
