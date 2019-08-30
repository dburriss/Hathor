#r "bin/Debug/netcoreapp2.2/Hathor.dll"
#r "bin/Debug/netcoreapp2.2/Hathor.Structurizr.dll"

open System.IO
open Hathor
open Hathor.Structurizr

let aSystem = A.internal_system "a-system" "A System" "A description of the system" []
let aPerson = A.internal_person "a-name" "A Person" "A user description"

let landscape =
    system_landscape_diagram "ACME landscape" "An example landscape" Size.A2_Landscape {
        system aSystem
        person aPerson
    } 

let filename = __SOURCE_FILE__ |> Path.GetFileNameWithoutExtension |> fun p -> p + ".json"

landscape
|> Diagram.SystemLandscape
|> Json.serialize
|> printfn "%s"
//|> save filename