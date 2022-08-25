// See https://aka.ms/new-console-template for more information

using Microsoft.Graph;
using Microsoft.Identity.Client;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

string tenantId = "get from Selva";
string clientId = "get from Selva";
string clientSecret = "get from Selva";
string userId = "get from Selva";
//The following scope is required to acquire the token
string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
var message = new Message
{
    Subject = "Test mail",
    Body = new ItemBody
    {
        ContentType = BodyType.Html,
        Content = "Email Content"
    },
    ToRecipients = new List<Recipient>()
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = "kabhhinav@humana.comom"
                        }
                    }
                },
    CcRecipients = new List<Recipient>()
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = "noreply@gmail.com"
                        }
                    },
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = "kumar.abhinav@tcs.com"
                        }
                    }
                }
};
IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}/v2.0"))
                .Build();

// Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
var authResult = await confidentialClient
        .AcquireTokenForClient(scopes)
        .ExecuteAsync().ConfigureAwait(false);

var token = authResult.AccessToken;
// Build the Microsoft Graph client. As the authentication provider, set an async lambda
// which uses the MSAL client to obtain an app-only access token to Microsoft Graph,
// and inserts this access token in the Authorization header of each API request. 
GraphServiceClient graphServiceClient =
    new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
    {
                    // Add the access token in the Authorization header of the API request.
                    requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    })
    );


await graphServiceClient.Users[userId]
      .SendMail(message, false)
      .Request()
      .PostAsync();
