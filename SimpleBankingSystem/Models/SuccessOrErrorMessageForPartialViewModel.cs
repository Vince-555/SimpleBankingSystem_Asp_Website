﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class SuccessOrErrorMessageForPartialViewModel
    {
        public bool IsError { get; set; }

        public IEnumerable<string> AllMessages { get; set; }
    }
}
