﻿module NetworkLoadBalancerResource

open NetworkCommon
open NetworkVirtualNetworkResource
open NetworkPublicIPAddressResource
open NetworkInterfaceIPConfiguration
open Newtonsoft.Json
open JsonConverters

type LoadBalancer = {
    apiVersion: string
    ``type``: string
    name: string
    location: string option
    tags: Map<string, string> option
    sku: LoadBalancerSku option
    properties: LoadBalancerProperties
}
and LoadBalancerSku = {
    name: LoadBalancerSkuName
}
and LoadBalancerSkuName = Basic | Standard
and LoadBalancerProperties = {
    frontendIPConfigurations: FrontendIPConfiguration array option
    backendAddressPools: BackendAddressPool array option
    loadBalancingRules: LoadBalancingRule array option
    probes: Probe array option
    inboundNatRules: InboundNatRule array option
    inboundNatPools: InboundNatPool array option
    outboundRules: OutboundRule array option
}
//and BackendAddressPool = {
//    id: string option
//    properties: BackendAddressPoolProperties option
//    name: string option
//}
and LoadBalancingRule = {
    id: string option
    name: string option
    properties: LoadBalancingRuleProperties option
}
and Probe = {
    id: string option
    name: string option
    properties: ProbeProperties option
}
and InboundNatRule = {
    id: string option
    name: string option
    properties: InboundNatRuleProperties option
}
and InboundNatPool = {
    id: string option
    name: string option
    properties: InboundNatPoolProperties option
}
and OutboundRule = {
    id: string option
    name: string option
    properties: OutboundRuleProperties option
}
and LoadBalancingRuleProperties = {
    frontendIPConfiguration: SubResource option
    backendAddressPool: SubResource option
    probe: SubResource option
    protocol: LoadBalancerProtocol
    loadDistribution: LoadBalancingRuleLoadDistribution option
    frontendPort: int
    backendPort: int option
    idleTimeoutInMinutes: int option
    enableFloatingIP: bool option
    enableTcpReset: bool option
    disableOutboundSnat: bool option
}
and ProbeProperties = {
    protocol: ProbeProtocol
    port: int
    intervalInSeconds: int option
    numberOfProbes: int option
    requestPath: string option
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
ProbeProtocol = Http | Tcp | Https
and InboundNatRuleProperties = {
    frontendIPConfiguration: SubResource option
    protocol: LoadBalancerProtocol option
    frontendPort: int option
    backendPort: int option
    idleTimeoutInMinutes: int option
    enableFloatingIP: bool option
    enableTcpReset: bool option
}
and InboundNatPoolProperties = { 
    frontendIPConfiguration: SubResource option
    protocol: LoadBalancerProtocol
    frontendPortRangeStart: int
    frontendPortRangeEnd: int
    backendPort: int
    idleTimeoutInMinutes: int option
    enableFloatingIP: bool option
    enableTcpReset: bool option
}
and OutboundRuleProperties = {
    allocatedOutboundPorts: int option
    frontendIPConfigurations: SubResource array
    backendAddressPool: SubResource
    protocol: LoadBalancerProtocol
    enableTcpReset: bool option
    idleTimeoutInMinutes: int option
}
and NetworkInterfaceIPConfiguration = {
    id: string option
    name: string option
    properties: NetworkInterfaceIPConfigurationProperties option
}
and NetworkInterfaceIPConfigurationProperties = {
    virtualNetworkTaps: VirtualNetworkTap array option
    applicationGatewayBackendAddressPools: ApplicationGatewayBackendAddressPool array option
    loadBalancerBackendAddressPools: BackendAddressPool array option
    loadBalancerInboundNatRules: InboundNatRule array option
    privateIPAddress: string option
    privateIPAllocationMethod: IPAllocationMethod option
    privateIPAddressVersion: IPAddressVersion option
    subnet: Subnet option
    primary: bool option
    publicIPAddress: PublicIPAddress option
    applicationSecurityGroups: ApplicationSecurityGroup array option
}
and VirtualNetworkTap = {
    id: string option
    location: string option
    tags: Map<string, string>
    properties: VirtualNetworkTapProperties option
}
and ApplicationGatewayBackendAddressPool = {
    id: string option
    name: string option
    properties: ApplicationGatewayBackendAddressPoolProperties option
}
and VirtualNetworkTapProperties = {
    destinationNetworkInterfaceIPConfiguration: NetworkInterfaceIPConfiguration option
    destinationLoadBalancerFrontEndIPConfiguration: FrontendIPConfiguration option
    destinationPort: int option
}
and ApplicationGatewayBackendAddressPoolProperties = {
    backendAddresses: ApplicationGatewayBackendAddress array
}
and ApplicationGatewayBackendAddress = {
    fqdn: string option
    ipAddress: string option
}