using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public class ErrorCollector : IErrorCollector
    {
        List<string> IErrorCollector.ErrorCollector(ModelStateDictionary modelstate)
        {
            return modelstate.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        }
    }
}
