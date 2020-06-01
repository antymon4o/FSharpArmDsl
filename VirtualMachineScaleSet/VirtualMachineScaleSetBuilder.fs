module VirtualMachineScaleSetBuilder

open NetworkCommon
open VirtualMachineScaleSetResource

type VirtualMachineScaleSetCreate = {
    name: string
    location: string
}

let create (input: VirtualMachineScaleSetCreate) = {
    apiVersion = "2019-12-01"
    ``type`` = "Microsoft.Compute/virtualMachineScaleSets"
    name = input.name
    location = input.location
    tags = None
    sku = None
    plan = None
    properties = {
        upgradePolicy = None
        automaticRepairsPolicy = None
        virtualMachineProfile = None
        overprovision = None
        doNotRunExtensionsOnOverprovisionedVMs = None
        singlePlacementGroup = None
        zoneBalance = None
        platformFaultDomainCount = None
        proximityPlacementGroup = None
        additionalCapabilities = None
        scaleInPolicy = None
    }
    identity = None
    zones = None
}

type UpgradePolicyParams = 
    | Mode of UpgradePolicyMode
    | RollingUpgradePolicy of RollingUpgradePolicy
    | AutomaticOSUpgradePolicy of  AutomaticOSUpgradePolicy

let overprovision (b: bool) (config: VirtualMachineScaleSet) = { 
    config with
        properties = {
            config.properties with
                overprovision = Some b
        }
    }

let upgradePolicy (paramsSeq: UpgradePolicyParams seq) (config: VirtualMachineScaleSet) =
    let upgradePolicy = 
        config.properties.upgradePolicy 
        |> Option.defaultValue { mode = None; rollingUpgradePolicy = None; automaticOSUpgradePolicy = None }

    {
        config with
            properties = {
                config.properties with
                    upgradePolicy =
                        paramsSeq
                        |> Seq.fold (fun s i ->
                            match i with
                            | Mode m -> { s with mode = Some m }
                            | RollingUpgradePolicy rup -> { s with rollingUpgradePolicy = Some rup }
                            | AutomaticOSUpgradePolicy aup -> { s with automaticOSUpgradePolicy = Some aup }
                        ) upgradePolicy
                        |> Some
            }
    }

type VirtualMachineScaleSetOSProfileParams = 
    | ComputerNamePrefix of string
    | AdminUsername of string
    | AdminPassword of string
    | CustomData of string
    | WindowsConfiguration of WindowsConfiguration
    | LinuxConfiguration of LinuxConfiguration
    | Secrets of VaultSecretGroup
    
let osProfile (paramsSeq: VirtualMachineScaleSetOSProfileParams seq) (config: VirtualMachineScaleSetOSProfile) =
    paramsSeq
    |> Seq.fold (fun s i ->
        match i with
        | ComputerNamePrefix p -> { config with computerNamePrefix = Some p}
        | AdminUsername u -> { config with adminUsername = Some u }
        | AdminPassword p -> { config with adminPassword = Some p}
        | CustomData d -> { config with customData = Some d}
        | WindowsConfiguration wc -> { config with windowsConfiguration = Some wc}
        | LinuxConfiguration lc -> { config with linuxConfiguration = Some lc }
        | Secrets vsg -> { 
            config with 
                secrets = 
                [| vsg |]
                |> Array.append (config.secrets |> Option.defaultValue [||])
                |> Some }
    ) config


type VirtualMachineScaleSetOSDiskParams = 
    | Name of string
    | Caching of Caching
    | WriteAcceleratorEnabled of bool
    | DiffDiskSettings of DiffDiskSettings
    | DiskSizeGB of int
    | OsType of OsType
    | Image of VirtualHardDisk
    | VhdContainers of string
    | ManagedDisk of VirtualMachineScaleSetManagedDiskParameters


