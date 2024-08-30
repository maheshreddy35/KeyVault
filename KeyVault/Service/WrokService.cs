
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace KeyVault.Service
{
    public class WrokService : IWorkService
    {
        private static readonly List<string> workList = [];
        private readonly string secretFlag;

        public WrokService(IConfiguration configuration)
        {
            var kvURL = configuration["KeyVaultURL"];
            var secretClient = new SecretClient(new Uri(kvURL), new DefaultAzureCredential());
            secretFlag = secretClient.GetSecret("PostFlag").ToString();
        }
        public void Add(List<string> works)
        {
            if (works.Contains(secretFlag))
            {
                workList.AddRange(works);
            }
            else
            {
                workList.Add("Flag Mismatched");
            }
            
        }

        public List<string> Get()
        {
            return workList;
        }
    }
}
