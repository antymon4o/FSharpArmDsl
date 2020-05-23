module StorageAccountFileService

open StorageAccountFileServiceResource
open StorageAccountCommon


type FileServiceCreate = {
    name: string
}

let create (input: FileServiceCreate) = {
    name = input.name
    ``type`` = "Microsoft.Storage/storageAccounts/fileServices"
    apiVersion = "2019-06-01"
    properties = {
        cors = None
        shareDeleteRetentionPolicy = None
    }
}

let cors (corsRule: CorsRule) (config: FileService) =
    let corsRules = 
        config.properties.cors
        |> StorageAccountCommon.corsRule corsRule
    { config with 
        properties = {
            config.properties with cors = corsRules 
        }
    }


let shareDeleteRetentionPolicy (deleteRetentionPolicy: DeleteRetentionPolicy) (config: FileService) =
    { config with 
        properties = { 
            config.properties with
                shareDeleteRetentionPolicy = Some deleteRetentionPolicy
        }
    }
