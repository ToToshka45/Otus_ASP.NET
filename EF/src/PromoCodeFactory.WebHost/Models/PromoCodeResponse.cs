﻿using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;

namespace PromoCodeFactory.WebHost.Models
{
    public class PromoCodeResponse
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string ServiceInfo { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public string PartnerName { get; set; }

        public EmployeeShortResponse PartnerManager { get; set; }

        public PreferenceResponse Preference { get; set; }
    }
}