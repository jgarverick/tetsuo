using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Resources;
using System.Globalization;
using Tetsuo.Common.Contracts;
using System.Messaging;
using System.Transactions;
using System.Threading;
using System.Runtime.Serialization.Formatters;
using System.Dynamic;
using Tetsuo.Common;
using System.Xml.Serialization;
using tempuri.org;
using System.ServiceModel.MsmqIntegration;

namespace TestClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
         //   MessageQueue q = new MessageQueue(@".\private$\request");
         //   Message msg = new Message();

         //   RoutedMessageContract m = new RoutedMessageContract()
         //{
         //    DestinationHub = "mainline",
         //    DestinationSpoke = "test2"
         //};
         //   msg.Body = m;

         //   msg.Label = "ServiceStart";
         //   using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
         //   {
         //       q.Send(msg, MessageQueueTransactionType.Automatic);
         //       scope.Complete();
         //   }
         //   Console.WriteLine(msg.Id);

         //   MessageQueue responder = new MessageQueue(@".\private$\response");

         //   MessageQueueTransaction tr = new MessageQueueTransaction();
         //   Message response = responder.ReceiveByCorrelationId(msg.Id, new TimeSpan(0, 0, 20), tr);
         //   Console.WriteLine(response.Id);
            

         //   Console.Read();
            for (int i = 0; i < 25; i++)
            {
                RoutedMessageContract ct = new RoutedMessageContract();
                XmlSerializer serial = new XmlSerializer(typeof(List<ActionParameter>));
                List<ActionParameter> parameters = new List<ActionParameter>();

                ct.DestinationHub = "mainline";
                ct.DestinationSpoke = "subSpoke";
                ct.StatusCode = Tetsuo.Common.TransmissionStatusCodes.SVC_STOP;
                using (StringWriter s = new StringWriter())
                {
                    serial.Serialize(s, parameters);
                    ct.MessageBody = s.ToString();
                }
                MessageQueue q = new MessageQueue(@".\private$\request");
                Message msg = new Message(ct);
                //Thread.Sleep(6005);
                ct.StatusCode = Tetsuo.Common.TransmissionStatusCodes.BEGIN;
                q = new MessageQueue(@".\private$\mainline.test.request");
                msg = new Message(ct);
                msg.Label = "DoSomething";
                ct.Action = "DoSomething";
                ct.IsFromGateway = true;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    q.Send(msg, MessageQueueTransactionType.Automatic);
                    scope.Complete();
                }

                ct.StatusCode = Tetsuo.Common.TransmissionStatusCodes.BEGIN;

                parameters.Add(new ActionParameter() { Name = "x", ParamType = "System.Int32", Value = new Random().Next(1, 5000) });
                parameters.Add(new ActionParameter() { Name = "y", ParamType = "System.Int32", Value = new Random().Next(100, 1200) });

                using (StringWriter s = new StringWriter())
                {
                    serial.Serialize(s, parameters);
                    ct.MessageBody = s.ToString();
                }
                q = new MessageQueue(@".\private$\backline.test2.request");
                msg = new Message(ct);
                msg.Label = "Multiply";
                ct.Action = "Multiply";
                ct.DestinationHub = "backline";
                ct.IsFromGateway = true;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    q.Send(msg, MessageQueueTransactionType.Automatic);
                    scope.Complete();
                }

                Console.WriteLine(msg.Id);

                MessageQueue responder = new MessageQueue(@".\private$\response");

                MessageQueueTransaction tr = new MessageQueueTransaction();
                responder.MessageReadPropertyFilter = new MessagePropertyFilter() { CorrelationId = true };
                Message response = responder.ReceiveByCorrelationId(msg.Id, new TimeSpan(0, 0, 20), tr);
                Console.WriteLine(response.Id);
            }
            Console.Read();
        }

    }
}