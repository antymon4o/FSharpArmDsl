module VirtualMachineScaleSetResource

open NetworkCommon

type VirtualMachineScaleSet = {
    name: string
    ``type``: string
    apiVersion: string
    location: string
    tags: Map<string, string> option
    sku: Sku option
    plan: Plan option
    properties: VirtualMachineScaleSetProperties
    identity: VirtualMachineScaleSetIdentity option
    zones: string array option
}
and Sku = {
    name: string option
    tier: SkuTier option
    capacity: int option
}
and SkuTier = Standard | Basic
and Plan = {
    name: string option
    publisher: string option
    product: string option
    promotionCode: string option
}
and VirtualMachineScaleSetProperties = {
    upgradePolicy: UpgradePolicy option
    automaticRepairsPolicy: AutomaticRepairsPolicy option
    virtualMachineProfile: VirtualMachineScaleSetVMProfile option
    overprovision: bool option
    doNotRunExtensionsOnOverprovisionedVMs: bool option
    singlePlacementGroup: bool option
    zoneBalance: bool option
    platformFaultDomainCount: int option
    proximityPlacementGroup: SubResource option
    additionalCapabilities: AdditionalCapabilities option
    scaleInPolicy: ScaleInPolicy option
}
and VirtualMachineScaleSetIdentity = {
    ``type``: VirtualMachineScaleSetIdentityType option 
    userAssignedIdentities: Map<string, string>
}
and VirtualMachineScaleSetIdentityType = SystemAssigned | UserAssigned | None
and UpgradePolicy = {
    mode: UpgradePolicyMode option
    rollingUpgradePolicy: RollingUpgradePolicy option
    automaticOSUpgradePolicy: AutomaticOSUpgradePolicy option
}
and UpgradePolicyMode = Manual | Automatic
and AutomaticRepairsPolicy = {
    enabled: bool option
    gracePeriod: string option
}
and VirtualMachineScaleSetVMProfile = {
    osProfile: VirtualMachineScaleSetOSProfile option
    storageProfile: VirtualMachineScaleSetStorageProfile option
    networkProfile: VirtualMachineScaleSetNetworkProfile option
    diagnosticsProfile: DiagnosticsProfile option
    extensionProfile: VirtualMachineScaleSetExtensionProfile option
    licenseType: LicenseType option
    priority: Priority option
    evictionPolicy: EvictionPolicy
    billingProfile: BillingProfile option
    scheduledEventsProfile: ScheduledEventsProfile option
}
and LicenseType = Windows_Client | Windows_Server
and Priority = Regular | Low | Spot
and EvictionPolicy = Deallocate | Delete
and AdditionalCapabilities = {
    ultraSSDEnabled: bool option
}
and ScaleInPolicy = {
    rules: ScaleInPolicyRule array option
}
and ScaleInPolicyRule = Default | OldestVM | NewestVM
and RollingUpgradePolicy = {
    maxBatchInstancePercent: int option
    maxUnhealthyInstancePercent: int option
    maxUnhealthyUpgradedInstancePercent: int option
    pauseTimeBetweenBatches: int option
}
and AutomaticOSUpgradePolicy = {
    enableAutomaticOSUpgrade: bool option
    disableAutomaticRollback: bool option
}
and VirtualMachineScaleSetOSProfile = {
    computerNamePrefix: string option
    adminUsername: string option
    adminPassword: string option
    customData: string option
    windowsConfiguration: WindowsConfiguration option
    linuxConfiguration: LinuxConfiguration option
    secrets: VaultSecretGroup array option
}
and VirtualMachineScaleSetStorageProfile = {
    imageReference: ImageReference option
    osDisk: VirtualMachineScaleSetOSDisk option
    dataDisks: VirtualMachineScaleSetDataDisk option
}
and VirtualMachineScaleSetNetworkProfile = {
    healthProbe: ApiEntityReference option
    networkInterfaceConfigurations: VirtualMachineScaleSetNetworkConfiguration array option
}
and DiagnosticsProfile = {
    bootDiagnostics: BootDiagnostics option
}
and VirtualMachineScaleSetExtensionProfile = {
    extensions: VirtualMachineScaleSetExtension array option
}
and BillingProfile = {
    maxPrice: decimal option
}
and ScheduledEventsProfile = {
    terminateNotificationProfile: TerminateNotificationProfile option
}
and WindowsConfiguration = {
    provisionVMAgent: bool option
    enableAutomaticUpdates: bool option
    timeZone: string option
    additionalUnattendContent: AdditionalUnattendContent array option
    winRM: WinRMConfiguration option
}
and LinuxConfiguration = {
    disablePasswordAuthentication: bool option
    ssh: SshConfiguration option
    provisionVMAgent: bool option
}
and VaultSecretGroup = {
    sourceVault: SubResource option
    vaultCertificates: VaultCertificate array option
}
and ImageReference = {
    id: string option
    publisher: string option
    offer: string option
    sku: string option
    version: string option
}
and VirtualMachineScaleSetOSDisk = {
    name: string option
    caching: Caching option     
    writeAcceleratorEnabled: bool option
    createOption: CreateOption
    diffDiskSettings: DiffDiskSettings option
    diskSizeGB: int option
    osType: OsType option 
    image: VirtualHardDisk option
    vhdContainers: string array option
    managedDisk: VirtualMachineScaleSetManagedDiskParameters option
}
and Caching = None | ReadOnly | ReadWrite
and CreateOption = FromImage | Empty | Attach
and OsType = Windows | Linux
and VirtualMachineScaleSetDataDisk = {
    name: string option
    lun: int
    caching: Caching
    writeAcceleratorEnabled: bool option
    createOption: CreateOption
    diskSizeGB: int option
    managedDisk: VirtualMachineScaleSetManagedDiskParameters option
    diskIOPSReadWrite: int option
    diskMBpsReadWrite: int option
}
and ApiEntityReference = { id: string }
and VirtualMachineScaleSetNetworkConfiguration = {
    id: string option
    name: string
    properties: VirtualMachineScaleSetNetworkConfigurationProperties option
}
and BootDiagnostics = {
    enabled: bool option
    storageUri: bool option
}
and VirtualMachineScaleSetExtension = {
    name: string option
    properties: VirtualMachineScaleSetExtensionProperties option
}
and TerminateNotificationProfile = {
    notBeforeTimeout: string option
    enable: bool option
}
and AdditionalUnattendContent = {
    passName: AdditionalUnattendContentPassName option
    componentName: AdditionalUnattendContentComponentName option
    settingName: AdditionalUnattendContentSettingName option
    content: string option
}
and AdditionalUnattendContentPassName = OobeSystem
and AdditionalUnattendContentComponentName = MicrosoftWindowsShellSetup
and AdditionalUnattendContentSettingName = AutoLogon | FirstLogonCommands
and WinRMConfiguration = {
    listeners: WinRMListener array option
}
and SshConfiguration = {
    publicKeys: SshPublicKey array option
}
and VaultCertificate = {
    certificateUrl: string option
    certificateStore: string option
}
and DiffDiskSettings = {
    option: DiffDiskSettingsOption option
    placement: DiffDiskSettingsPlacement option
}
and DiffDiskSettingsOption = Local
and DiffDiskSettingsPlacement = CacheDisk | ResourceDisk
and VirtualHardDisk = { uri: string option }
and VirtualMachineScaleSetManagedDiskParameters = {
    storageAccountType: StorageAccountType option 
    diskEncryptionSet: DiskEncryptionSetParameters option
}
and StorageAccountType = Standard_LRS | Premium_LRS | StandardSSD_LRS |UltraSSD_LRS
and VirtualMachineScaleSetNetworkConfigurationProperties = {
    primary: bool option
    enableAcceleratedNetworking: bool option
    networkSecurityGroup: SubResource option
    dnsSettings:  VirtualMachineScaleSetNetworkConfigurationDnsSettings option
    ipConfigurations: VirtualMachineScaleSetIPConfiguration option
    enableIPForwarding: bool option
}
and VirtualMachineScaleSetExtensionProperties = {
    publisher: string option
    ``type``: string option
    typeHandlerVersion: string option
    autoUpgradeMinorVersion: bool option
    settings: obj option
    protectedSettings: obj option
    provisionAfterExtensions: string array option
}
and WinRMListener = {
    protocol: Protocol option
    certificateUrl: string option
}
and Protocol = Http | Https
and SshPublicKey = {
    path: string option
    keyData: string option
}
and DiskEncryptionSetParameters = { id: string }
and VirtualMachineScaleSetNetworkConfigurationDnsSettings = {
    dnsServers: string array option
}
and VirtualMachineScaleSetIPConfiguration = {
    id: string option
    name: string
    properties: VirtualMachineScaleSetIPConfigurationProperties option
}
and VirtualMachineScaleSetIPConfigurationProperties = {
    subnet: ApiEntityReference option
    primary: bool option
    publicIPAddressConfiguration: VirtualMachineScaleSetPublicIPAddressConfiguration option
    privateIPAddressVersion: IPAddressVersion option
    applicationGatewayBackendAddressPools: SubResource array option
    applicationSecurityGroups: SubResource array option
    loadBalancerBackendAddressPools: SubResource array option
    loadBalancerInboundNatPools: SubResource array option
}
and VirtualMachineScaleSetPublicIPAddressConfiguration = {
    name: string
    properties: VirtualMachineScaleSetPublicIPAddressConfigurationProperties option
}
and VirtualMachineScaleSetPublicIPAddressConfigurationProperties = {
    idleTimeoutInMinutes: int option
    dnsSettings: VirtualMachineScaleSetPublicIPAddressConfigurationDnsSettings option
    ipTags: VirtualMachineScaleSetIpTag array option
    publicIPPrefix: SubResource option
    publicIPAddressVersion: IPAddressVersion option
}
and VirtualMachineScaleSetPublicIPAddressConfigurationDnsSettings = {
    domainNameLabel: string
}
and VirtualMachineScaleSetIpTag = {
    ipTagType: string option
    tag: string option
}