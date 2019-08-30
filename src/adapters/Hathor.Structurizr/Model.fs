namespace Hathor.Structurizr

open Hathor

type Position = int * int

type Size =
| A6_Landscape
| A6_Portrait
| A5_Landscape
| A5_Portrait
| A4_Landscape
| A4_Portrait
| A3_Landscape
| A3_Portrait
| A2_Landscape
| A2_Portrait
| Letter_Landscape
| Letter_Portrait
| Legal_Landscape
| Legal_Portrait
| Slide43_Landscape
| Slide169_Landscape

type ColorHex = string

type Shape =
| Box
| RoundedBox
| Circle
| Ellipse
| Folder
| Hexagon
| Person
| Cylinder
| Pipe
| WebBrowser
| MobileDevicePortrait
| MobileDeviceLandscape
| Robot

type ElementStyle = {
    Tag:Tag
    Width:int
    Height:int
    Background:ColorHex
    Color:ColorHex
    FontSize:int
    Opacity:int
    Shape:Shape    
}

type Routing = Direct | Orthogonal

type RelationshipStyle = {
    Tag:Tag
    Position:Position
    Thickness:int
    Width:int
    Height:int
    Color:ColorHex
    FontSize:int
    Dashed:bool
    Routing:Routing
    Opacity:int    
}

type Style = 
| Element of ElementStyle
| Relationship of RelationshipStyle

type UserElement = {
    Name:string
    Description:string
    Tags:Tag list
    Position:Position
}

type UserType =
| Person of UserElement
| Mobile of UserElement
| WebBrowser of UserElement
| DesktopApp of UserElement

type SoftwareSystemElement = {
    Name:string
    Description:string
    Tags:Tag list
    Position:Position
    Containers: ContainerElement list
}

and ContainerElement = {
    Name:string
    Description:string
    Technology:string
    Tags:Tag list
    Position:Position
    Components: ComponentElement list
}

and ComponentElement = {
    Name:string
    Description:string
    Technology:string
    Tags:Tag list
    Position:Position
}

type Element =
| User of UserType
| SoftwareSystem of SoftwareSystemElement
| Container of ContainerElement
| Component of ComponentElement

and SystemViewElement =
| User of UserType
| System of SoftwareSystemElement

and ContainerViewElement =
| User of UserType
| System of SoftwareSystemElement
| Container of ContainerElement

type RelationshipType = {
    Source:Element
    Destination:Element
    Description:string
    Technology:string
    Tags:Tag list
    Vertices:Position list
    Order:int
}


[<AutoOpen>]
module Core =
    let i = ()

module Style =
    open Hathor.FSharp

    let element = {
            Tag = Tag.create "Element"
            Width = 0
            Height = 0
            Background = "#000000"
            Color = "#ffffff"
            FontSize = 0
            Opacity = 0
            Shape = Shape.Box  
        }
    let withColor color (style:ElementStyle) = { style with Color = color }
    let withBackground color (style:ElementStyle) = { style with Background = color }
    let asPerson style = { style with Tag = Tag.create "Person"; Shape = Shape.Person }
    let defaultUser = element |> asPerson |> withBackground "#08427b" |> Style.Element
    let defaultSoftwareSystem = element |> withBackground "#1168bd" |> Style.Element
    let softwareSystemDefaults:Style list = [defaultUser;defaultSoftwareSystem]


[<RequireQualifiedAccess>]
module UserElement =
    let ofUser (user:Hathor.User) =
        let userElement : UserElement = {
            Name = user.Name
            Description = user.Description
            Tags = []
            Position = (0,0)
        }
        userElement

[<RequireQualifiedAccess>]
module UserType =
    let person name desc tags pos = 
        UserType.Person {
            Name = name
            Description = desc
            Tags = tags
            Position = pos
        }

    let mobile name desc tags pos = 
        UserType.Mobile {
            Name = name
            Description = desc
            Tags = tags
            Position = pos
        }

    let webBrowser name desc tags pos = 
        UserType.WebBrowser {
            Name = name
            Description = desc
            Tags = tags
            Position = pos
        }

    let desktopApp name desc tags pos = 
        UserType.DesktopApp {
            Name = name
            Description = desc
            Tags = tags
            Position = pos
        }

    let name person = 
        match person with
        | Person x -> x.Name    
        | WebBrowser x -> x.Name    
        | Mobile x -> x.Name    
        | DesktopApp x -> x.Name    

[<RequireQualifiedAccess>]
module SoftwareSystemElement =
    let init name desc tags pos : SoftwareSystemElement = {
        Name = name
        Description = desc
        Tags = tags
        Position = pos
        Containers = []
    }

    let ofSystem(system:Hathor.SoftwareSystem) =
        let tags = system.Tags |> Set.toList
        init system.Name system.Description tags (0,0)

[<RequireQualifiedAccess>]
module Element =
    let name (e:Element) =
        match e with
        | Element.User x -> UserType.name x
        | Element.SoftwareSystem x -> x.Name
        | Element.Container x -> x.Name
        | Element.Component x -> x.Name

    let equal (e1:Element) (e2:Element) = (e1 |> name) = (e2 |> name)

    let ofSystemViewElement (src:SystemViewElement) : Element =
        match src with
        | SystemViewElement.System el -> Element.SoftwareSystem el
        | SystemViewElement.User el -> Element.User el

[<RequireQualifiedAccess>]
module Relationship =
    let between src desc dest = {
        Source = src
        Destination = dest
        Description = desc
        Technology = ""
        Tags = []
        Vertices = []
        Order = 0
    }
    // api |> Relationship.``with`` "calls" service
    let ``with`` desc dest src = between src desc dest

    let equal (r1:RelationshipType) (r2:RelationshipType) : bool =
        (Element.equal r1.Source r2.Source) && (Element.equal r1.Destination r2.Destination) && (r1.Description = r2.Description)

[<RequireQualifiedAccess>]
module SystemViewElement =
    let name system =
        match system with
        | SystemViewElement.System x -> x.Name
        | SystemViewElement.User x -> UserType.name x

    let equal s1 s2 = (s1 |> name) = (s2 |> name)

    let ofPerson(user:Hathor.User) =
        let u : UserType = Person (user |> UserElement.ofUser)
        let el : SystemViewElement = SystemViewElement.User u
        el
            
