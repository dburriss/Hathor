module Tests

open System
open Xunit
open Hathor
open Hathor.FSharp
open Swensen.Unquote

[<Fact>]
let ``A person`` () =
    let person = A.user "a-name" "A Person" "A description" [] Internal
    test <@ person.Id = "a-name" @>
    test <@ person.Name = "A Person" @>
    test <@ person.Description = "A description" @>
    test <@ person.Location = Internal @>

[<Fact>]
let ``A person with a relationship`` () =
    let person = A.user "a-name" "A Person" "A description" [] Internal
    let system = A.internal_system "a-system" "A System" "A description" []
    let relationship = Relationship.create "a desc of relationship" (person.Id,system.Id) []
    let p1 = User.addRelationship relationship person
    let pRel = p1.Relationships |> Seq.head
    test <@ pRel = relationship @>

[<Fact>]
let ``Can add tags to a person`` () =
    let tag = Tag.create "test"
    let person = 
        A.user "a-name" "A Person" "A description" [] Internal
        |> User.addTag (Tag.create "test")
    
    test <@ person.Tags |> Seq.head = tag @>