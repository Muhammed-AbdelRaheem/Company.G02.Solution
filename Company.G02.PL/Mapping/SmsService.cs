using Company.G02.DAL.Models;
using Company.G02.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.G02.PL.Mapping
{
    public class SmsService : ISmsService
    {
		private readonly IOptions<TwilloSettings> _options;

		public SmsService(IOptions<TwilloSettings> options)
        {
			_options = options;
		}
        public MessageResource SendSms(SmsMessage sms)
        {
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);
            var result = MessageResource.Create(
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_options.Value.TwilioPhoneNumber),
                to: sms.PhoneNumber);
            return result;
        }
    }
}
