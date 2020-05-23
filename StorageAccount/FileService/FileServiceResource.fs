module StorageAccountFileServiceResource

open StorageAccountCommon

type FileService = {
    name: string
    ``type``: string
    apiVersion: string
    properties: FileServiceProperties
}
and FileServiceProperties = {
    cors: CorsRules option
    shareDeleteRetentionPolicy: DeleteRetentionPolicy option
}