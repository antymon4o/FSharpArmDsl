// Learn more about F# at http://fsharp.org

open System
open Newtonsoft.Json
open JsonConverters

module StorageAccountBlobServiceSF =
    open StorageAccountBlobService
    
    let blobService = 
        create {
            name = "default"
        }
        |> deleteRetentionPolicy { enabled = false; days = 0 }

module StorageAccountSF = 
    open StorageAccount

    let c =
        create {
            name = "Imeto"
            kind = StorageV2
            location = "Lokacia"
            skuName = Standard_LRS
        }
        |> networkAcls Allow [ 
            Bypass "AzureServices"
            VirtualNetworkRules [||]
        ]
        |> supportsHttpsTrafficOnly false
        |> encryption Storage [
            Services [
                EncryptionServiceParams.File { enabled = true; keyType = Account }
                EncryptionServiceParams.Blob { enabled = true; keyType = Account }
            ]
        ]

module VirtualNetworkSF = 

    open NetworkVirtualNetworkBuilder

    let vnet = 
        create {
            name = "[parameters('virtualNetworkName')]"
            location = "[parameters('computeLocation')]" 
        }
        |> subnet "[parameters('subnet0Name')]" [
            AddressPrefix "[parameters('subnet0Prefix')]"
        ]

module PublicIPAddressSF = 
    
    open NetworkPublicIPAddressBuilder

    let publicIP = 
        create "[concat(parameters('lbIPName'),'-',parameters('vmNodeType0Name'))]"
        |> dnsSettings [  DomainNameLabel "[parameters('dnsName')]" ]


module LoadBalancerSF = 
    
    open NetworkCommon
    open NetworkLoadBalancerResource
    open NetworkLoadBalancerBuilder

    let loadBalancer = 
        create {
            name = "[concat('LB','-', parameters('clusterName'),'-',parameters('vmNodeType0Name'))]"
            location = "[parameters('computeLocation')]"
        }
        |> frontendIPConfiguration "LoadBalancerIPConfig" [
            PublicIPAddress (Id "[resourceId('Microsoft.Network/publicIPAddresses',concat(parameters('lbIPName'),'-',parameters('vmNodeType0Name')))]") ]
        |> backendAddressPool "LoadBalancerBEAddressPool"
        |> loadBalancingRule "LBRule" LoadBalancerProtocol.Tcp 19000 [
            LoadBalancingRuleParams.BackendAddressPool "[variables('lbPoolID0')]"
            LoadBalancingRuleParams.BackendPort 19000
            LoadBalancingRuleParams.FrontendIPConfiguration "[variables('lbIPConfig0')]"
            LoadBalancingRuleParams.IdleTimeoutInMinutes 5
            Probe "[variables('lbProbeID0')]" ]
        |> loadBalancingRule "LBHttpRule" LoadBalancerProtocol.Tcp 19080 [
            LoadBalancingRuleParams.BackendAddressPool "[variables('lbPoolID0')]"
            LoadBalancingRuleParams.BackendPort 19080
            LoadBalancingRuleParams.FrontendIPConfiguration "[variables('lbIPConfig0')]"
            LoadBalancingRuleParams.IdleTimeoutInMinutes 5
            LoadBalancingRuleParams.Probe "[variables('lbHttpProbeID0')]" ]
        |> probe "FabricGatewayProbe" Tcp 19000 [
            ProbeParams.IntervalInSeconds 5
            ProbeParams.NumberOfProbes 2 ]
        |> probe "FabricHttpGatewayProbe" Tcp 19080 [
            ProbeParams.IntervalInSeconds 5
            ProbeParams.NumberOfProbes 2 ]
        |> inboundNatPool { 
            name = "LoadBalancerBEAddressNatPool" 
            protocol = LoadBalancerProtocol.Tcp
            frontendPortRangeStart = 3389
            frontendPortRangeEnd = 4500
            backendPort = 3389
        } [
            InboundNatPoolParams.FrontendIPConfiguration "[variables('lbIPConfig0')]"
        ]

