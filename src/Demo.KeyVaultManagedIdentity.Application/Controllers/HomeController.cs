using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace Demo.KeyVaultManagedIdentity.Application.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var isManagedIdentity = Environment.GetEnvironmentVariable("MSI_ENDPOINT") != null
                                    && Environment.GetEnvironmentVariable("MSI_SECRET") != null;

            ViewBag.IsManagedIdentity = isManagedIdentity;

            if (isManagedIdentity)
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient =
                    new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                var secret =
                    await keyVaultClient.GetSecretAsync(
                        "https://demo4847.vault.azure.net/secrets/password");

                ViewBag.Secret = secret.Value;
            }

            return View();
        }
    }
}