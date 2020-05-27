[<AutoOpen>]
module StorageAccountResource

open Newtonsoft.Json
open JsonConverters

type StorageAccount = {
    apiVersion: string
    ``type``: string
    name: string
    sku: Sku
    kind: Kind
    location: string
    tags: string array option
    properties: StorageAccountPropertiesCreateParameters 
}
and Sku = {
    name: SkuName
    tier: SkuTier option
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
Kind = Storage | StorageV2 | BlobStorage | FileStorage | BlockBlobStorage
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
SkuName = Standard_LRS | Standard_GRS | Standard_RAGRS | Standard_ZRS | Premium_LRS | Premium_ZRS | Standard_GZRS | Standard_RAGZRS
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
SkuTier = Standard | Premium
and StorageAccountPropertiesCreateParameters = { 
    customDomain: CustomDomain option
    encryption: Encryption option
    networkAcls: NetworkRuleSet option
    accessTier: AccessTier option
    azureFilesIdentityBasedAuthentication: AzureFilesIdentityBasedAuthentication option
    supportsHttpsTrafficOnly: bool option
    isHnsEnabled: bool option
    largeFileSharesState: State option
    routingPreference: RoutingPreference option
}
and CustomDomain = {
    name: string
    useSubDomainName: bool
}
and Encryption = {
    services: EncryptionServices option
    keySource: KeySource
    keyvaultproperties: KeyVaultProperties option
}
and EncryptionServices = {
    blob: EncryptionService option
    file: EncryptionService option
    table: EncryptionService option
    queue: EncryptionService option
}
and EncryptionService = {
    enabled: bool
    keyType: EncryptionServiceKeyType
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
KeySource = Storage | Keyvault
and KeyVaultProperties = {
    keyname: string option
    keyversion: string option
    keyvaulturi: string option
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
EncryptionServiceKeyType = Service | Account
and NetworkRuleSet = {
    bypass: string option
    virtualNetworkRules: VirtualNetworkRule array option
    ipRules: IPRule array option
    defaultAction: Action
}
and VirtualNetworkRule = {
    id: string
    action: Action option
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
Action = Allow | Deny
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
State = Disabled | Enabled
and IPRule = { 
    value: string
    action: Action option
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
AccessTier = Hot | Cool
and AzureFilesIdentityBasedAuthentication = { 
    directoryServiceOptions: DirectoryServiceOptions
    activeDirectoryProperties: ActiveDirectoryProperties option
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
DirectoryServiceOptions = AADDS | AD
and ActiveDirectoryProperties = {
    domainName: string
    netBiosDomainName: string
    forestName: string
    domainGuid: string
    domainSid: string
    azureStorageSid: string
}
and RoutingPreference = {
    routingChoice: RoutingChoice option
    publishMicrosoftEndpoints: bool option
    publishInternetEndpoints: bool option
}
and [<JsonConverter(typeof<ToStringJsonConverter>)>]
RoutingChoice = MicrosoftRouting | InternetRouting
