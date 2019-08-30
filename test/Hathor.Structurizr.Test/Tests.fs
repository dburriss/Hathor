module Tests

open System
open Xunit
open Hathor
open Hathor.FSharp
open Hathor.Structurizr

[<Fact>]
let ``Can define diagram`` () =
    let thePerson = A.user "a-person" "my person" "A test person" ["tag1"] Location.Internal
    let theSystem = A.internal_system "a-system" "my system" "A test system" ["tag1"]

    let landscape = system_landscape_diagram "scope"  "desc"  Size.A2_Landscape {
        system theSystem
        person thePerson
    }

    Assert.NotEmpty(landscape.Elements)


[<Fact>]
let ``Can serialize diagram`` () =
    let thePerson = A.user "a-person" "my person" "A test person" ["tag1"] Location.Internal
    let theSystem = A.internal_system "a-system" "my system" "A test system" ["tag1"]
    let landscape = system_landscape_diagram "scope"  "desc"  Size.A2_Landscape {
        system theSystem
        person thePerson
    }

    let j = landscape
            |> Diagram.SystemLandscape
            |> Json.serialize
    Assert.NotEmpty(j)
