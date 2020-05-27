module NetworkPublicIPAddress

open NetworkCommon

type PublicIPAddress = {
    id: string option
    location: string option
    tags: Map<string, string> option
    sku: PublicIPAddressSku option
    properties: PublicIPAddressProperties option
    zones: string array option
}
and PublicIPAddressSku = {
    name: PublicIPAddressSkuName
}
and PublicIPAddressSkuName = Basic | Standard
and PublicIPAddressProperties = {
    publicIPAllocationMethod: PublicIPAllocationMethod option
    publicIPAddressVersion: IPAddressVersion option
    dnsSettings: PublicIPAddressDnsSettings option
    ddosSettings: DdosSettings option
    ipTags: IpTag array option
    ipAddress: string option
    publicIPPrefix: SubResource option
    idleTimeoutInMinutes: int option
}
and PublicIPAllocationMethod = Static | NoDynamicInvocationAttribute
and PublicIPAddressDnsSettings = {
    domainNameLabel: string option
    fqdn: string option
    reverseFqdn: string option
}
and DdosSettings  = {
    ddosCustomPolicy: SubResource option
    protectionCoverage: ProtectionCoverage option
    protectedIP: bool option
}
and ProtectionCoverage = Basic | Standard
and IpTag = {
    ipTagType: string option
    tag: string option
}