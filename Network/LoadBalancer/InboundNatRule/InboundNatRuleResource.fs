module NetworkLoadBalancerInboundNatRuleResource

open NetworkInterfaceIPConfiguration

type InboundNatRule = {
    apiVersion: string
    ``type``: string
    name: string
    properties: InboundNatRuleProperties
}