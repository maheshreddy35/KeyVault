namespace KeyVault.Service
{
    public interface IWorkService
    {
        public List<string> Get();
        public void Add(List<string> works);
    }
}