let osDisk (parasSeq: VirtualMachineScaleSetOSDiskParams seq) (config: VirtualMachineScaleSetOSDisk) =
    parasSeq
    |> Seq.fold (fun s i ->
        match i with
        | Name n -> { s with VirtualMachineScaleSetOSDisk.name = Some n }
        | Caching c -> { s with caching = Some c }
        | WriteAcceleratorEnabled e -> { s with writeAcceleratorEnabled = Some e }
        | DiffDiskSettings d -> { s with diffDiskSettings = Some d }
        | DiskSizeGB ds -> { s with diskSizeGB = Some ds }
        | OsType t -> { s with osType = Some t }
        | Image vhd -> { s with image = Some vhd }
        | VhdContainers vhdc -> { 
            s with 
                vhdContainers = 
                    [| vhdc |]
                    |> Array.append (s.vhdContainers |> Option.defaultValue [||])
                    |> Some
            }
        | ManagedDisk vmmd -> {
            s with 
                managedDisk = Some vmmd
        }
    ) config



type VirtualMachineScaleSetStorageProfileParams =
    | ImageReference of ImageReference
    | OsDisk of CreateOption * VirtualMachineScaleSetOSDiskParams seq
    | DataDisks of VirtualMachineScaleSetDataDisk


let storageProfile (paramsSeq: VirtualMachineScaleSetStorageProfileParams seq) (config: VirtualMachineScaleSetStorageProfile) =
    paramsSeq
    |> Seq.fold (fun s i ->
        match i with
        | ImageReference i -> { s with imageReference = Some i }
        | OsDisk (co, paramsSeq) -> { 
            s with 
                osDisk = {
                    name = None
                    caching = None
                    writeAcceleratorEnabled = None
                    createOption = co
                    diffDiskSettings = None
                    diskSizeGB = None
                    osType = None
                    image = None
                    vhdContainers = None
                    managedDisk = None
                }
                |> osDisk paramsSeq
                |> Some
            }
        | DataDisks dd -> { s with dataDisks = Some dd }
    ) config

type VirtualMachineScaleSetNetworkConfigurationPropertiesParams = 
    | Primary of bool
    | EnableAcceleratedNetworking of bool
    | NetworkSecurityGroup of string
    | DnsSettings of string array
    | IpConfiguration of string * VirtualMachineScaleSetIPConfigurationParams seq
    | EnableIPForwarding of bool
and VirtualMachineScaleSetIPConfigurationParams = 
    | Subnet of string
    | Primary of bool
    | PublicIPAddressConfiguration of VirtualMachineScaleSetPublicIPAddressConfiguration
    | PrivateIPAddressVersion of IPAddressVersion
    | ApplicationGatewayBackendAddressPools of string
    | ApplicationSecurityGroups of string
    | LoadBalancerBackendAddressPools of string
    | LoadBalancerInboundNatPools of string

let ipConfigurations (paramsSeq: VirtualMachineScaleSetIPConfigurationParams seq) (props: VirtualMachineScaleSetIPConfigurationProperties) =

    let addSubresourceToArray id arrOption = 
        [| {
            SubResource.id = id
        } |]
        |> Array.append ( arrOption |> Option.defaultValue [||] )
        |> Some

    paramsSeq
    |> Seq.fold (fun s i ->
        match i with
        | Subnet sn -> { s with subnet = Some { id = sn} }
        | Primary p -> { s with primary = Some p }
        | PublicIPAddressConfiguration pIP -> { s with publicIPAddressConfiguration = Some pIP}
        | PrivateIPAddressVersion ipV -> { s with privateIPAddressVersion = Some ipV }
        | ApplicationGatewayBackendAddressPools id -> { 
            s with 
                applicationGatewayBackendAddressPools = 
                    addSubresourceToArray id s.applicationGatewayBackendAddressPools }
        | ApplicationSecurityGroups id -> { 
            s with 
                applicationSecurityGroups = 
                    addSubresourceToArray id s.applicationSecurityGroups }
        | LoadBalancerBackendAddressPools id -> { 
            s with 
                loadBalancerBackendAddressPools = 
                    addSubresourceToArray id s.loadBalancerBackendAddressPools }
        | LoadBalancerInboundNatPools id -> { 
            s with 
                loadBalancerInboundNatPools = 
                    addSubresourceToArray id s.loadBalancerInboundNatPools }
    ) props

