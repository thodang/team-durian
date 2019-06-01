using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using User = DurianAirBnBWebservice.Identity.User;

namespace DurianAirBnBWebservice.Helper
{
    public class MailjetManager
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _clientBaseUrl;
        private readonly string _senderEmailAddress;

        public MailjetManager(IConfiguration configuration)
        {
            _apiKey = configuration.GetValue<string>("Mailjet:ApiKey");
            _apiSecret = configuration.GetValue<string>("Mailjet:ApiSecret");
            _clientBaseUrl = configuration.GetValue<string>("Mailjet:ClientBaseUrl");
            _senderEmailAddress = configuration.GetValue<string>("Mailjet:EmailAddress");
        }

        public async Task SendPasswordResetEmailAsync(User user, string token)
        {
            var client = new MailjetClient(_apiKey, _apiSecret)
            {
                Version = ApiVersion.V3_1,
            };
            var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.Messages, new JArray {
                    new JObject {
                        {"From", new JObject {
                            {"Email", _senderEmailAddress},
                            {"Name", "Durian Support"}
                        }},
                        {"To", new JArray {
                            new JObject {
                                {"Email", user.UserName},
                                {"Name", "You"}
                            }
                        }},
                        {"Subject", "Password Reset"},
                        {"TextPart", "Greetings from Durian Team!"},
                        {"HTMLPart", $"<div>Hi {user.UserName},</div><br /><div>To reset your password, click the link below. If you did not request your password to be reset, just ignore this email and your password will continue to stay the same. </div>" +
                                     $"<div><a href=\"{_clientBaseUrl}/passwordreset/{token}\"> Password Reset </a></div>" +
                                     $"<br /><br /><div>Thank You!<br />Durian Support</div>"}
                    }
                });

            await client.PostAsync(request);
        }

        public async Task SendConfirmationEmailAsync(User user)
        {
            var client = new MailjetClient(_apiKey, _apiSecret)
            {
                Version = ApiVersion.V3_1,
            };
            var request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.Messages, new JArray {
                    new JObject {
                        {"From", new JObject {
                            {"Email", _senderEmailAddress},
                            {"Name", "Durian Support"}
                        }},
                        {"To", new JArray {
                            new JObject {
                                {"Email", user.UserName},
                                {"Name", "You"}
                            }
                        }},
                        {"Subject", "Welcome to Durian!"},
                        {"TextPart", "Greetings from Durian!"},
                        {"HTMLPart", $"<div>Dear {user.UserName},</div><br /><div>Thank you for signing up for Durian! You can now view listings from all over Australia and pick a house for your next vacation stay. " +
                                     " Click the following link to start your experience with Durian! </div>" +
                                     $"<div><a href=\"{_clientBaseUrl}\"> Durian </a></div>" +
                                     $"<br /><br /><div>Thank You!<br />Durian Support</div>"}
                    }
                });

            await client.PostAsync(request);
        }

    }
}
