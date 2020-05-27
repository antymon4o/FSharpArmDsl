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
        |> probe "FabricGatewayProbe" Tcp 19000 []
        |> probe "FabricHttpGatewayProbe" Tcp 19080 []
        |> inboundNatPool { 
            name = "LoadBalancerBEAddressNatPool" 
            protocol = LoadBalancerProtocol.Tcp
            frontendPortRangeStart = 3389
            frontendPortRangeEnd = 4500
            backendPort = 3389
        } [
            InboundNatPoolParams.FrontendIPConfiguration "[variables('lbIPConfig0')]"
        ]

[<EntryPoint>]
let main argv =

    let jss = JsonSerializerSettings()
    jss.NullValueHandling <- NullValueHandling.Ignore
    jss.Converters.Add(new OptionConverter())

    let json = JsonConvert.SerializeObject(LoadBalancerSF.loadBalancer, jss)

    printfn "%s" json

    0 // return an integer exit code
