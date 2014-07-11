using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetsuo.Entities.Model;

namespace Tetsuo.Entities
{
    public class EntityManager
    {
        public List<HubService> GetSpokesByHub(int hubId)
        {
            using (tetsuoEntities te = new tetsuoEntities())
            {
                return te.HubServices.Where(x => x.Hub.HubId == hubId).ToList();
            }
        }

        public string GetSpokeContract(string hubName, string spokeName)
        {
            using (tetsuoEntities te = new tetsuoEntities())
            {
                var query = from h in te.Hubs
                            where h.HubServices.Where(x => x.HubServiceName == spokeName).Any() &&
                            h.HubName == hubName
                            select h.HubServices.Where(x => x.HubServiceName == spokeName).FirstOrDefault();
                return query.FirstOrDefault().HubServiceContract;
            }
        }

        public List<Hub> GetHubsByGateway(int gatewayId)
        {
            using (tetsuoEntities te = new tetsuoEntities())
            {
                return te.Hubs.Where(x => x.Gateway.GatewayId == gatewayId).ToList();
            }
        }

        public List<Hub> GetHubsByGateway(Uri address)
        {
            using (tetsuoEntities te = new tetsuoEntities())
            {
                return te.Hubs.Where(x => x.Gateway.GatewayBaseUri == address.OriginalString).ToList();
            }
        }

        public List<Gateway> GetGateways()
        {
            using (tetsuoEntities te = new tetsuoEntities())
            {
                return te.Gateways.ToList();
            }
        }

        public Gateway GetGateway(int gatewayId)
        {
            using (tetsuoEntities te = new tetsuoEntities())
            {
                return te.Gateways.Where(x => x.GatewayId == gatewayId).FirstOrDefault();
            }
        }

        public void Instrument(string source, string message, string eventCode, int? id = null,string messageID="")
        {
            using (tetsuoEntities te = new tetsuoEntities())
            {
                Instrumentation i = new Instrumentation();
                i.DateCreated = DateTime.Now;
                i.EventType = eventCode;
                i.EventDetail = message;
                i.CorrelationID = messageID;
                switch (source)
                {
                    case "Gateway":
                        i.GatewayID = id;
                        break;
                    case "Spoke":
                        i.HubServiceID = id;
                        break;
                    case "Hub":
                        i.HubID = id;
                        break;
                }
                te.Instrumentations.AddObject(i);
                te.SaveChanges();

            }
        }
    }
}
