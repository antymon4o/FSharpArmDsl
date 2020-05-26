module ServiceFabricClusterResource

type Cluster = {
    name: string
    ``type``: string
    apiVersion: string
    location: string
    tags: Map<string, string> option
    properties: ClusterProperties 
}
and ClusterProperties = {
    addOnFeatures: AddOnFeatures array option
    azureActiveDirectory: AzureActiveDirectory option
    certificate: CertificateDescription option
    certificateCommonNames: ServerCertificateCommonNames option
    clientCertificateCommonNames: ClientCertificateCommonName array option
    clientCertificateThumbprints:ClientCertificateThumbprint array option
    clusterCodeVersion: string option
    diagnosticsStorageAccountConfig: DiagnosticsStorageAccountConfig option
    eventStoreServiceEnabled: bool option
    fabricSettings: SettingsSectionDescription array option
    managementEndpoint: string
    nodeTypes: NodeTypeDescription array
    reliabilityLevel: ReliabilityLevel option
    reverseProxyCertificate: CertificateDescription option
    reverseProxyCertificateCommonNames: ServerCertificateCommonNames option
    upgradeDescription: ClusterUpgradePolicy option
    upgradeMode: UpgradeMode option
    vmImage: string option
}
and AddOnFeatures = RepairManager | DnsService | BackupRestoreService | ResourceMonitorService
and ReliabilityLevel = None | Bronze | Silver | Gold | Platinum
and UpgradeMode = Automatic | Manual
and AzureActiveDirectory = {
    tenantId: string option
    clusterApplication: string option
    clientApplication: string option
    
}
and CertificateDescription = {
    thumbprint: string
    thumbprintSecondary: string option
    x509StoreName: X509StoreName option
        
}
and X509StoreName = AddressBook | AuthRoot | CertificateAuthority | Disallowed | My | Root | TrustedPeople | TrustedPublisher
and ServerCertificateCommonNames = {
    commonNames: ServerCertificateCommonName array option
    x509StoreName: X509StoreName option    
}
and ClientCertificateCommonName = {
    isAdmin: bool
    certificateCommonName: string
    certificateIssuerThumbprint: string   
}
and ClientCertificateThumbprint = {
    isAdmin: bool
    certificateThumbprint: string
}
and DiagnosticsStorageAccountConfig = {
    storageAccountName: string
    protectedAccountKeyName: string
    protectedAccountKeyName2: string option
    blobEndpoint: string
    queueEndpoint: string
    tableEndpoint: string
}
and SettingsSectionDescription = {
    name: string
    parameters: SettingsParameterDescription array   
}
and NodeTypeDescription = {
    name: string
    placementProperties: Map<string, string> option
    capacities: Map<string, string> option
    clientConnectionEndpointPort: int
    httpGatewayEndpointPort: int
    durabilityLevel: DurabilityLevel option
    applicationPorts: EndpointRangeDescription option
    ephemeralPorts: EndpointRangeDescription option
    isPrimary: bool
    vmInstanceCount: int
    reverseProxyEndpointPort: int option
}
and DurabilityLevel = Bronze | Silver | Gold
and ClusterUpgradePolicy = {
    forceRestart: bool option
    upgradeReplicaSetCheckTimeout: string
    healthCheckWaitDuration: string
    healthCheckStableDuration: string
    healthCheckRetryTimeout: string
    upgradeTimeout: string
    upgradeDomainTimeout: string
    healthPolicy: ClusterHealthPolicy
    deltaHealthPolicy: ClusterUpgradeDeltaHealthPolicy option
}
and ServerCertificateCommonName = {
    certificateCommonName: string
    certificateIssuerThumbprint: string
}
and SettingsParameterDescription = {
    name: string
    value: string
}
and EndpointRangeDescription = {
    startPort: int
    endPort: int
}
and ClusterHealthPolicy = {
    maxPercentUnhealthyNodes: int option
    maxPercentUnhealthyApplications: int option
    applicationHealthPolicies: obj option    
}
and ClusterUpgradeDeltaHealthPolicy = {
    maxPercentDeltaUnhealthyNodes: int
    maxPercentUpgradeDomainDeltaUnhealthyNodes: int
    maxPercentDeltaUnhealthyApplications: int
    applicationDeltaHealthPolicies: obj option    
}
