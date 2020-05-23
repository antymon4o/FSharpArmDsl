module ContainerResource

type Container = {
    name: string
    ``type``: string
    apiVersion: string
    properties: ContainerProperties 
}
and ContainerProperties = {
    publicAccess: PublicAccess
    metadata: Map<string, string>
}
and PublicAccess = Container | Blob | None