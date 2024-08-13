namespace eventinfra_pub.Controllers
{
    using Azure.Messaging;
    using Azure;
    using Azure.Messaging.EventGrid.Namespaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class EventPubController : ControllerBase
    {
        private readonly ILogger<EventPubController> _logger;

        private readonly IConfigurationRoot _config;

        private readonly string _nsEndpoint = "https://kabalu-eg-ns1.westus2-1.eventgrid.azure.net";

        private readonly string _topicName = "kabalu-topic1";

        private readonly string _topicKey;

        public EventPubController(ILogger<EventPubController> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
            _topicKey = _config["topicKey"] ?? string.Empty;
        }

        [HttpPost(Name = "PublishEvent")]
        public async Task<EventPub> PublishEventAsync([FromBody] EventPub pub)
        {
            // create instance
            EventPub toPub = new EventPub();
            toPub.EventId = Guid.NewGuid();
            toPub.EventDescription = pub.EventDescription;
            toPub.EventName = pub.EventName;

            // pub client
            var client = new EventGridSenderClient(new Uri(_nsEndpoint), _topicName, new AzureKeyCredential(_topicKey));

            // create event
            var @ev = new CloudEvent("EventPublisherSource", "EventPub_Type", toPub);
            
            // response
            var response = await client.SendAsync(ev);

            return toPub;
        }
    }
}
    