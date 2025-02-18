using System;

namespace Pcf.RabbitMQ_Events
{
    public class PromocodeEvent
    {
        public string Code { get; set; }

        public string ServiceInfo { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public Guid? PartnerManagerId { get; set; }

        public Guid PartnerId { get; set; }

        public Guid PreferenceId { get; set; }
    }
}
