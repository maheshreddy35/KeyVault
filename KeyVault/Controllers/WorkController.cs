using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        private static readonly List<string> _worklist = new List<string>();
        private string _secretFlag;
        private readonly IConfiguration _configuration;

        public WorkController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_worklist);
        }

        [HttpPost]
        public IActionResult Post(string str)
        {
            //var kvURL = configuration["KeyVaultURL"];
            //var clientId = configuration["ClientId"];
            //var clientSecret = configuration["ClientSecret"];
            //var tenantId = configuration["TenantId"];
            // Use ClientSecretCredential with values from configuration
            var kvURL = _configuration["KeyVaultURL"];
            var tenantId = _configuration["TenantId"];
            var clientId = _configuration["ClientId"];
            var clientSecret = _configuration["ClientSecret"];
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var secretClient = new SecretClient(new Uri(kvURL), clientSecretCredential);

            try
            {
                var secret = secretClient.GetSecret("PostFlag");
                _secretFlag = secret.Value.Value;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                throw new Exception("Failed to retrieve the secret from Key Vault.", ex);
            }
            if (str.Contains(_secretFlag))
            {
                _worklist.Add(str);
                return Ok(str);
            }
            else
            {
                _worklist.Add("Secret Flag Mismatch");
                return NotFound("Secret Flag Mismatch");
            }
        }
    }
}
