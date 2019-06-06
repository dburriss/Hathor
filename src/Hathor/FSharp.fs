namespace Hathor.FSharp

open Hathor

module Tag =
    let create s = Tag s
    let kv k v = Tag (sprintf "%s:%s" k v)
    let split (Tag s) = 
        if(s.Contains(":")) then
            let arr = s.Split([|':'|])
            let v = arr |> Array.tail |> fun ss -> ss |> String.concat ":"
            (arr.[0],arr.[1])
        else (s,"")

module Relationship =
    
    let create desc between tech =
        let r : Relationship =
            {
                Description = desc
                Between = between
                Technology = tech |> Set.ofList
                Tags = Set.empty
            }
        r

module Person =
    let create id name desc loc =
        let p : Person = 
            {
                Id = id
                Name = name
                Description = desc
                Location = loc
                Relationships = Set.empty
                Tags = Set.empty
            }
        p

    let addRelationship (relationship:Relationship) (person:Person) =
        {person with Relationships = person.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (person:Person) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        person |> addRelationship relationship

    let addTag (tag:Tag) (person:Person) =
        {person with Tags = person.Tags.Add(tag)}

module CodeElement =

    let create id name desc fulltype =
        let c : CodeElement = 
            { 
                Id = id
                Name = name
                Description = desc
                FullTypeName = fulltype
                Relationships = Set.empty
                Tags = Set.empty 
            }
        c

    let addRelationship (relationship:Relationship) (codeElement:CodeElement) =
        {codeElement with Relationships = codeElement.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (codeElement:CodeElement) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        codeElement |> addRelationship relationship

module Component =

    let create id name desc tech =
        let c : Component = 
            { 
                Id = id
                Name = name
                Description = desc
                Technology = tech
                CodeElements = Set.empty
                Relationships = Set.empty
                Tags = Set.empty
            }
        c

    let addRelationship (relationship:Relationship) (comp:Component) =
        {comp with Relationships = comp.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (comp:Component) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        comp |> addRelationship relationship

module Container =

    let create id name desc tech =
        let c : Container = 
            {
                Id = id
                Name = name
                Description = desc
                Technology = tech
                Components = []
                Relationships = Set.empty
                Tags = Set.empty 
            }
        c

    let addRelationship (relationship:Relationship) (container:Container) =
        {container with Relationships = container.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (container:Container) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        container |> addRelationship relationship

module SoftwareSystem =

    let create id name desc lbls loc =
        let s : SoftwareSystem = 
            { 
                Id = id
                Name = name
                Description = desc
                Labels = lbls |> Set.ofList
                Location = loc
                Containers = []
                Relationships = Set.empty
                Tags = Set.empty 
            }
        s

    let addRelationship (relationship:Relationship) (sys:SoftwareSystem) =
        {sys with Relationships = sys.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (sys:SoftwareSystem) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        sys |> addRelationship relationship

module Landscape =
    let create id name desc lbls =
        let l : Landscape = 
            { 
                Id = id
                Name = name
                Description = desc
                Labels = lbls
                Systems = []
                Relationships = Set.empty
                Tags = Set.empty 
            }
        l                           

    let addRelationship (relationship:Relationship) (landscape:Landscape) =
        {landscape with Relationships = landscape.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (landscape:Landscape) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        landscape |> addRelationship relationship
