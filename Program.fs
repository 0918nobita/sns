module Program

open System
open Suave
open Suave.Filters
open Suave.Operators

let webServerConfig: SuaveConfig =
    let ip = Net.IPAddress.Parse("0.0.0.0")
    let httpBinding = {
        scheme = HTTP
        socketBinding = {
            ip = ip
            port = 8080us
        }
    }
    let homeFolder = IO.Path.GetFullPath("./public")
    {
        defaultConfig with
            bindings = [httpBinding]
            homeFolder = Some homeFolder
    }

let app: WebPart =
    choose [
        GET >=> path "/" >=> Files.browseFileHome "index.html"
        GET >=> Files.browseHome
        RequestErrors.NOT_FOUND "Page not found"
    ]

[<EntryPoint>]
let main _ =
    startWebServer webServerConfig app
    0
