module Tests

open System
open Xunit
open Hathor
open Hathor.FSharp
open Swensen.Unquote

[<Fact>]
let ``A person`` () =
    let person = A.Person "a-name" "A Person" "A description" Internal
    test <@ person.Id = "a-name" @>
    test <@ person.Name = "A Person" @>
    test <@ person.Description = "A description" @>
    test <@ person.Location = Internal @>

[<Fact>]
let ``A person with a relationship`` () =
    let person = A.Person "a-name" "A Person" "A description" Internal
    let system = A.System "a-system" "A System" "A description" [] Internal
    let relationship = Relationship.create "a desc of relationship" (person.Id,system.Id) []
    let p1 = Person.addRelationship relationship person
    let pRel = p1.Relationships |> Seq.head
    test <@ pRel = relationship @>

[<Fact>]
let ``Can add tags to a person`` () =
    let tag = Tag.create "test"
    let person = 
        A.Person "a-name" "A Person" "A description" Internal
        |> Person.addTag (Tag.create "test")
    
    test <@ person.Tags |> Seq.head = tag @>