﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class AllCardsViewModel
    {
        public UserNavbarViewModel UserNavBarModel { get; set; }

        public SuccessOrErrorMessageForPartialViewModel SuccessOrError { get; set; }

        public ICollection<CardViewModel> AllCards { get; set; }
    }
}
