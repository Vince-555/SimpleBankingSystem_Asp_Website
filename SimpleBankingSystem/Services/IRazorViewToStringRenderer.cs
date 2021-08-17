using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public interface IRazorViewToStringRenderer
    {
        public Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }

}
