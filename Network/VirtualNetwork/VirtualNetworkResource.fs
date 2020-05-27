module NetworkVirtualNetworkResource

open NetworkCommon

type VirtualNetwork = {
    apiVersion: string
    ``type``: string
    name: string
    location: string
    properties: VirtualNetworkProperties
}
and VirtualNetworkProperties = {
    addressSpace: AddressSpace option
    dhcpOptions: DhcpOptions  option
    subnets: Subnet array option
    virtualNetworkPeerings: VirtualNetworkPeering array option
    enableDdosProtection: bool option
    enableVmProtection: bool option
    ddosProtectionPlan: SubResource option
    bgpCommunities: VirtualNetworkBgpCommunities option
    ipAllocations: SubResource array option
}
and AddressSpace = {
    addressPrefixes: string array
}
and DhcpOptions = {
    dnsServers: string array
}
and VirtualNetworkPeering = {
    id: string option
    properties: VirtualNetworkPeeringProperties option
    name: string option
}
and VirtualNetworkBgpCommunities = {
    virtualNetworkCommunity: string
}
and VirtualNetworkPeeringProperties = {
    allowVirtualNetworkAccess: bool option
    allowForwardedTraffic: bool option
    allowGatewayTransit: bool option
    useRemoteGateways: bool option
    remoteVirtualNetwork: SubResource option
    remoteAddressSpace: AddressSpace option
}