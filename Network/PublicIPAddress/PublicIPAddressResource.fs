module NetworkPublicIPAddressResource

open NetworkPublicIPAddress

type PublicIPAddress = {
    apiVersion: string
    ``type``: string
    name: string
    location: string option
    tags: Map<string, string> option
    sku: PublicIPAddressSku option
    properties: PublicIPAddressProperties
    zones: string array option
}
