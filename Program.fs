module Program

open Suave

let webServerConfig: SuaveConfig =
    let ip = System.Net.IPAddress.Parse("0.0.0.0")
    let httpBinding = {
        scheme = HTTP
        socketBinding = {
            ip = ip
            port = 8080us
        }
    }
    { defaultConfig with bindings = [httpBinding] }

[<EntryPoint>]
let main _ =
    startWebServer webServerConfig (Successful.OK "Hello World!")
    0
