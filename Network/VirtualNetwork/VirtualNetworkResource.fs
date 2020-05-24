module NetworkVirtualNetworkResource

open NetworkCommon

type VirtualNetwork = {
    name: string
    ``type``: string
    apiVersion: string
    location: string
    properties: VirtualNetworkProperties
}
// todo: add the remaining properties definition!
and VirtualNetworkProperties = {
    addressSpace: AddressSpace option
    dhcpOptions: DhcpOptions  option
    subnets: Subnet option
}
and AddressSpace = {
    addressPrefixes: string array
}
and DhcpOptions = {
    dnsServers: string array
}
