module NetworkLoadBalancerBackendAddressPoolResource

open NetworkInterfaceIPConfiguration

type BackendAddressPool = {
    name: string
    ``type``: string
    apiVersion: string
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