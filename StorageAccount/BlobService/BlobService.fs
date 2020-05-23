module StorageAccountBlobService

open StorageAccountBlobServiceResource
open StorageAccountCommon

type BlobServiceCreate = {
    name: string
}

let create (input: BlobServiceCreate) = {
    name = input.name
    ``type`` = "Microsoft.Storage/storageAccounts/blobServices"
    apiVersion = "2019-06-01"
    properties = {
        cors = None
        defaultServiceVersion = None
        deleteRetentionPolicy = None
        isVersioningEnabled = None
        automaticSnapshotPolicyEnabled = None
        changeFeed = None
        restorePolicy = None
        containerDeleteRetentionPolicy = None
    }
}

let cors (corsRule: CorsRule) (config: BlobService) =
    let corsRules = 
        config.properties.cors
        |> StorageAccountCommon.corsRule corsRule
    { config with 
        properties = {
            config.properties with cors = corsRules 
        }
    }

let deleteRetentionPolicy (deleteRetentionPolicy: DeleteRetentionPolicy) (config: BlobService) =
    { config with 
        properties = { 
            config.properties with
                deleteRetentionPolicy = Some deleteRetentionPolicy
        }
    }