module VirtualMachineScaleSetSF = 
    
    open VirtualMachineScaleSetBuilder
    open VirtualMachineScaleSetResource
    
    let virtualMachineScaleSet =
        create {
            name = "[parameters('vmNodeType0Name')]"
            location = "[parameters('computeLocation')]"
        }
        |> overprovision false
        |> upgradePolicy [ Mode Automatic ]
        |> virtualMachineProfile [
            Extension ("[concat(parameters('vmNodeType0Name'),'_ServiceFabricNode')]", [
                ``Type`` "ServiceFabricNode"
                AutoUpgradeMinorVersion true
                ProtectedSettings 
                    {|
                        StorageAccountKey1 = "[listKeys(resourceId('Microsoft.Storage/storageAccounts', parameters('supportLogStorageAccountName')),'2015-05-01-preview').key1]"
                        StorageAccountKey2 = "[listKeys(resourceId('Microsoft.Storage/storageAccounts', parameters('supportLogStorageAccountName')),'2015-05-01-preview').key2]"
                    |}
                Publisher "Microsoft.Azure.ServiceFabric"
                Settings 
                    {|
                        clusterEndpoint = "[reference(parameters('clusterName')).clusterEndpoint]"
                        nodeTypeRef = "[parameters('vmNodeType0Name')]"
                        dataPath = "D:\\\\SvcFab"
                        durabilityLevel = "Bronze"
                        enableParallelJobs = true
                        nicPrefixOverride = "[parameters('subnet0Prefix')]"
                        certificate = 
                        {|
                            thumbprint = "[parameters('certificateThumbprint')]"
                            x509StoreName = "[parameters('certificateStoreValue')]"
                        |}
                    |}
                TypeHandlerVersion "1.1"
            ])

            Extension ("[concat('VMDiagnosticsVmExt','_vmNodeType0Name')]", [
                ``Type`` "IaaSDiagnostics"
                AutoUpgradeMinorVersion true
                ProtectedSettings 
                    {|
                        storageAccountName = "[parameters('applicationDiagnosticsStorageAccountName')]"
                        storageAccountKey = "[listKeys(resourceId('Microsoft.Storage/storageAccounts', parameters('applicationDiagnosticsStorageAccountName')),'2015-05-01-preview').key1]"
                        storageAccountEndPoint = "https://core.windows.net/"
                    |}
                Publisher "Microsoft.Azure.Diagnostics"
                Settings 
                    {|
                        WadCfg = 
                        {|
                            DiagnosticMonitorConfiguration = 
                            {|
                                overallQuotaInMB = "50000"
                                EtwProviders = 
                                {|
                                    EtwEventSourceProviderConfiguration = [
                                        {|
                                            provider = "Microsoft-ServiceFabric-Actors"
                                            scheduledTransferKeywordFilter = "1"
                                            scheduledTransferPeriod = "PT5M"
                                            DefaultEvents = 
                                            {|
                                                eventDestination = "ServiceFabricReliableActorEventTable"
                                            |}
                                        |} :> obj
                                        {|
                                            provider = "Microsoft-ServiceFabric-Services"
                                            scheduledTransferPeriod = "PT5M"
                                            DefaultEvents = 
                                            {|
                                                eventDestination = "ServiceFabricReliableServiceEventTable"
                                            |}
                                        |} :> obj
                                    ]
                                    EtwManifestProviderConfiguration = [
                                        {|
                                            provider = "cbd93bc2-71e5-4566-b3a7-595d8eeca6e8"
                                            scheduledTransferLogLevelFilter = "Information"
                                            scheduledTransferKeywordFilter = "4611686018427387904"
                                            scheduledTransferPeriod = "PT5M"
                                            DefaultEvents = 
                                            {|
                                                eventDestination = "ServiceFabricSystemEventTable"
                                            |}
                                        |}
                                    ]
                                |}
                            |}
                        |}
                    |}
                TypeHandlerVersion "1.5"
            ])

            NetworkProfile [
                NetworkInterfaceConfiguration ("[concat(parameters('nicName'), '-0')]", [
                    IpConfiguration ("[concat(parameters('nicName'),'-',0)]", [
                        LoadBalancerBackendAddressPools "[variables('lbPoolID0')]"
                        LoadBalancerInboundNatPools "[variables('lbNatPoolID0')]"
                        Subnet "[variables('subnet0Ref')]"
                    ])
                    VirtualMachineScaleSetNetworkConfigurationPropertiesParams.Primary true
                ])
            ]

            OsProfile [
                AdminPassword "[parameters('adminPassword')]"
                AdminUsername "[parameters('adminUsername')]"
                ComputerNamePrefix "[parameters('vmNodeType0Name')]"
                Secrets {
                    sourceVault = Some { id = "[parameters('sourceVaultValue')]" }
                    vaultCertificates = Some [|
                        { 
                            certificateUrl = Some "[parameters('certificateUrlValue')]"
                            certificateStore = Some "[parameters('certificateStoreValue')]" 
                        }
                    |]
                }
            ]

            StorageProfile [
                ImageReference {
                    id = None
                    publisher = Some "[parameters('vmImagePublisher')]"
                    offer = Some "[parameters('vmImageOffer')]"
                    sku = Some "[parameters('vmImageSku')]"
                    version = Some "[parameters('vmImageVersion')]"
                }
                OsDisk (CreateOption.FromImage, [
                    Caching ReadOnly
                    ManagedDisk {
                        storageAccountType = Some StandardSSD_LRS
                        diskEncryptionSet = None
                    }
                ])
            ]
        ]
[<EntryPoint>]
let main argv =

    let jss = JsonSerializerSettings()
    jss.NullValueHandling <- NullValueHandling.Ignore
    jss.Converters.Add(new OptionConverter())

    let json = JsonConvert.SerializeObject(VirtualMachineScaleSetSF.virtualMachineScaleSet, jss)

    printfn "%s" json

    0 // return an integer exit code
