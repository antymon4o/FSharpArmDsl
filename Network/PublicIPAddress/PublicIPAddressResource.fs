module NetworkPublicIPAddressResource

open NetworkPublicIPAddress

type PublicIPAddress = {
    name: string
    ``type``: string
    apiVersion: string
    location: string
    tags: Map<string, string> option
    sku: PublicIPAddressSku option
    properties: PublicIPAddressProperties
    zones: string array option
}