let virtualMachineScaleSetNetworkConfiguration (name: string) (paramsSeq: VirtualMachineScaleSetNetworkConfigurationPropertiesParams seq) = 
    {
        VirtualMachineScaleSetNetworkConfiguration.id = None
        name = name
        properties = 
            paramsSeq
            |> Seq.fold (fun (s: VirtualMachineScaleSetNetworkConfigurationProperties) i ->
                match i with 
                | VirtualMachineScaleSetNetworkConfigurationPropertiesParams.Primary p -> { s with primary = Some p }
                | EnableAcceleratedNetworking e -> { s with enableAcceleratedNetworking = Some e }
                | NetworkSecurityGroup id -> { s with networkSecurityGroup = Some { id = id } }
                | DnsSettings dns -> { s with dnsSettings = Some { dnsServers = Some dns } }
                | IpConfiguration (name, paramsSeq) -> { 
                    s with 
                        ipConfigurations = {
                                id = None
                                name = name
                                properties = {
                                    subnet = None
                                    primary = None
                                    publicIPAddressConfiguration = None
                                    privateIPAddressVersion = None
                                    applicationGatewayBackendAddressPools = None
                                    applicationSecurityGroups = None
                                    loadBalancerBackendAddressPools = None
                                    loadBalancerInboundNatPools = None
                                }
                                |> ipConfigurations paramsSeq
                                |> Some
                            }
                            |> Array.create 1
                            |> Array.append (s.ipConfigurations |> Option.defaultValue [||]) 
                            |> Some
                    }
                | EnableIPForwarding e -> { s with enableIPForwarding = Some e }
            ) {
                primary = None
                enableAcceleratedNetworking = None
                networkSecurityGroup = None
                dnsSettings = None
                ipConfigurations = None
                enableIPForwarding = None
            }
            |> Some
    }


type VirtualMachineScaleSetNetworkProfileParams = 
    | HealthProbe of ApiEntityReference
    | NetworkInterfaceConfiguration of string * VirtualMachineScaleSetNetworkConfigurationPropertiesParams seq

let networkProfile (paramsSeq: VirtualMachineScaleSetNetworkProfileParams seq) (config: VirtualMachineScaleSetNetworkProfile) =
    paramsSeq
    |> Seq.fold (fun s i ->
        match i with
        | HealthProbe hp -> { s with healthProbe = Some hp }
        | NetworkInterfaceConfiguration (name, paramsSeq) ->
            { s with 
                networkInterfaceConfigurations = 
                    [| virtualMachineScaleSetNetworkConfiguration name paramsSeq |]
                    |> Array.append ( s.networkInterfaceConfigurations |> Option.defaultValue [||] )
                    |> Some }

    ) config

type VirtualMachineScaleSetExtensionPropertiesParams =
    | ``Type`` of string 
    | AutoUpgradeMinorVersion of bool 
    | ProtectedSettings of obj
    | Publisher of string
    | Settings of obj
    | TypeHandlerVersion of string
    | ProvisionAfterExtensions of string array

let extension (name: string) (paramsSeq: VirtualMachineScaleSetExtensionPropertiesParams seq) =
    let emptyProperties = {
        ``type`` = None
        autoUpgradeMinorVersion = None
        protectedSettings = None
        publisher = None
        settings = None
        typeHandlerVersion = None
        provisionAfterExtensions = None
    }

    let properties = 
        paramsSeq
        |> Seq.fold (fun s i ->
            match i with
            | ``Type`` t -> { s with VirtualMachineScaleSetExtensionProperties.``type`` = Some t }
            | AutoUpgradeMinorVersion a -> { s with autoUpgradeMinorVersion = Some a }
            | ProtectedSettings ps -> { s with protectedSettings = Some ps }
            | Publisher pub -> { s with publisher = Some pub }
            | Settings set -> { s with settings = Some set }
            | TypeHandlerVersion thVer -> { s with typeHandlerVersion = Some thVer }
            | ProvisionAfterExtensions provAfter -> { s with provisionAfterExtensions = Some provAfter }
        ) emptyProperties

    {
        VirtualMachineScaleSetExtension.name = Some name
        properties = Some properties
    }

