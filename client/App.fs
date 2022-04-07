module App

open Browser.Dom
open Thoth.Fetch

let messageBody = document.getElementById "messageBody"
let submitButton = document.getElementById "submitButton"
let timeline = document.getElementById "timeline"

promise {
    let! res = Fetch.get "/timeline"
    console.log res
}
|> ignore
