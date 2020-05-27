module NetworkLoadBalancerBuilder

open NetworkLoadBalancerResource
open NetworkInterfaceIPConfiguration
open NetworkCommon

type LoadBalancerCreate = {
    name: string
    location: string
}

let create (input: LoadBalancerCreate) = {
    name = input.name
    ``type`` = "Microsoft.Network/loadBalancers"
    apiVersion = "2020-04-01"
    location = Some input.location
    tags = None
    sku = None
    properties = {
        frontendIPConfigurations = None
        backendAddressPools = None
        loadBalancingRules = None
        probes = None
        inboundNatRules = None
        inboundNatPools = None
        outboundRules = None
    }
}

type FrontendIPConfigurationParams = 
    | PrivateIPAddress of string
    | PrivateIPAllocationMethod of IPAllocationMethod
    //| PrivateIPAddressVersion of IPAddressVersion
    //| Subnet of Subnet
    | PublicIPAddress of PublicIPAddressEnum
    | PublicIPPrefix of string
and PublicIPAddressEnum = 
    | Id of string

let frontendIPConfigurationProperties (paramsSeq: FrontendIPConfigurationParams seq) (config: FrontendIPConfiguration) =
    { config with 
        properties = 
            paramsSeq 
            |> Seq.fold (fun (s: FrontendIPConfigurationProperties) i ->
                match i with
                | PrivateIPAddress ip -> { s with privateIPAddress = Some ip }
                | PrivateIPAllocationMethod al -> { s with privateIPAllocationMethod = Some al }
                | PublicIPAddress pIP -> { s with 
                                            publicIPAddress = 
                                                Some (
                                                    match pIP with
                                                    | Id id -> { 
                                                            NetworkPublicIPAddress.PublicIPAddress.id = Some id
                                                            location = None
                                                            tags = None
                                                            sku = None
                                                            properties = None
                                                            zones = None
                                                        }
                                                )
                                        }
                | PublicIPPrefix id -> { s with publicIPPrefix = Some { id = id } }
            ) {
                privateIPAddress = None
                privateIPAllocationMethod = None
                privateIPAddressVersion = None
                subnet = None
                publicIPAddress = None
                publicIPPrefix = None
            }
            |> Some
    }


let frontendIPConfiguration (name) (paramsSeq: FrontendIPConfigurationParams seq) (config: LoadBalancer) = 
    let frontendIPConfigurations = 
        config.properties.frontendIPConfigurations
        |> Option.defaultValue [||]

    { config with 
        properties = 
            { config.properties with
                frontendIPConfigurations = 
                    {
                        id = None
                        properties = None
                        name = Some name
                        zones = None
                    }
                    |> frontendIPConfigurationProperties paramsSeq
                    |> Array.create 1 
                    |> Array.append frontendIPConfigurations
                    |> Some
        }
    }


let backendAddressPool name (config: LoadBalancer) = 
    
    let backendAddressPools = 
        config.properties.backendAddressPools
        |> Option.defaultValue [||]

    { config with 
        properties = 
            { config.properties with
                backendAddressPools = 
                    { BackendAddressPool.id = None; properties = None; name = Some name }
                    |> Array.create 1 
                    |> Array.append backendAddressPools
                    |> Some
            }
    }

type LoadBalancingRuleParams = 
    | FrontendIPConfiguration of string
    | BackendAddressPool of string
    | Probe of string
    | LoadDistribution of LoadBalancingRuleLoadDistribution
    | BackendPort of int
    | IdleTimeoutInMinutes of int
    | EnableFloatingIP of bool 
    | EnableTcpReset  of bool 
    | DisableOutboundSnat  of bool 

