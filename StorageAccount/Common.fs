module StorageAccountCommon

type CorsRules = {
    corsRules: CorsRule array option
}
and CorsRule = {
    allowedOrigins: string array
    allowedMethods: string array
    maxAgeInSeconds: int
    exposedHeaders: string array
    allowedHeaders: string array
}
and DeleteRetentionPolicy = {
    enabled: bool
    days: int
}


let corsRule (rule: CorsRule) (rules: CorsRules option) =
    match rules with
    | Some cors -> {
        corsRules = 
            cors.corsRules
            |> Option.defaultValue [||]
            |> Array.append [| rule |]
            |> Some
        }
    | None ->
        { corsRules = Some [| rule |] }
    |> Some