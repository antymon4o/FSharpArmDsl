module StorageAccountFileServiceResource

open StorageAccountCommon

type FileService = {
    apiVersion: string
    ``type``: string
    name: string
    properties: FileServiceProperties
}
and FileServiceProperties = {
    cors: CorsRules option
    shareDeleteRetentionPolicy: DeleteRetentionPolicy option
}