let extensionProfile (name: string) (paramsSeq: VirtualMachineScaleSetExtensionPropertiesParams seq) (config: VirtualMachineScaleSetExtensionProfile)  =    
    {
        config with
            extensions = 
                extension name paramsSeq
                |> Array.create 1
                |> Array.append (config.extensions |> Option.defaultValue [||])
                |> Some
    }    


type VirtualMachineScaleSetVMProfileParams = 
    | OsProfile of VirtualMachineScaleSetOSProfileParams seq
    | StorageProfile of VirtualMachineScaleSetStorageProfileParams seq
    | NetworkProfile of VirtualMachineScaleSetNetworkProfileParams seq
    | DiagnosticsProfile of DiagnosticsProfile
    | Extension of string * VirtualMachineScaleSetExtensionPropertiesParams seq
    | LicenseType of LicenseType
    | Priority of Priority
    | EvictionPolicy of EvictionPolicy
    | BillingProfile of BillingProfile
    | ScheduledEventsProfile of ScheduledEventsProfile

let virtualMachineProfile (paramsSeq: VirtualMachineScaleSetVMProfileParams seq) (config: VirtualMachineScaleSet) =
    let virtualMachineProfile = 
        config.properties.virtualMachineProfile
        |> Option.defaultValue {
            osProfile = None
            storageProfile = None
            networkProfile = None
            diagnosticsProfile = None
            extensionProfile = None
            licenseType = None
            priority = None
            evictionPolicy = None
            billingProfile = None
            scheduledEventsProfile = None
        }

    let newVirtualMachineProfile = 
        paramsSeq
        |> Seq.fold (fun s i ->
            match i with
            | OsProfile paramsSeq -> { 
                s with 
                    osProfile = 
                        s.osProfile 
                        |> Option.defaultValue {
                            computerNamePrefix = None
                            adminUsername = None
                            adminPassword = None
                            customData = None
                            windowsConfiguration = None
                            linuxConfiguration = None
                            secrets = None
                        }
                        |> osProfile paramsSeq
                        |> Some
                }
            | StorageProfile paramsSeq -> {
                s with
                    storageProfile =
                        s.storageProfile
                        |> Option.defaultValue {
                            imageReference = None
                            osDisk = None
                            dataDisks = None
                        }
                        |> storageProfile paramsSeq
                        |> Some
                }
            | NetworkProfile paramsSeq -> {
                s with 
                    networkProfile =
                        s.networkProfile
                        |> Option.defaultValue {
                            VirtualMachineScaleSetNetworkProfile.healthProbe = None
                            VirtualMachineScaleSetNetworkProfile.networkInterfaceConfigurations = None
                        }
                        |> networkProfile paramsSeq 
                        |> Some
                }
            //| DiagnosticsProfile of DiagnosticsProfile
            | Extension (name, paramsSeq) -> {
                s with
                    extensionProfile =
                        s.extensionProfile
                        |> Option.defaultValue { extensions = None }
                        |> extensionProfile name paramsSeq
                        |> Some
                }
            //| LicenseType of LicenseType
            //| Priority of Priority
            //| EvictionPolicy of EvictionPolicy
            //| BillingProfile of BillingProfile
            //| ScheduledEventsProfile of ScheduledEventsProfile

        ) virtualMachineProfile

    { config with
        properties = {
            config.properties with
                virtualMachineProfile = Some newVirtualMachineProfile
        }
    }


