module ContainerResource

type Container = {
    apiVersion: string
    ``type``: string
    name: string
    properties: ContainerProperties 
}
and ContainerProperties = {
    publicAccess: PublicAccess
    metadata: Map<string, string>
}
and PublicAccess = Container | Blob | None