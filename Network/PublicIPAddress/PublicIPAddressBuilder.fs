module NetworkPublicIPAddressBuilder

open NetworkPublicIPAddressResource

type PublicIPAddressCreate = {
    name: string
}

let create name  = {
    name = name
    ``type`` = "Microsoft.Network/publicIPAddresses"
    apiVersion = "2020-04-01"
    location = None
    tags = None
    sku = None
    properties = {
        publicIPAllocationMethod = None
        publicIPAddressVersion = None
        dnsSettings = None
        ddosSettings = None
        ipTags = None
        ipAddress = None
        publicIPPrefix = None
        idleTimeoutInMinutes = None
    }
    zones = None
}

type DnsSettingsParams = 
    | DomainNameLabel of string
    | Fqdn of string
    | ReverseFqdn of string

let dnsSettings (dnsSettingsParams: DnsSettingsParams seq) (pip: PublicIPAddress) =
    { pip with 
        properties = { 
            pip.properties with 
                dnsSettings = 
                    dnsSettingsParams
                    |> Seq.fold (fun (s: NetworkPublicIPAddress.PublicIPAddressDnsSettings) i ->
                        match i with 
                        | DomainNameLabel dnl -> { s with domainNameLabel = Some dnl }
                        | Fqdn f -> { s with fqdn = Some f }
                        | ReverseFqdn rf -> { s with reverseFqdn = Some rf }
                    ) { domainNameLabel = None; fqdn = None; reverseFqdn = None }
                    |> Some
        }
    }