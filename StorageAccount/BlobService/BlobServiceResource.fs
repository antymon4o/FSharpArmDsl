module StorageAccountBlobServiceResource

open StorageAccountCommon

type BlobService = {
    name: string
    ``type``: string
    apiVersion: string
    properties: BlobServiceProperties 
}
and BlobServiceProperties = {
    cors: CorsRules option
    defaultServiceVersion: string option
    deleteRetentionPolicy: DeleteRetentionPolicy option
    isVersioningEnabled: bool option
    automaticSnapshotPolicyEnabled: bool option
    changeFeed: ChangeFeed option
    restorePolicy: RestorePolicyProperties option
    containerDeleteRetentionPolicy: DeleteRetentionPolicy option
}
and ChangeFeed = {
    enabled: bool
}
and RestorePolicyProperties = {
    enabled: bool
    days: int
}
