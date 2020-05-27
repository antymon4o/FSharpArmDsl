module NetworkLoadBalancerBackendAddressPoolResource

open NetworkInterfaceIPConfiguration

type BackendAddressPool = {
    apiVersion: string
    ``type``: string
    name: string
    properties: BackendAddressPoolProperties
}
and BackendAddressPoolProperties = {
    loadBalancerBackendAddresses: LoadBalancerBackendAddress array option
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