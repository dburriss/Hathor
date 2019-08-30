namespace Hathor.FSharp

open Hathor

module Tag =
    let create s = Tag s
    let single s = [s] |> Set.ofList
    let ssingle s = [Tag s] |> Set.ofList
    let none = Set.empty
    let kv k v = Tag (sprintf "%s:%s" k v)
    let split (Tag s) = 
        if(s.Contains(":")) then
            let arr = s.Split([|':'|])
            let v = arr |> Array.tail |> fun ss -> ss |> String.concat ":"
            (arr.[0],arr.[1])
        else (s,"")
    let toString (Tag s) = s
    let ofList (ts : Tag list) = ts |> Set.ofList
    let toList (ts : Set<Tag>) = ts |> Set.toList

    let Person = Tag "Person"
    let System = Tag "System"

module Relationship =
    
    let create desc between tech =
        let r : Relationship =
            {
                Description = desc
                Between = between
                Technology = tech |> Set.ofList
                Tags = Tag.none
            }
        r

module User =
    let create id name desc tags loc =
        let p : User = 
            {
                Id = id
                Name = name
                Description = desc
                Location = loc
                Relationships = Set.empty
                Tags = tags
            }
        p

    let addRelationship (relationship:Relationship) (person:User) =
        {person with Relationships = person.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (person:User) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        person |> addRelationship relationship

    let addTag (tag:Tag) (person:User) =
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
                Tags = Tag.none
            }
        c

    let addRelationship (relationship:Relationship) (codeElement:CodeElement) =
        {codeElement with Relationships = codeElement.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (codeElement:CodeElement) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        codeElement |> addRelationship relationship

    let addTag (tag:Tag) (codeElement:CodeElement) =
        {codeElement with Tags = codeElement.Tags.Add(tag)}

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
                Tags = Tag.none
            }
        c

    let addRelationship (relationship:Relationship) (comp:Component) =
        {comp with Relationships = comp.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (comp:Component) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        comp |> addRelationship relationship

    let addTag (tag:Tag) (``component``:Component) =
        {``component`` with Tags = ``component``.Tags.Add(tag)}

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
                Tags = Tag.none
            }
        c

    let addRelationship (relationship:Relationship) (container:Container) =
        {container with Relationships = container.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (container:Container) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        container |> addRelationship relationship

    let addTag (tag:Tag) (container:Container) =
        {container with Tags = container.Tags.Add(tag)}

module SoftwareSystem =

    let create id name desc lbls loc =
        let s : SoftwareSystem = 
            { 
                Id = id
                Name = name
                Description = desc
                Labels = lbls
                Location = loc
                Containers = []
                Relationships = Set.empty
                Tags = Tag.none
            }
        s

    let addRelationship (relationship:Relationship) (sys:SoftwareSystem) =
        {sys with Relationships = sys.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (sys:SoftwareSystem) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        sys |> addRelationship relationship

    let addTag (tag:Tag) (sys:SoftwareSystem) =
        {sys with Tags = sys.Tags.Add(tag)}

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
                Tags = Tag.none
            }
        l                           

    let addRelationship (relationship:Relationship) (landscape:Landscape) =
        {landscape with Relationships = landscape.Relationships.Add(relationship)}

    let addRelationshipLink (idFrom:string) (desc:string) (idTo:string) (landscape:Landscape) =
        let relationship = Relationship.create desc (idFrom,idTo) []
        landscape |> addRelationship relationship

    let addTag (tag:Tag) (landscape:Landscape) =
        {landscape with Tags = landscape.Tags.Add(tag)}