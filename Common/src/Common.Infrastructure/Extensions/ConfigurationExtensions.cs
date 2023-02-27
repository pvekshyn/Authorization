using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Common.Infrastructure.Extensions;
public static class ConfigurationExtensions
{
    public static void AddKeyVault(this ConfigurationManager configuration, string keyVaultName, string managedIdentityClientId)
    {
        var options = new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId };
        var keyVaultEndpoint = $"https://{keyVaultName}.vault.azure.net";
        configuration.AddAzureKeyVault(
            new Uri(keyVaultEndpoint),
            new DefaultAzureCredential(options));
    }
}