let loadBalancingRule name protocol frontendPort (paramsSeq: LoadBalancingRuleParams seq) (config: LoadBalancer) = 
    let loadBalancingRules = 
        config.properties.loadBalancingRules
        |> Option.defaultValue [||]

    let loadBalancingRuleProperties = 
        paramsSeq
        |> Seq.fold (fun (s: LoadBalancingRuleProperties) i ->
            match i with
            | FrontendIPConfiguration id -> { s with frontendIPConfiguration = Some { id = id } }
            | BackendAddressPool id -> { s with backendAddressPool = Some { id = id } }
            | Probe id -> { s with probe = Some { id = id } }
            | LoadDistribution ld -> { s with loadDistribution = Some ld }
            | BackendPort port -> { s with backendPort = Some port }
            | IdleTimeoutInMinutes m -> { s with idleTimeoutInMinutes = Some m }
            | EnableFloatingIP b -> { s with enableFloatingIP = Some b }
            | EnableTcpReset b -> { s with enableTcpReset = Some b }
            | DisableOutboundSnat b -> { s with disableOutboundSnat = Some b }
        ) {
            frontendIPConfiguration = None
            backendAddressPool = None
            probe = None
            protocol = protocol
            loadDistribution = None
            frontendPort = frontendPort
            backendPort = None
            idleTimeoutInMinutes = None
            enableFloatingIP = None
            enableTcpReset = None
            disableOutboundSnat = None
        }

    let LoadBalancingRule = 
        { 
            LoadBalancingRule.id = None
            properties = Some loadBalancingRuleProperties
            name = Some name
        }

    { config with
        properties = {
            config.properties with 
                loadBalancingRules = 
                    [| LoadBalancingRule |]
                    |> Array.append loadBalancingRules
                    |> Some
        }
    }

type ProbeParams = 
    | IntervalInSeconds of int
    | NumberOfProbes of int
    | RequestPath of string

let probe name protocol port (paramsSeq: ProbeParams seq) (config: LoadBalancer) = 
    let probes = config.properties.probes |> Option.defaultValue [||]

    let probeProperties = 
        paramsSeq
        |> Seq.fold (fun (s: ProbeProperties) i ->
            match i with 
            | IntervalInSeconds i -> { s with intervalInSeconds = Some i }
            | NumberOfProbes n -> { s with numberOfProbes = Some n }
            | RequestPath path -> { s with requestPath = Some path }
        ) {
            protocol = protocol
            port = port
            intervalInSeconds = None
            numberOfProbes = None
            requestPath = None
        }

    { config with 
        properties = {
            config.properties with
                probes = 
                    [|{
                        Probe.id = None
                        name = Some name
                        properties = Some probeProperties
                    }|]
                    |> Array.append probes
                    |> Some
        }
    }

type InboundNatPoolParams = 
    | FrontendIPConfiguration of string
    | IdleTimeoutInMinutes of int
    | EnableFloatingIP of bool
    | EnableTcpReset of bool

type InboundNatPoolCreate = {
    name: string 
    protocol: LoadBalancerProtocol
    frontendPortRangeStart: int
    frontendPortRangeEnd: int 
    backendPort: int
}

let inboundNatPool (input: InboundNatPoolCreate) (paramsSeq: InboundNatPoolParams seq) (config: LoadBalancer) =
    let inboundNatPools = config.properties.inboundNatPools |> Option.defaultValue [||]

    let inboundNatPoolProperties = 
        paramsSeq
        |> Seq.fold (fun (s: InboundNatPoolProperties) i ->
            match i with
            | FrontendIPConfiguration id -> { s with frontendIPConfiguration = Some { id = id } }
            | IdleTimeoutInMinutes m -> { s with idleTimeoutInMinutes = Some m }
            | EnableFloatingIP b -> { s with enableFloatingIP = Some b }
            | EnableTcpReset b -> { s with  enableTcpReset = Some b }
        ) { 
            frontendIPConfiguration = None
            protocol = input.protocol
            frontendPortRangeStart = input.frontendPortRangeStart
            frontendPortRangeEnd = input.frontendPortRangeEnd
            backendPort = input.backendPort
            idleTimeoutInMinutes = None
            enableFloatingIP = None
            enableTcpReset = None
        }

    { config with 
        properties = {
            config.properties with
                inboundNatPools = 
                    [|{
                        InboundNatPool.id = None
                        name = Some input.name
                        properties = Some inboundNatPoolProperties
                    }|]
                    |> Array.append inboundNatPools
                    |> Some
        }
    }