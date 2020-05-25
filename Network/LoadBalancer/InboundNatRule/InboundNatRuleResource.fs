module NetworkLoadBalancerInboundNatRuleResource

open NetworkInterfaceIPConfiguration

type InboundNatRule = {
    name: string
    ``type``: string
    apiVersion: string
    properties: InboundNatRuleProperties
}