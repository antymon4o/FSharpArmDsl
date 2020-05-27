module NetworkVirtualNetworkBuilder

open NetworkCommon
open NetworkVirtualNetworkResource

type VirtualNetworkCreate = {
    name: string
    location: string
}

let create (input: VirtualNetworkCreate) = {
    name = input.name
    ``type`` = "Microsoft.Network/virtualNetworks"
    apiVersion = "2020-04-01"
    location = input.location
    properties = {
        addressSpace = None
        dhcpOptions = None
        subnets = None
        virtualNetworkPeerings = None
        enableDdosProtection = None
        enableVmProtection = None
        ddosProtectionPlan = None
        bgpCommunities = None
        ipAllocations = None
    }
}

type SubnetParam = 
    | AddressPrefix of string
    | AddressPrefixes of string array

let subnet (name: string) (subnetPropertySeq: SubnetParam seq) (vnet: VirtualNetwork) =
    let subnetProperties = 
        subnetPropertySeq
        |> Seq.fold (fun (s: SubnetProperties) i ->
            match i with
            | AddressPrefix (ap) -> { s with addressPrefix = Some ap }
            | AddressPrefixes (aps) -> { s with addressPrefixes = Some aps}
        ) {
            addressPrefix = None
            addressPrefixes = None
            networkSecurityGroup = None
            routeTable = None
            natGateway = None
            serviceEndpoints = None
            serviceEndpointPolicies = None
            ipAllocations = None
            delegations = None
            privateEndpointNetworkPolicies = None
            privateLinkServiceNetworkPolicies = None
        }

    let subnets = 
        vnet.properties.subnets 
        |> Option.defaultValue [||]
        |> Array.append [| { id = None; name = name; properties =  Some subnetProperties } |]

    { vnet with properties = { vnet.properties with subnets = Some subnets }}