using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SimpleBankingSystem.Services
{
  public interface IErrorCollector
    {
        public List<string> ErrorCollector(ModelStateDictionary modelstate);
    }
}
