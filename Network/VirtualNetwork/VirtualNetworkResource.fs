module NetworkVirtualNetworkResource

open NetworkCommon

type VirtualNetwork = {
    name: string
    ``type``: string
    apiVersion: string
    location: string
    properties: VirtualNetworkProperties
}
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
and Subnet = {
    id: string option
    name: string
    properties: SubnetProperties option
}
