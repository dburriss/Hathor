namespace Hathor.Structurizr

[<RequireQualifiedAccess>]
module Json =
    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization
    open Microsoft.FSharp.Reflection

    type JComponent = {
        Type:string
        Name:string
        Description:string
        Technology:string
        Tags:string
        Position:string
    }

    type JContainer = {
        Type:string
        Name:string
        Description:string
        Technology:string
        Tags:string
        Position:string
        Components: JComponent[]
    }

    type JElement = {
        Type:string
        Name:string
        Description:string
        Tags:string
        Position:string
        Containers: JContainer[]
    }

    type JRelationship = {
        Source:string
        Description:string
        Technology:string
        Destination:string
        Tags:string
        Order:string
        Vertices:string[]
    }

    type JStyle = {
        Type:string
        Description:string
        Tag:string
        Width:string
        Height:string
        Background:string
        Color:string
        FontSize:string
        Opacity:string
        Shape:string
        Routing:string
        Dashed:string
        Metadata:string
    }

    type JSystemLandscape = {
        Type:string
        Scope:string
        Description:string
        Size:string
        Elements:JElement[]
        Relationships:JRelationship[]
        Styles:JStyle[]
    }

    let private toString (x:'a) = 
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name

    let private fromString<'a> (s:string) =
        match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
        |[|case|] -> Some(FSharpValue.MakeUnion(case,[||]) :?> 'a)
        |_ -> None

    let intToString i = if(i = 0) then "" else i |> string

    let private positionToString ((x,y):Position) =
        sprintf "%i,%i" x y
    
    let private tagsToString (tags:Tag list) = 
        if(tags |> List.isEmpty) then "" 
        else tags |> List.fold (fun r s -> r + s + ",") ""

    let private userElToJElement (userEl:UserElement) : JElement =
        {
            Type = "Person"
            Name = userEl.Name
            Description = userEl.Description
            Tags = userEl.Tags |> tagsToString
            Position = userEl.Position |> positionToString
            Containers = [||]
        }

    let private toJRelationship (r:Relationship) : JRelationship =
        {
            Source = r.Source |> Element.name
            Description = r.Description
            Technology = r.Technology
            Destination = r.Destination |> Element.name
            Tags = r.Tags |> tagsToString
            Order = r.Order |> string
            Vertices = r.Vertices |> List.map positionToString |> List.toArray
        }

    let private toJStyle (style:Style) : JStyle =
        match style with
        | Style.Element s -> 
            {
                Type = "element"
                Description = "true"
                Tag = s.Tag
                Width = s.Width |> intToString
                Height = s.Height |> intToString
                Background = s.Background
                Color = s.Color
                FontSize = s.FontSize |> intToString
                Opacity = s.Opacity |> intToString
                Shape = s.Shape |> toString
                Routing = null
                Dashed = null
                Metadata = "true"
            }
        | Style.Relationship s ->
            {
                Type = "element"
                Description = "true"
                Tag = s.Tag
                Width = s.Width |> intToString
                Height = s.Height |> intToString
                Background = null
                Color = s.Color
                FontSize = s.FontSize |> string
                Opacity = s.Opacity |> intToString
                Shape = null
                Routing = s.Routing |> toString
                Dashed = s.Dashed |> string
                Metadata = "true"
            }     
        

    let private serializerSettings = 
        let s = JsonSerializerSettings()
        s.ContractResolver <- new CamelCasePropertyNamesContractResolver()
        s.NullValueHandling <- NullValueHandling.Ignore
        s

    let private serializeSystemLandscapeDiagram (diagram:SystemLandscapeDiagram) =
        
        let toJElement (el:SystemViewElement) : JElement =
            match el with
            | SystemViewElement.System x -> 
                {
                    Type = "Software System"
                    Name = x.Name
                    Description = x.Description
                    Tags = x.Tags |> tagsToString
                    Position = x.Position |> positionToString
                    Containers = [||]
                }
            | SystemViewElement.User x -> 
                match x with
                | User.DesktopApp y -> userElToJElement y
                | User.Mobile y -> userElToJElement y
                | User.Person y -> userElToJElement y
                | User.WebBrowser y -> userElToJElement y

        let data : JSystemLandscape = {
            Type = "System Landscape"
            Scope = diagram.Scope
            Description = diagram.Description
            Size = diagram.Size |> toString
            Elements = diagram.Elements |> List.map toJElement |> List.toArray
            Relationships = diagram.Relationships |> List.map toJRelationship |> List.toArray
            Styles = diagram.Styles |> List.map toJStyle |> List.toArray
        }
        JsonConvert.SerializeObject(data, Formatting.Indented, serializerSettings)

    let serialize (diagram:Diagram) =
        match diagram with
        | Diagram.SystemLandscape d -> serializeSystemLandscapeDiagram d
        | _ -> failwith "NotImplemented"