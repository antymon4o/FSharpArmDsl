module NetworkVirtualNetworkSubnetResource

open NetworkCommon

type Subnet = {
    name: string
    ``type``: string
    apiVersion: string
    location: string
    properties: SubnetProperties
}