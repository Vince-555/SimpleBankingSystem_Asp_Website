using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public interface IRazorViewToStringRenderer
    {
        public Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }

}
