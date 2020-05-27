module StorageAccount

open StorageAccountResource

type StorageAccountCreate = {
    name: string
    kind: Kind
    location: string
    skuName: SkuName
}

let create (input: StorageAccountCreate) = {
        name = input.name
        ``type`` = "Microsoft.Storage/storageAccounts"
        apiVersion = "2019-06-01"
        sku = { name = input.skuName; tier = None }
        kind = input.kind
        location = input.location
        tags = None
        properties = { customDomain = None
                       encryption = None
                       networkAcls = None
                       accessTier = None
                       azureFilesIdentityBasedAuthentication = None
                       supportsHttpsTrafficOnly = None
                       isHnsEnabled = None
                       largeFileSharesState = None
                       routingPreference = None  }
}

let customDomain (customDomain: CustomDomain) (config: StorageAccount) =
    { config with 
        properties = 
        { config.properties with 
            customDomain = Some(customDomain) 
        }
    }


type NetworkAclsParams = 
    | Bypass of string
    | VirtualNetworkRules of VirtualNetworkRule array
    | IpRules of IPRule array

let networkAcls (action: Action) (networkAclsParams: NetworkAclsParams seq) (config: StorageAccount) = 
    let networkAcls = 
        networkAclsParams
        |> Seq.fold (fun s e -> 
            match e with
            | Bypass b -> { s with bypass = Some b }
            | VirtualNetworkRules vnr -> { s with virtualNetworkRules = Some vnr }
            | IpRules ir -> { s with ipRules = Some ir }
        ) { defaultAction = action; bypass = None; virtualNetworkRules = None; ipRules = None }

    { config with
            properties = {
                config.properties with
                    networkAcls = Some networkAcls
            }
    }

let supportsHttpsTrafficOnly (v: bool) (config: StorageAccount) =
    { config with
            properties = {
                config.properties with
                    supportsHttpsTrafficOnly = Some v
            }
    }

type EncryptionServiceParams = 
    | Blob of EncryptionService
    | File of EncryptionService
    | Table of EncryptionService
    | Queue of EncryptionService
let encryptionService (encryptionServiceParams: EncryptionServiceParams seq) (encryptionServices: EncryptionServices) =
    encryptionServiceParams
    |> Seq.fold (fun s e ->
        match e with
        | Blob encrService -> {s with blob = Some encrService}
        | File encrService -> {s with file = Some encrService}
        | Table encrService -> {s with table = Some encrService}
        | Queue encrService -> {s with queue = Some encrService}
    ) encryptionServices

type EncryptionParams =
    | Services of (EncryptionServiceParams seq)
    | Keyvaultproperties of KeyVaultProperties
and ServiceType = Blob | File | Table | Queue

let encryption (keySource: KeySource) (encryptionParams: EncryptionParams seq) (config: StorageAccount) =
    let encryption = 
        encryptionParams
        |> Seq.fold (fun s e -> 
            match e with
            | Services esps -> 
                let services = 
                    s.services 
                    |> Option.defaultWith (fun () -> { blob = None; file = None; table = None; queue = None })
                    |> encryptionService esps
                { s with services = Some services }
            | Keyvaultproperties kvp -> { s with keyvaultproperties = Some kvp }
        ) { keySource = keySource; services = None; keyvaultproperties = None }

    { config with
            properties = {
                config.properties with
                    encryption = Some encryption
            }
    }

