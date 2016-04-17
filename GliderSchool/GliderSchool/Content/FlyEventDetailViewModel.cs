﻿using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using GliderSchool.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GliderSchool.Content
{
    public class FlyEventDetailViewModel : ViewModelBase
    {
        private readonly IMasterNavigationService navigationService;
        public FlyEventDetailViewModel(IMasterNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}
