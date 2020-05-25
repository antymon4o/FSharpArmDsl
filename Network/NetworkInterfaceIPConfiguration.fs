module NetworkInterfaceIPConfiguration

open NetworkCommon
open NetworkPublicIPAddress
open NetworkVirtualNetworkResource

type NetworkInterfaceIPConfiguration = {
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
and BackendAddressPool = {
    id: string option
    properties: BackendAddressPoolProperties option
    name: string option
}
and InboundNatRule = {
    id: string option
    properties: InboundNatRuleProperties option
    name: string option
}
and InboundNatRuleProperties = {
    frontendIPConfiguration: SubResource option
    protocol: LoadBalancerProtocol option
    frontendPort: int option
    backendPort: int option
    idleTimeoutInMinutes: int option
    enableFloatingIP: bool option
    enableTcpReset: bool option
}
and FrontendIPConfiguration = {
    id: string option
    properties: FrontendIPConfigurationProperties option
    name: string option
    zones: string array option
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
