module NetworkLoadBalancerResource

open NetworkCommon
open NetworkVirtualNetworkResource
open NetworkPublicIPAddressResource

type LoadBalancer = {
    name: string
    ``type``: string
    apiVersion: string
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
and FrontendIPConfiguration = {
    id: string option
    properties: FrontendIPConfigurationProperties option
    name: string option
    zones: string array option
}
and BackendAddressPool = {
    id: string option
    properties: BackendAddressPoolProperties option
    name: string option
}
and LoadBalancingRule = {
    id: string option
    properties: LoadBalancingRuleProperties option
    name: string option
}
and Probe = {
    id: string option
    properties: ProbeProperties option
    name: string option
}
and InboundNatRule = {
    id: string option
    properties: InboundNatRuleProperties option
    name: string option
}
and InboundNatPool = {
    id: string option
    properties: InboundNatPoolProperties option
    name: string option
}
and OutboundRule = {
    id: string option
    properties: OutboundRuleProperties option
    name: string option
}
and FrontendIPConfigurationProperties = {
    privateIPAddress: string option
    privateIPAllocationMethod: IPAllocationMethod option
    privateIPAddressVersion: IPAddressVersion option
    subnet: Subnet option
    publicIPAddress: PublicIPAddress option
    publicIPPrefix: SubResource option
}
and BackendAddressPoolProperties = {
    loadBalancerBackendAddresses: LoadBalancerBackendAddress option
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
and LoadBalancerProtocol = Udp | Tcp | All
and LoadBalancingRuleLoadDistribution = Default | SourceIP | SourceIPProtocol
and ProbeProperties = {
    protocol: ProbeProtocol
    port: int
    intervalInSeconds: int option
    numberOfProbes: int option
    requestPath: string option
}
and ProbeProtocol = Http | Tcp | Https
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
and PublicIPAddress = {
    id: string option
    location: string option
    tags: Map<string, string>
    sku: PublicIPAddressSku option
    properties: PublicIPAddressProperties option
    zones: string array option
}
and LoadBalancerBackendAddress = {
    properties: LoadBalancerBackendAddressProperties option
    name: string option
}
and LoadBalancerBackendAddressProperties = {
    virtualNetwork: VirtualNetwork option
    ipAddress: string option
    networkInterfaceIPConfiguration: NetworkInterfaceIPConfiguration option
}
and VirtualNetwork = {
    id: string option
    location: string option
    tags: Map<string, string>
    properties: VirtualNetworkProperties option
}
and NetworkInterfaceIPConfiguration = {
    id: string option
    properties: NetworkInterfaceIPConfigurationProperties option
    name: string option
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
    properties: ApplicationGatewayBackendAddressPoolProperties option
    name: string option
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