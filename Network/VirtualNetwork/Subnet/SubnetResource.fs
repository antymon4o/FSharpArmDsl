module NetworkVirtualNetworkSubnetResource

open NetworkCommon

type Subnet = {
    apiVersion: string
    ``type``: string
    name: string
    location: string
    properties: SubnetProperties
}