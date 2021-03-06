﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace net.obliteracy.tetsuo.admin.MyHub {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MyHub.IServiceResolverGateway")]
    public interface IServiceResolverGateway {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceResolverGateway/OnShutdownGateway", ReplyAction="http://tempuri.org/IServiceResolverGateway/OnShutdownGatewayResponse")]
        void OnShutdownGateway();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceResolverGateway/OnInitializeGateway", ReplyAction="http://tempuri.org/IServiceResolverGateway/OnInitializeGatewayResponse")]
        void OnInitializeGateway(string HubContract, System.Uri baseAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceResolverGateway/ResolveHub", ReplyAction="http://tempuri.org/IServiceResolverGateway/ResolveHubResponse")]
        bool ResolveHub(string uri);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceResolverGateway/ResolveSpoke", ReplyAction="http://tempuri.org/IServiceResolverGateway/ResolveSpokeResponse")]
        object ResolveSpoke(string HubName, string SpokeName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceResolverGatewayChannel : net.obliteracy.tetsuo.admin.MyHub.IServiceResolverGateway, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceResolverGatewayClient : System.ServiceModel.ClientBase<net.obliteracy.tetsuo.admin.MyHub.IServiceResolverGateway>, net.obliteracy.tetsuo.admin.MyHub.IServiceResolverGateway {
        
        public ServiceResolverGatewayClient() {
        }
        
        public ServiceResolverGatewayClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceResolverGatewayClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceResolverGatewayClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceResolverGatewayClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void OnShutdownGateway() {
            base.Channel.OnShutdownGateway();
        }
        
        public void OnInitializeGateway(string HubContract, System.Uri baseAddress) {
            base.Channel.OnInitializeGateway(HubContract, baseAddress);
        }
        
        public bool ResolveHub(string uri) {
            return base.Channel.ResolveHub(uri);
        }
        
        public object ResolveSpoke(string HubName, string SpokeName) {
            return base.Channel.ResolveSpoke(HubName, SpokeName);
        }
    }
}
