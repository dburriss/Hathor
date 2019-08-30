namespace Hathor.Structurizr

[<AutoOpen>]
module Diagram =
    
    type SystemLandscapeDiagram = {
        Scope:string
        Description:string
        Size:Size
        Elements: SystemViewElement list
        Relationships: RelationshipType list
        Styles: Style list
    }

    type SystemContextDiagram = {
        Scope:string
        Description:string
        Size:Size
        Elements: SystemViewElement list
        Relationships: RelationshipType list
        Styles: Style list
    }

    type ContainerDiagram = {
        Scope:string
        Description:string
        Size:Size
        Elements: ContainerViewElement list
        Relationships: RelationshipType list
        Styles: Style list
    }

    type ElementDiagram = {
        Scope:string
        Description:string
        Size:Size
        Elements: Element list
        Relationships: RelationshipType list
        Styles: Style list
    }

    type Diagram = 
    | SystemLandscape of SystemLandscapeDiagram
    | SystemContext of SystemContextDiagram
    | Container of ContainerDiagram
    | Component of ElementDiagram
    | Dynamic of ElementDiagram

    
[<RequireQualifiedAccess>]
module SystemLandscapeDiagram = 
    open Diagram

    let init scope desc size : SystemLandscapeDiagram = 
        {
            Scope = scope
            Description = desc
            Size = size
            Elements = []
            Relationships = []
            Styles = Style.softwareSystemDefaults
        }

    let addElement (element:SystemViewElement) (diagram:SystemLandscapeDiagram) =
        let update = fun (x) -> if(SystemViewElement.equal element x) then element else x
        let updated = Update.collectioni update (diagram.Elements |> List.toSeq)|> List.ofSeq |> List.map snd
        if(Update.hasReplaced updated) then
            let elements = updated |> List.map Update.get
            {diagram with Elements = elements}
        else
            let elements = List.append diagram.Elements [element]
            {diagram with Elements = elements}
        
    let addSoftwareSystem (softwareSystem:SoftwareSystemElement) (diagram:SystemLandscapeDiagram) =
        addElement (SystemViewElement.System softwareSystem) diagram

    let addPerson (user:UserType) (diagram:SystemLandscapeDiagram) =
        addElement (SystemViewElement.User user) diagram

    let addRelationship (relationship:RelationshipType) (diagram:SystemLandscapeDiagram) =
        if(diagram.Relationships |> List.exists (fun x -> Relationship.equal x relationship) |> not) then 
            let rels = List.append diagram.Relationships [relationship]
            {diagram with Relationships = rels}
        else diagram

    let findElementByName name (diagram:SystemLandscapeDiagram) = 
        diagram.Elements |> List.tryFind (fun x -> (SystemViewElement.name x) = name)
  