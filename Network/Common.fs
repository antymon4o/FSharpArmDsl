module NetworkCommon

type SubnetProperties = {
    addressPrefix: string option
    addressPrefixes: string array option
    networkSecurityGroup: NetworkSecurityGroup option
    routeTable: RouteTable option
    natGateway: SubResource option
    serviceEndpoints: ServiceEndpointProperties option
    serviceEndpointPolicies: ServiceEndpointPolicy array option
    ipAllocations: SubResource array option
    delegations: Delegation array option
    privateEndpointNetworkPolicies: string option
    privateLinkServiceNetworkPolicies: string option
}
and RouteTable = {
    id: string
    location: string
    tags: Map<string, string> option
    properties: RouteTableProperties option
}
and SubResource  = { id: string }
and ServiceEndpointProperties = {
    service: string
    locations: string array
}
and ServiceEndpointPolicy = {
    id: string
    location: string
    tags: Map<string, string> option
    properties: ServiceEndpointPolicyProperties option
}
and Delegation = {
    id: string
    name: string
    properties: ServiceDelegationProperties option
}
and RouteTableProperties = {
    routes: Route array
    disableBgpRoutePropagation: bool option
}
and ServiceEndpointPolicyProperties = {
    serviceEndpointPolicyDefinitions: ServiceEndpointPolicyDefinition  array
}
and ServiceDelegationProperties = {
    serviceName: string
}
and Route = {
    id: string
    name: string
    properties: RouteProperties option
}
and ServiceEndpointPolicyDefinition = {
    id: string
    name: string
    properties: ServiceEndpointPolicyDefinitionProperties option
}
and RouteProperties = {
    addressPrefix: string option
    nextHopType: NextHopType
    nextHopIpAddress: string option
}
and NextHopType = VirtualNetworkGateway | VnetLocal | Internet | VirtualAppliance | None
and ServiceEndpointPolicyDefinitionProperties = {
    description: string option
    service: string option
    serviceResources: string array option
}
and NetworkSecurityGroup = {
    id: string
    location: string
    tags: Map<string, string> option
    properties: NetworkSecurityGroupProperties option
}
and NetworkSecurityGroupProperties = {
    securityRules: SecurityRule  array
}
and SecurityRule = {
    id: string
    name: string
    properties: SecurityRuleProperties option
}
and SecurityRuleProperties = {
    description: string option
    protocol: string
    sourcePortRange: string option
    destinationPortRange: string option
    sourceAddressPrefix: string option
    sourceAddressPrefixes: string array option
    sourceApplicationSecurityGroups: ApplicationSecurityGroup  array option
    destinationAddressPrefix: string option
    destinationAddressPrefixes: string array option
    destinationApplicationSecurityGroups: ApplicationSecurityGroup  array option
    sourcePortRanges: string array option
    destinationPortRanges: string array option
    access: Access
    priority: int option
    direction: Direction
}
and Access = Allow | Deny
and Direction = Inbound | Outbound
and ApplicationSecurityGroup = {
    id: string
    name: string
    tags: Map<string, string> option
}
and IPAddressVersion = IPv4 | IPv6
and IPAllocationMethod = Static | Dynamic
and Subnet = {
    id: string option
    name: string
    properties: SubnetProperties option
}
