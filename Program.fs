// Learn more about F# at http://fsharp.org

open System
open Newtonsoft.Json
open JsonConverters


[<EntryPoint>]
let main argv =

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

    let blobService = 
        StorageAccountBlobService.create {
            name = "default"
        }
        |> StorageAccountBlobService.deleteRetentionPolicy { enabled = false; days = 0}


    let jss = JsonSerializerSettings()
    jss.NullValueHandling <- NullValueHandling.Ignore
    jss.Converters.Add(new OptionConverter())

    let json = JsonConvert.SerializeObject(c, jss)

    printfn "%s" json

    0 // return an integer exit code
